using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class PlayerIngameStatus
    {
        public PlayerIngameStatus()
        {
            BotResponses = new HashSet<BotResponse>();
            GameSessionsUsersRoles = new HashSet<GameSessionsUsersRole>();
        }

        public int Id { get; set; }
        public string Status { get; set; }

        public virtual ICollection<BotResponse> BotResponses { get; set; }
        public virtual ICollection<GameSessionsUsersRole> GameSessionsUsersRoles { get; set; }
    }
}
