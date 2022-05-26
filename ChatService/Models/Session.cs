using System;
using System.Collections.Generic;

#nullable disable

namespace ChatService.Models
{
    public partial class Session
    {
        public Session()
        {
            GameSessionsUsersRoles = new HashSet<GameSessionsUsersRole>();
        }

        public int Id { get; set; }
        public string SessionName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ulong RoleAutoInitialize { get; set; }
        public int AdminId { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MaxNumberOfPlayers { get; set; }

        public virtual User Admin { get; set; }
        public virtual ICollection<GameSessionsUsersRole> GameSessionsUsersRoles { get; set; }
    }
}
