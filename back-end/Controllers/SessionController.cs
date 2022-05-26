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

        [HttpGet("GetAdmin/{id}")]
        public async Task<ActionResult> GetAdminIdBySessionId(int id)
        {
            Session session = await _service.Get(id);
            if (session==null)
            {
                return BadRequest("wrong session id");
            }
            return Ok(session.AdminId);
        }

        #endregion



        #region POST REQUESTS

        [HttpPost]
        public async Task<ActionResult> CreateSession(Session session)
        {
            int val=await _service.Add(session);
            if (val < 0)
            {
                return BadRequest("Error");
            }
            return Ok(val);
           // return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + session.Id, session);
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

        [HttpPut("PlayerJoined/{id}")]
        public async Task<ActionResult> PlayerJoined(int id, int userId)
        {
            bool result = await _service.IncrementNumberOfPlayers(id,userId);
            if (result == false) return BadRequest();
            return Ok();
        }


        [HttpPut("PlayerLeft/{id}")]
        public async Task<ActionResult> PlayerLeft(int id)
        {
            bool result = await _service.DecrementNumberOfPlayers(id);
            if (result == false) return BadRequest();
            return Ok();
        }



        #endregion


    }

}