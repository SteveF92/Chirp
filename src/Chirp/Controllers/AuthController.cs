using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Chirp.Models;
using Chirp.ViewModels;
using System.Security.Claims;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Net;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Chirp.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<ChirpUser> m_signInManager;
        private UserManager<ChirpUser> m_userManager;
        private ILogger<AuthController> m_logger;

        public AuthController(SignInManager<ChirpUser> a_signInManager, UserManager<ChirpUser> a_userManager, ILogger<AuthController> a_logger)
        {
            m_signInManager = a_signInManager;
            m_userManager = a_userManager;
            m_logger = a_logger;
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

            var signInResult = await m_signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);
            if (!signInResult.Succeeded)
            {
                return Json(new { error = "Username or password incorrect" });
            }

            if (!String.IsNullOrWhiteSpace(returnUrl))
            {
                return Json(new { url = returnUrl });
            }

            return Json(new { url = "/app/index" });
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
            if (ModelState.IsValid)
            {
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
                if (signUpResult.Succeeded)
                {
                    var signInResult = await m_signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);
                    if (signInResult.Succeeded)
                    {
                        return Json(new { url = "/app/index" });
                    }
                }
                else
                {
                    return Json(new { error = "Unknown sign up error." });
                }
            }

            return Json(new { error = "Unknown sign up error." });
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
    }
}
