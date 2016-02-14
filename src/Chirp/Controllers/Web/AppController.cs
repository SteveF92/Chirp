using Chirp.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Chirp.Controllers.Web
{
    public class AppController : Controller
    {
        private IChirpRepository m_repository;
        private UserManager<ChirpUser> m_userManager;

        public AppController(IChirpRepository a_repository, UserManager<ChirpUser> a_userManager)
        {
            m_repository = a_repository;
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

        [Route("app/users/{userName}")]
        public async Task<IActionResult> Users(string userName)
        {
            var userFound = await m_userManager.FindByNameAsync(userName);
            if (userFound == null)
            {
                return Json(new { error = "Username or password incorrect" });
            }

            return View(userFound);
        }
    }
}
