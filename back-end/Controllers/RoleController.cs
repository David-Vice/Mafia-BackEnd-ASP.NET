using back_end.Models;
using back_end.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        
        #region GET REQUESTS

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _roleService.Get(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(role);
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            return Ok(await _roleService.GetAll());
        }


        #endregion


        #region POST REQUESTS

        [HttpPost]
        public async Task<ActionResult> CreateRole(Role role)
        {
            await _roleService.Add(role);
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + role.Id, role);
        }


        #endregion


        #region DELETE REQUESTS

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            await _roleService.Delete(id);
            return Ok();
        }

        #endregion


        #region PUT REQUESTS

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(int id, Role role)
        {
            await _roleService.Update(id, role);
            return Ok();
        }

        #endregion



    }

}