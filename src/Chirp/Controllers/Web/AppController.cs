using AutoMapper;
using Chirp.Database;
using Chirp.Models;
using Chirp.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net;
using Chirp.PageModels;

namespace Chirp.Controllers.Web
{
    [Route("[action]")]
    public class AppController : Controller
    {
        private UserManager<ChirpUser> m_userManager;

        public AppController(UserManager<ChirpUser> a_userManager)
        {
            m_userManager = a_userManager;
        }

        [Authorize]
        [Route("/")]
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

        [Authorize]
        public IActionResult Signout()
        {
            return View();
        }

        [Authorize]
        [Route("/user/{userName}")]
        [ActionName("user")]
        public async Task<IActionResult> Users(string userName)
        {
            var userFound = await m_userManager.FindByNameAsync(userName);
            if (userFound == null)
            {
                return new HttpNotFoundResult();
            }
            var userViewModel = Mapper.Map<ChirpUserViewModel>(userFound);
            return View("User", userViewModel);
        }

        [Authorize]
        public async Task<IActionResult> MyAccount(string actionTaken)
        {
            var user = await m_userManager.FindByIdAsync(User.GetUserId());
            var userViewModel = Mapper.Map<ChirpUserViewModel>(user);

            AccountSettingsPageModel pageModel = new AccountSettingsPageModel();
            pageModel.SetMessage(actionTaken);
            pageModel.UserVM = userViewModel;

            return View(pageModel);
        }
    }
}
