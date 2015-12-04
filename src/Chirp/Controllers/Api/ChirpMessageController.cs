using AutoMapper;
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
            var results = Mapper.Map<IEnumerable<ChirpMessageViewModel>>(m_repository.GetAllMessages());
            return Json(results);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]ChirpMessageViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newMessage = Mapper.Map<ChirpMessage>(vm);

                    //Save to the database

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    return Json(Mapper.Map<ChirpMessageViewModel>(newMessage));
                }

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message});
            }
            
        }
    }
}
