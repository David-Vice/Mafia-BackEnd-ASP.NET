using back_end.Dtos;
using back_end.Models;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IAuthService
    {

        Task<User> Register(AuthDto authDto);
        Task<int> Login(AuthDto authDto);
    }
}
