using Chirp.Models;
using Microsoft.AspNet.Mvc;
using System;
using System.Linq;

namespace Chirp.Controllers.Web
{
    public class AppController : Controller
    {
        private ChirpContext m_context;

        public AppController(ChirpContext a_context)
        {
            m_context = a_context;
        }
        public IActionResult Index()
        {
            var chirpMessages = m_context.ChirpMessages.OrderBy(t => t.PostTime).ToList();
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
