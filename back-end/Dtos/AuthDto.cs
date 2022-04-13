using back_end.Models;
using System;
using System.Collections.Generic;

namespace back_end.Dtos
{
    public class AuthDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }
        public byte[] Photo { get; set; }
        public int? Rating { get; set; } = 200;
        public virtual ICollection<GameSessionsUsersRole> GameSessionsUsersRoles { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }


    }
}
