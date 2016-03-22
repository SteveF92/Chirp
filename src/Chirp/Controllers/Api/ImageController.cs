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
using Microsoft.AspNet.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using Chirp.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Chirp.Controllers.Api
{
    public class ImageController : Controller
    {
        private UserManager<ChirpUser> m_userManager;
        private IProfilePictureService m_profilePictureService;

        public ImageController(UserManager<ChirpUser> a_userManager, IProfilePictureService a_profilePictureService)
        {
            m_userManager = a_userManager;
            m_profilePictureService = a_profilePictureService;
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
            string fileName = "";
            if (user.HasProfilePicture)
            {
                fileName = user.Id + ".jpg";
            }
            else
            {
                fileName = "default.png";
            }
            
            return Redirect(awsURL + fileName);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(ICollection<IFormFile> files)
        {
            ChirpUser user = await m_userManager.FindByNameAsync(User.Identity.Name);

            var file = files.First();

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var stream = file.OpenReadStream();


            string contentAsString;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                contentAsString = reader.ReadToEnd();
            }
            string key = user.Id + ".jpg";
            m_profilePictureService.UploadProfilePicture(key, contentAsString);

            user.HasProfilePicture = true;
            await m_userManager.UpdateAsync(user);
            
            return RedirectToAction("ChangeProfilePicture", "Auth");
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
