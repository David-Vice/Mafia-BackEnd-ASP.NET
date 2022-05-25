using back_end.Dtos;
using back_end.Models;
using back_end.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<ActionResult<AuthDto>> Register(AuthDto userDto)
        {
            string token = await _authService.Register(userDto);
            if (token == null)
            {
                return BadRequest("This username has already been taken");
            }
            return Ok(token);
            //return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + user.Id, user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AuthDto userDto)
        {
            string token = await _authService.Login(userDto);
            if (token == null)
            {
                return BadRequest("Username or Password is incorrect!");
            }
            
            return Ok(token);
          //  return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + user.Id, user);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLogin(int id,AuthDto userDto)
        {
            string token = await _authService.UpdateLogin(id,userDto);
            if (token == null)
            {
                return BadRequest("This username has already been taken");
            }

            return Ok(token);
            //  return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + user.Id, user);
        }
    }
}
