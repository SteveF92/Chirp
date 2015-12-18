using Chirp.Models;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;

namespace Chirp.Controllers.Web
{
    public class AppController : Controller
    {
        private IChirpRepository m_repository;

        public AppController(IChirpRepository a_repository)
        {
            m_repository = a_repository;
        }

        [Authorize]
        public IActionResult Index()
        {
            var chirpMessages = m_repository.GetAllMessages();
            return View(chirpMessages);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
