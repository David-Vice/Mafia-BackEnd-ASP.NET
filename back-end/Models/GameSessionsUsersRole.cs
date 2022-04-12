using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class GameSessionsUsersRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public int RoleId { get; set; }
        public int PlayerIngameStatusId { get; set; }

        public virtual PlayerIngameStatus PlayerIngameStatus { get; set; }
        public virtual Role Role { get; set; }
        public virtual Session Session { get; set; }
        public virtual User User { get; set; }
    }
}
