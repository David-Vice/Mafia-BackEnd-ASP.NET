using back_end.Dtos;

namespace back_end.Services.Abstract
{
    public interface IAuthService
    {

        Task<User?> Register(UserDto user);
        Task<int> Login(UserDto user);
    }
}
