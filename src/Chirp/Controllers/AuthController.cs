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

namespace Chirp.Controllers
{
    [Route("auth/[action]")]
    public class AuthController : Controller
    {
        private SignInManager<ChirpUser> m_signInManager;
        private UserManager<ChirpUser> m_userManager;
        private ILogger<AuthController> m_logger;
        private IEmailSender m_emailSender;

        public AuthController(SignInManager<ChirpUser> a_signInManager, UserManager<ChirpUser> a_userManager, ILogger<AuthController> a_logger, IEmailSender a_emailSender)
        {
            m_signInManager = a_signInManager;
            m_userManager = a_userManager;
            m_logger = a_logger;
            m_emailSender = a_emailSender;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login([FromBody]LoginViewModel vm, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { error = "Sign in Error" });
            }

            string username;
            if (vm.Username.Contains("@"))
            {
                var userFound = await m_userManager.FindByEmailAsync(vm.Username);
                if (userFound == null)
                {
                    return Json(new { error = "Username or password incorrect" });
                }
                username = userFound.UserName;
            }
            else
            {
                username = vm.Username;
            }

            var signInResult = await m_signInManager.PasswordSignInAsync(username, vm.Password, true, false);
            if (!signInResult.Succeeded)
            {
                return Json(new { error = "Username or password incorrect" });
            }

            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                return Json(new { url = returnUrl });
            }

            return Json(new { url = "" });
        }

        [HttpGet]
        public IActionResult Signup()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }


        [HttpPost]
        public async Task<JsonResult> Signup([FromBody]SignupViewModel vm, string returnUrl)
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

            //var signInResult = await m_signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);
            //if (!signInResult.Succeeded)
            //{
            //    return Json(new { error = "Account Created, but could not sign in." });
            //}

            try
            {
                var code = await m_userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var callbackUrl = Url.Action("EmailConfirmed", "Auth", new { userId = newUser.Id, code = code }, protocol: HttpContext.Request.Scheme);
                var emailBody = $"Please confirm your account by clicking this link: <a href=\"{callbackUrl}\"> {callbackUrl} </a>";
                await m_emailSender.SendEmailAsync(newUser.Email, "Confirm your account", emailBody);
            }
            catch (Exception e)
            {
                throw e;
            }
            
            return Json(new { url = "confirmemailsent" });
        }


        [HttpGet]
        public async Task<JsonResult> CurrentUser()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await m_userManager.FindByIdAsync(User.GetUserId());
                    return Json(Mapper.Map<ChirpUserViewModel>(user));
                }

                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(new { Message = "You are not logged in" });
            }
            catch (Exception ex)
            {
                m_logger.LogError("Failed to get current user", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await m_signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "App");
        }

        public IActionResult ConfirmEmailSent()
        {
            return View();
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EmailConfirmed(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await m_userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await m_userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "EmailConfirmed" : "Error");
        }
    }
}
