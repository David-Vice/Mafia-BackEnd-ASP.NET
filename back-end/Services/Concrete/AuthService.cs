using back_end.Dtos;
using back_end.Handlers;
using back_end.Models;
using back_end.Security;
using back_end.Services.Abstract;
using System;
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
        
        public async Task<AuthDto> Login(AuthDto authDto)
        {
            IEnumerable<User> users = await _userService.GetAll();
            User user=users.Where(u=>u.UserName.Equals(authDto.UserName)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            if (!_passwordHasher.VerifyPassswordHash(authDto.Password,user.PasswordHash,user.PasswordSalt))
            {
                return null;
            }
            //тут я пытаюсь облегчить жизнь фронту
            // отправляю дто со всеми нужными полями и сразк отправлю ранк а не только ранк айди

            AuthDto userDto = authDto;
            userDto.Photo = user.Photo;
            userDto.RegistrationDate = user.RegistrationDate;
            userDto.Email=user.Email;
           // userDto.UserRankId=user.UserRankId;

           // userDto.UserRank = user.UserRank;
            return null;
        }

        public async Task<AuthDto> Register(AuthDto authDto)
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
                authDto.RegistrationDate = DateTime.Now;
                authDto.Sessions = new List<Session>();
                authDto.Photo = ImageHandler.ImageToByteArray("mafia.png");
              

                User user = new User()
                {
                    UserName = authDto.UserName,
                    Email = authDto.Email,
                    PasswordHash=passwordHash,
                    PasswordSalt=passwordSalt,

                };
                await _userService.Add(user);
                return null;
            }
        }

        
    }
}
