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
using Microsoft.AspNet.Authorization;
using SendGridMessenger;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Chirp.Controllers.Api
{
    public class ImageController : Controller
    {
        private UserManager<ChirpUser> m_userManager;
        public ImageController(UserManager<ChirpUser> a_userManager)
        {
            m_userManager = a_userManager;
        }

        [Route("image/profilepicture/{userName}")]
        public async Task<ActionResult> ProfilePicture(string userName)
        {
            var user = await m_userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new HttpNotFoundResult();
            }

            string awsURL = @"https://s3.amazonaws.com/chirp-profile-pictures/";
            string fileName = user.Id + ".jpg";
            return Redirect(awsURL + fileName);
        }
    }
}
