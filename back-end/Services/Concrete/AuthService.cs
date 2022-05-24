using back_end.Dtos;
using back_end.Handlers;
using back_end.Models;
using back_end.Security;
using back_end.Services.Abstract;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        
        public async Task<string> Login(AuthDto authDto)
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
            userDto.Email = user.Email;
            userDto.Rating = user.Rating;
            userDto.Sessions = user.Sessions;
            userDto.GameSessionsUsersRoles = user.GameSessionsUsersRoles;
            userDto.Photo = user.Photo;

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!My!T0k3n!S3cr3t!K3y"));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            string sessionClaim = "[";
            List<Session> sessions = userDto.Sessions.ToList();

            for (int i = 0; i < sessions.Count; i++)
            {
                sessionClaim += "{ sessionName: \"" + sessions[i].SessionName + "\"";
                sessionClaim += "startTime: \"" + sessions[i].StartTime.ToString() + "\"";
                sessionClaim += "endTime: \"" + sessions[i].EndTime.ToString() + "\" }";
                if (i < sessions.Count - 1)
                {
                    sessionClaim += ",";
                }


            }
            sessionClaim += "]";
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserName", authDto.UserName),
                new Claim("Email", authDto.Email),
                new Claim("RegDate", authDto.RegistrationDate.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("Sessions",sessionClaim),
                //new Claim("Photo", user.Photo),
                new Claim("Rating", authDto.Rating.ToString()),
            };

            JwtSecurityToken token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> Register(AuthDto authDto)
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
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!My!T0k3n!S3cr3t!K3y"));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                string sessionClaim = "[";
                List<Session> sessions = authDto.Sessions.ToList();

                for (int i = 0; i < sessions.Count; i++)
                {
                    sessionClaim += "{ sessionName: \"" + sessions[i].SessionName + "\"";
                    sessionClaim += "startTime: \"" + sessions[i].StartTime.ToString() + "\"";
                    sessionClaim += "endTime: \"" + sessions[i].EndTime.ToString() + "\" }";
                    if (i < sessions.Count - 1)
                    {
                        sessionClaim += ",";
                    }


                }
                sessionClaim += "]";
                List<Claim> claims = new List<Claim>
            {
                new Claim("UserName", authDto.UserName),
                new Claim("Email", authDto.Email),
                new Claim("RegDate", authDto.RegistrationDate.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("Sessions",sessionClaim),
                //new Claim("Photo", user.Photo),
                new Claim("Rating", authDto.Rating.ToString()),
            };

                JwtSecurityToken token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }

        public async Task<string> UpdateLogin(int id,AuthDto authDto)
        {
            var itemToUpdate = await _userService.Get(id);
            if (itemToUpdate == null)
            {
                return null;
            }

            IEnumerable<User> users = await _userService.GetAll();
            if (users.Any(u => u.UserName == authDto.UserName&&u.Id!=id))
            {
                return null;
            }
            else
            {
                //hashing
                byte[] passwordHash;
                byte[] passwordSalt;
                if (authDto.Password != "" && authDto.Password != null)
                {
                    _passwordHasher.CreatePassswordHash(authDto.Password, out passwordHash, out passwordSalt);

                }
                else
                {
                    passwordHash=itemToUpdate.PasswordHash;
                    passwordSalt=itemToUpdate.PasswordSalt;
                }
                authDto.RegistrationDate = itemToUpdate.RegistrationDate;
                authDto.Sessions = itemToUpdate.Sessions;
                if (authDto.Photo==null)
                {
                    authDto.Photo = itemToUpdate.Photo;
                }
                authDto.GameSessionsUsersRoles = itemToUpdate.GameSessionsUsersRoles;
                authDto.Rating = itemToUpdate.Rating;
                User user = new User()
                {
                    Id= id,
                    UserName = authDto.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    RegistrationDate = authDto.RegistrationDate,
                    Email = authDto.Email,
                    Sessions = authDto.Sessions,
                    Photo = authDto.Photo,
                    GameSessionsUsersRoles = authDto.GameSessionsUsersRoles,
                    Rating = authDto.Rating
                    
                };

                await _userService.Update(id, user);
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("!My!T0k3n!S3cr3t!K3y"));
                SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                string sessionClaim = "[";
                List<Session> sessions = authDto.Sessions.ToList();

                for (int i = 0; i < sessions.Count; i++)
                {
                    sessionClaim += "{ sessionName: \"" + sessions[i].SessionName + "\"";
                    sessionClaim += "startTime: \"" + sessions[i].StartTime.ToString() + "\"";
                    sessionClaim += "endTime: \"" + sessions[i].EndTime.ToString() + "\" }";
                    if (i < sessions.Count - 1)
                    {
                        sessionClaim += ",";
                    }


                }
                sessionClaim += "]";
                List<Claim> claims = new List<Claim>
            {
                new Claim("UserName", authDto.UserName),
                new Claim("Email", authDto.Email),
                new Claim("RegDate", authDto.RegistrationDate.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("Sessions",sessionClaim),
                //new Claim("Photo", user.Photo),
                new Claim("Rating", authDto.Rating.ToString()),
            };

                JwtSecurityToken token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}
