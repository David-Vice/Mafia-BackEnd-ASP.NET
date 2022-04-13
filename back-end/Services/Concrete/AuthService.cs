using back_end.Dtos;
using back_end.Models;
using back_end.Security;
using back_end.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(IUserService userService,IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }
        
        public async Task<int> Login(AuthDto authDto)
        {
            IEnumerable<User> users = await _userService.GetAll();
            User user=users.Where(u=>u.UserName.Equals(authDto.UserName)).FirstOrDefault();
            if (user == null)
            {
                return -1;
            }
            if (!_passwordHasher.VerifyPassswordHash(authDto.Password,user.PasswordHash,user.PasswordSalt))
            {
                return 0;
            }
            return user.Id;
        }

        public async Task<User?> Register(AuthDto authDto)
        {
            IEnumerable<User> users= await _userService.GetAll();
            if (users.Any(u => u.UserName == authDto.UserName))
            {
                return null;
            }
            else
            {
                //hashing
                _passwordHasher.CreatePassswordHash(authDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = new User()
                {
                    UserName = authDto.UserName,
                    PasswordHash=passwordHash,
                    PasswordSalt=passwordSalt,
                };
                await _userService.Add(user);
                return user;
            }
        }

        
    }
}
