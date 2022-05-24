using back_end.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IUserService
    {
        Task<User> Get(int id);
        Task<IEnumerable<User>> GetAll();
        Task Add(User user);
        Task Delete(int id);
        Task Update(int id,User user);

        Task<byte[]> GetPhoto(int id);
    }
}
