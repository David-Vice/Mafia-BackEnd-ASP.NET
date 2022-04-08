using System;
using System.Collections.Generic;

namespace back_end.Models
{
    public partial class GameResult
    {
        public int Id { get; set; }
        public int? GameSessionUsersId { get; set; }
        public ulong IsWinner { get; set; }

        public virtual GameSessionUser? GameSessionUsers { get; set; }
    }
}
