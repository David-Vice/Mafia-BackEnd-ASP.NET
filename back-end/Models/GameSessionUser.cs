using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class GameSessionUser
    {
        public GameSessionUser()
        {
            GameResults = new HashSet<GameResult>();
        }

        public int Id { get; set; }
        public int? SessionId { get; set; }
        public int? RoleId { get; set; }
        public int? UserId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Session Session { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<GameResult> GameResults { get; set; }
    }
}
