using back_end.Models;
using back_end.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class SessionController : Controller
    {
        private readonly ISessionService _service;
        public SessionController(ISessionService service)
        {
            _service = service;
        }


        #region GET REQUESTS


        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSession(int id)
        {
            var session = await _service.Get(id);
            if (session == null)
            {
                return NotFound();
            }
            return Ok(session);
        }

        [HttpGet]
        public async Task<ActionResult<Session>> GetSessions()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("GetActiveSessions")]
        public async Task<ActionResult<List<Session>>> GetActiveSessions()
        {
            return Ok(await _service.GetOpenSessions());
        }

        #endregion



        #region POST REQUESTS

        [HttpPost]
        public async Task<ActionResult> CreateSession(Session session)
        {
            await _service.Add(session);
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + session.Id, session);
        }

        #endregion


        #region DELETE REQUESTS


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSession(int id)
        {
            await _service.Delete(id);
            return Ok();
        }

        #endregion


        #region PUT REQUESTS


        [HttpPut("EndSession/{id}")]
        public async Task<ActionResult> EndSession(int id)
        {
            await _service.EndSession(id);
            return Ok();
        }

        #endregion


    }

}