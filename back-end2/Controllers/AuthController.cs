using back_end.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
       
        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register(UserDto userDto)
        {
            User? user = await _authService.Register(userDto);
            if (user==null)
            {
                return BadRequest("This username has already been taken");
            }
            return Ok(user);
            //return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + user.Id, user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto user)
        {
            int val=await _authService.Login(user);
            if (val==-1)
            {
                return BadRequest("Username not found!");
            }
            else if (val==0)
            {
                return BadRequest("Wrong password");
            }
            return Ok("Success");
          //  return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + user.Id, user);
        }
    }
}
