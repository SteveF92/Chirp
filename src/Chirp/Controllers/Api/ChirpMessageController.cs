using Chirp.Models;
using Chirp.ViewModels;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chirp.Controllers.Api
{
    [Route("api/chirpmessages")]
    public class ChirpMessageController : Controller
    {
        private IChirpRepository m_repository;

        public ChirpMessageController(IChirpRepository a_repository)
        {
            m_repository = a_repository;
        }
        [HttpGet("")]
        public JsonResult Get()
        {
            var results = m_repository.GetAllMessages();
            return Json(results);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]ChirpMessageViewModel message)
        {
            if (ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.Created;
                return Json(true);
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
