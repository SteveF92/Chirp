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
using Chirp.Services;
using Microsoft.AspNet.Authorization;

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

            var code = await m_userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var callbackUrl = Url.Action("EmailConfirmed", "Auth", new { userId = newUser.Id, code = code }, protocol: HttpContext.Request.Scheme);
            var emailBody = $"Please confirm your account by clicking this link: <br/> <a href=\"{callbackUrl}\"> {callbackUrl} </a>";
            await m_emailSender.SendEmailAsync(newUser.Email, "Confirm your account", emailBody);

            return Json(new { url = "confirmemailsent" });
        }
    }
}
