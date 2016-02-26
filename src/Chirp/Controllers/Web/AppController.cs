using AutoMapper;
using Chirp.Database;
using Chirp.Models;
using Chirp.Services;
using Chirp.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Chirp.Controllers.Web
{
    public class AppController : Controller
    {
        private UserManager<ChirpUser> m_userManager;

        public AppController(UserManager<ChirpUser> a_userManager)
        {
            m_userManager = a_userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Route("app/user/{userName}")]
        public async Task<IActionResult> Users(string userName)
        {
            var userFound = await m_userManager.FindByNameAsync(userName);
            if (userFound == null)
            {
                return Json(new { error = "Username or password incorrect" });
            }

            var userViewModel = Mapper.Map<ChirpUserViewModel>(userFound);
            return View("User", userViewModel);
        }

        [Authorize]
        public async Task<IActionResult> MyAccount()
        {
            var user = await m_userManager.FindByIdAsync(User.GetUserId());
            var userViewModel = Mapper.Map<ChirpUserViewModel>(user);
            return View(userViewModel);
        }
    }
}
