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
    public class PlayerInGameStatusController : Controller
    {
        private readonly IPlayerInGameStatusService _playerInGameStatusService;

        public PlayerInGameStatusController(IPlayerInGameStatusService playerInGameStatusService)
        {
            _playerInGameStatusService = playerInGameStatusService;
        }

        #region GET REQUESTS

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerIngameStatus>> GetPlayerInGameStatus(int id)
        {
            var status = await _playerInGameStatusService.Get(id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerIngameStatus>>> GetPlayerInGameStatuses()
        {
            return Ok(await _playerInGameStatusService.GetAll());
        }

        #endregion



        #region POST REQUESTS

        [HttpPost]
        public async Task<ActionResult> CreatePlayerInGameStatus(PlayerIngameStatus playerIngameStatus)
        {
            await _playerInGameStatusService.Add(playerIngameStatus);
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + playerIngameStatus.Id, playerIngameStatus);
        }

        #endregion



        #region DELETE REQUEST

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlayerInGameStatus(int id)
        {
            await _playerInGameStatusService.Delete(id);
            return Ok();
        }

        #endregion



        #region PUT REQUSTS

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePlayerInGameStatus(int id, PlayerIngameStatus playerIngameStatus)
        {
            await _playerInGameStatusService.Update(id, playerIngameStatus);
            return Ok();
        }

        #endregion
    }
}
