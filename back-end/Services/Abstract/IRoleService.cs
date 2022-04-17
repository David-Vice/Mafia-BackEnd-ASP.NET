using back_end.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IRoleService
    {
        Task<Role> Get(int id);
        Task<IEnumerable<Role>> GetAll();
        Task Add(Role role);
        Task Update(int id, Role role);
        Task Delete(int id);
    }

}