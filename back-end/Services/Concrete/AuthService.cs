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
         
            AuthDto userDto = authDto;
            userDto.Photo = user.Photo;
            userDto.RegistrationDate = user.RegistrationDate;
            userDto.Email=user.Email;
            userDto.Rating = user.Rating;
            userDto.Sessions = user.Sessions;
            userDto.GameSessionsUsersRoles = user.GameSessionsUsersRoles;
            userDto.Photo=user.Photo;
            return userDto;
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
                authDto.GameSessionsUsersRoles=new List<GameSessionsUsersRole>();
                
                User user = new User()
                {
                    UserName = authDto.UserName,
                    PasswordHash=passwordHash,
                    PasswordSalt=passwordSalt,
                    RegistrationDate=authDto.RegistrationDate,
                    Email=authDto.Email,
                    Sessions=authDto.Sessions,
                    Photo=authDto.Photo,
                    GameSessionsUsersRoles=authDto.GameSessionsUsersRoles,
                    Rating=200
                    
                };
                await _userService.Add(user);
                return authDto;
            }
        }

        
    }
}
