using back_end.Dtos;
using back_end.Models;
using System.Threading.Tasks;

namespace back_end.Services.Abstract
{
    public interface IAuthService
    {

        Task<AuthDto> Register(AuthDto authDto);
        Task<AuthDto> Login(AuthDto authDto);
    }
}
