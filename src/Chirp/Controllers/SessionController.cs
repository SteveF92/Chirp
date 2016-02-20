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
    public class SessionController : Controller
    {
        private SignInManager<ChirpUser> m_signInManager;
        private UserManager<ChirpUser> m_userManager;
        private ILogger<SessionController> m_logger;

        public SessionController(SignInManager<ChirpUser> a_signInManager, UserManager<ChirpUser> a_userManager, ILogger<SessionController> a_logger)
        {
            m_signInManager = a_signInManager;
            m_userManager = a_userManager;
            m_logger = a_logger;
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

        [HttpPost]
        public async Task<IActionResult> Signout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await m_signInManager.SignOutAsync();
            }

            Response.StatusCode = (int)HttpStatusCode.SeeOther;

            return RedirectToAction("Index", "App");
        }
    }
}
