using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class User
    {
        public User()
        {
            GameSessionsUsersRoles = new HashSet<GameSessionsUsersRole>();
            Sessions = new HashSet<Session>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime RegistrationDate { get; set; }
        public byte[] Photo { get; set; }
        public int Rating { get; set; }

        public virtual ICollection<GameSessionsUsersRole> GameSessionsUsersRoles { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
