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
        private IChirpRepository m_repository;
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

        public async Task<IActionResult> Users(string userName)
        {
            var path = this.HttpContext.Request.Path.Value;
            var userNameFromPath = path.Substring(path.LastIndexOf('/') + 1);

            var userFound = await m_userManager.FindByNameAsync(userNameFromPath);
            if (userFound == null)
            {
                return Json(new { error = "Username or password incorrect" });
            }

            var userViewModel = Mapper.Map<ChirpUserViewModel>(userFound);
            return View(userViewModel);
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
