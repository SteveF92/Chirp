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
        public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await m_signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if (signInResult.Succeeded)
                {
                    if (String.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Chirps", "App");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username or password incorrect");
                }
            }

            return View();
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
        public async Task<ActionResult> Signup([FromBody]SignupViewModel vm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (vm.Password != vm.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Passwords must match");
                    return View();
                }

                var userFound = await m_userManager.FindByNameAsync(vm.Username);
                if (userFound != null)
                {
                    ModelState.AddModelError("", "Username taken");
                    return View();
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
                        return RedirectToAction("Index", "App");
                    }
                }
                else
                {
                    if (signUpResult.Errors.First().Code == "DuplicateEmail")
                    {
                        ModelState.AddModelError("", "That email is already in use.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or password incorrect");
                    }
                }
            }

            return View();
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
