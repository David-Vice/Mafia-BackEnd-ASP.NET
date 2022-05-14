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
    public class BotResponceController : Controller
    {
        private readonly IBotResponceService _botResponceService;
        public BotResponceController(IBotResponceService botResponceService)
        {
            _botResponceService = botResponceService;
        }


        #region GET REQUESTS

        [HttpGet("{id}")]
        public async Task<ActionResult<BotResponse>> GetBotResponce(int id)
        {
            var responce = await _botResponceService.Get(id);
            if (responce == null)
            {
                return NotFound();
            }
            return Ok(responce);
        }

        [HttpGet]
        public async Task<ActionResult<BotResponse>> GetBotResponces()
        {
            return Ok(await _botResponceService.GetAll());
        }

        [HttpGet("GetResponceToAction")]
        public async Task<ActionResult<string>> GetResponce(int roleId, int palyerInGameStatusId)
        {
            return Ok(await _botResponceService.GetResponce(roleId, palyerInGameStatusId));
        }

        #endregion



        #region POST REQUESTS

        [HttpPost]
        public async Task<ActionResult> CreateBotResponce(BotResponse response)
        {
            await _botResponceService.Add(response);
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + response.Id, response);
        }

        #endregion



        #region DELETE REQUESTS

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBotResponce(int id)
        {
            await _botResponceService.Delete(id);
            return Ok();
        }

        #endregion



        #region PUT REQUESTS

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBotResponce(int id, BotResponse botResponse)
        {
            await _botResponceService.Update(id, botResponse);
            return Ok();
        }

        #endregion

    }
}
