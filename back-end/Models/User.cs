using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class User
    {
        public User()
        {
            GameSessionUsers = new HashSet<GameSessionUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<GameSessionUser> GameSessionUsers { get; set; }
    }
}
