using back_end.Models;
using back_end.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankController : Controller
    {
        private readonly IRankService _rankService;
        public RankController(IRankService rankService)
        {
            _rankService = rankService;
        }
        [HttpGet]
        public async Task<ActionResult<UserRank>> GetUserRankByUserId(int userId){
            return Ok(await _rankService.GetUserRankByUserId(userId));
        }
    }
}
