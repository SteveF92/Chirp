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
        private IEmailSender m_emailSender;

        public AppController(IChirpRepository a_repository, UserManager<ChirpUser> a_userManager, IEmailSender a_emailSender)
        {
            m_repository = a_repository;
            m_userManager = a_userManager;
            m_emailSender = a_emailSender;
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

        [Route("users/{userName}")]
        public async Task<IActionResult> Users(string userName)
        {
            var userFound = await m_userManager.FindByNameAsync(userName);
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
