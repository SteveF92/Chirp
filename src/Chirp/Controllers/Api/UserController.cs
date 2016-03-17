using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Chirp.Database;
using Chirp.ViewModels;
using System.Security.Claims;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;
using Chirp.Models;
using Microsoft.AspNet.Authorization;
using SendGridMessenger;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Chirp.Controllers.Api
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private SignInManager<ChirpUser> m_signInManager;
        private UserManager<ChirpUser> m_userManager;
        private ILogger<UserController> m_logger;
        private IEmailSender m_emailSender;

        public UserController(SignInManager<ChirpUser> a_signInManager, UserManager<ChirpUser> a_userManager, ILogger<UserController> a_logger, IEmailSender a_emailSender)
        {
            m_signInManager = a_signInManager;
            m_userManager = a_userManager;
            m_logger = a_logger;
            m_emailSender = a_emailSender;
        }

        [Route("{userName}")]
        public async Task<JsonResult> Get(string userName)
        {
            var user = await m_userManager.FindByNameAsync(userName);
            var results = Mapper.Map<ChirpUserViewModel>(await m_userManager.FindByIdAsync(user.Id));
            return Json(results);
        }

        [HttpPost]
        public async Task<JsonResult> Post([FromBody]SignupViewModel vm, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { error = "Unknown sign up error." });
            }

            if (vm.Password != vm.ConfirmPassword)
            {
                return Json(new { error = "The passwords you entered did not match." });
            }

            var userFound = await m_userManager.FindByNameAsync(vm.Username);
            if (userFound != null)
            {
                return Json(new { error = "This username is already in use." });
            }

            userFound = await m_userManager.FindByEmailAsync(vm.Email);
            if (userFound != null)
            {
                return Json(new { error = "This email address is already in use." });
            }

            var newUser = new ChirpUser()
            {
                UserName = vm.Username,
                Email = vm.Email
            };

            var signUpResult = await m_userManager.CreateAsync(newUser, vm.Password);
            if (!signUpResult.Succeeded)
            {
                if (signUpResult.Errors.Any())
                {
                    if (signUpResult.Errors.First().Code == "InvalidUserName")
                    {
                        return Json(new { error = "Invalid username. Usernames can only contain letters, numbers, and .-_" });
                    }
                }
                return Json(new { error = "Unknown sign up error." });
            }

            await SendConfirmationEmail(newUser);

            return Json(new { url = "confirmemailsent" });
        }

        [HttpPost ("ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail()
        {
            var user = await m_userManager.FindByIdAsync(User.GetUserId());
            await SendConfirmationEmail(user);
            return RedirectToAction("ConfirmEmailSent", "Auth");
        }

        public async Task SendConfirmationEmail(ChirpUser a_user)
        {
            var code = await m_userManager.GenerateEmailConfirmationTokenAsync(a_user);
            var callbackUrl = Url.Action("EmailConfirmed", "Auth", new { userId = a_user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            var emailBody = $"Please confirm your account by clicking this link: <br/> <a href=\"{callbackUrl}\"> {callbackUrl} </a>";
            await m_emailSender.SendEmailAsync(a_user.Email, "Confirm your account", emailBody);
        }

        [HttpPost("ChangePassword")]
        public async Task<JsonResult> ChangePassword([FromBody]ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { error = "Please fill out all fields." });
            }

            if (vm.NewPassword != vm.ConfirmPassword)
            {
                return Json(new { error = "Passwords do not match." });
            }

            var user = await m_userManager.FindByIdAsync(User.GetUserId());
            if (user != null)
            {
                var result = await m_userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
                if (result.Succeeded)
                {
                    await m_signInManager.SignInAsync(user, isPersistent: false);
                    m_logger.LogInformation(3, "User changed their password successfully.");
                    return Json(new { url = "myaccount?actionTaken=PasswordChanged" });
                }
                return Json(new { error = "Current Password is incorrect." });
            }
            return Json(new { error = "Unknown Error." });
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<JsonResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { url = "/" });
            }
            var user = await m_userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Json(new { url = "/" });
            }
            var result = await m_userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Json(new { url = "/" });
            }
            return Json(new { url = "/PasswordReset" });
        }
    }
}
