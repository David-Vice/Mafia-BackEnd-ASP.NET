using back_end.Dtos;
using back_end.Models;
using back_end.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class UserController : Controller
    {

        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<User>> GetUsers()
        {
            return Ok(await _service.GetAll());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _service.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("GetProfilePhoto/{id}")]
        public async Task<ActionResult<byte[]>> GetPhoto(int id)
        {
            var photo = await _service.GetPhoto(id);
            if (photo == null) return NotFound();
            return Ok(photo);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user)
        {
            await _service.Add(user);
           // return Ok();
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + user.Id, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _service.Delete(id);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser( int id, User user)
        {

            await _service.Update(id,user);
            return Ok();
        }



    }
}
