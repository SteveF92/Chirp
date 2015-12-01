using Microsoft.AspNet.Mvc;
using System;

namespace Chirp.Controllers.Web
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
