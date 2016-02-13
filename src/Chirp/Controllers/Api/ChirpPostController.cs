﻿using AutoMapper;
using Chirp.Hubs;
using Chirp.Models;
using Chirp.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Chirp.Controllers.Api
{
    [Microsoft.AspNet.Authorization.Authorize]
    [Route("api/chirpposts")]
    public class ChirpPostController : Controller
    {
        private ILogger m_logger;
        private IChirpRepository m_repository;
        private IConnectionManager m_connectionManager;

        public ChirpPostController(IChirpRepository a_repository, ILogger<ChirpPostController> a_logger, IConnectionManager a_connectionManager)
        {
            m_repository = a_repository;
            m_logger = a_logger;
            m_connectionManager = a_connectionManager;
        }

        [HttpGet("")]
        public JsonResult Get()
        {
            var results = Mapper.Map<IEnumerable<ChirpPostViewModel>>(m_repository.GetAllPosts());
            return Json(results);
        }

        [HttpPost("")]
        public JsonResult Post([FromBody]ChirpPostViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newPost = Mapper.Map<ChirpPost>(vm);

                    newPost.PostTime = DateTimeOffset.UtcNow;

                    //Save to the database
                    m_logger.LogInformation("Attemting to save a new post");
                    m_repository.AddPost(newPost);

                    if (m_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;

                        IHubContext context = m_connectionManager.GetHubContext<ChirpPostHub>();
                        context.Clients.All.RefreshChirps();

                        return Json(Mapper.Map<ChirpPostViewModel>(newPost));
                    }
                }

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = "Failed", ModelState = ModelState });
            }
            catch (Exception ex)
            {
                m_logger.LogError("Failed to save new post", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message});
            }
            
        }
    }
}
