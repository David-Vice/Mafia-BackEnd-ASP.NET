using Microsoft.AspNetCore.Mvc;
using back_end.Services.Abstract;
using back_end.Services.Concrete;
using back_end.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class GameSessionsUsersRoleController : Controller
    {
        private readonly IGameSessionsUsersRoleService _service;
        public GameSessionsUsersRoleController(IGameSessionsUsersRoleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameSessionsUsersRole>>> GetGameSessionsUsersRoles()
        {
            return Ok(await _service.GetAll());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GameSessionsUsersRole>> GetGameSessionsUsersRole(int id)
        {
            var gameSessionsUsersRole = await _service.Get(id);
            if (gameSessionsUsersRole == null)
            {
                return NotFound();
            }

            return Ok(gameSessionsUsersRole);
        }

        [HttpGet("GetUsernames/{id}")]
        public async Task<ActionResult<List<string>>> GetUsernames(int id)
        {
            return Ok(await _service.GetUsernamesBySessionId(id));
        }


        [HttpPost]
        public async Task<ActionResult> CreateGameSessionsUsersRole(GameSessionsUsersRole gameSessionsUsersRole)
        {
            await _service.Add(gameSessionsUsersRole);
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + gameSessionsUsersRole.Id, gameSessionsUsersRole);
        }

        [HttpPut("DistributeRoles")]
        public async Task<ActionResult> DistributeRoles(int sessionId)
        {
            return Ok(await _service.DistributeRoles(sessionId));
        }
    }
}
