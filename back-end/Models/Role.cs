using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class Role
    {
        public Role()
        {
            BotResponses = new HashSet<BotResponse>();
            GameSessionsUsersRoles = new HashSet<GameSessionsUsersRole>();
        }

        public int Id { get; set; }
        public string Role1 { get; set; }
        public string RoleDescription { get; set; }
        public byte[] RolePhoto { get; set; }

        public virtual ICollection<BotResponse> BotResponses { get; set; }
        public virtual ICollection<GameSessionsUsersRole> GameSessionsUsersRoles { get; set; }
    }
}
