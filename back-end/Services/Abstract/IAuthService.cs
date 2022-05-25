using back_end.Dtos;
using back_end.Models;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IAuthService
    {
        Task<string> Register(AuthDto authDto);
        Task<string> Login(AuthDto authDto);
        Task<string> UpdateLogin(int id,AuthDto userDto);
    }
}
