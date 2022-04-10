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
        
        public async Task<int> Login(UserDto userDto)
        {
            IEnumerable<User> users = await _userService.GetAll();
            User user=users.Where(u=>u.Username.Equals(userDto.Username)).FirstOrDefault();
            if (user == null)
            {
                return -1;
            }
            if (!_passwordHasher.VerifyPassswordHash(userDto.Password,user.PasswordHash,user.PasswordSalt))
            {
                return 0;
            }
            return 1;
        }

        public async Task<User?> Register(UserDto userDto)
        {
            IEnumerable<User> users= await _userService.GetAll();
            if (users.Any(u => u.Username == userDto.Username))
            {
                return null;
            }
            else
            {
                //hashing
                _passwordHasher.CreatePassswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = new User()
                {
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Surname = userDto.Surname,
                    PasswordHash=passwordHash,
                    PasswordSalt=passwordSalt,
                    Name = userDto.Name
                };
                await _userService.Add(user);
                return user;
            }
        }

        
    }
}
