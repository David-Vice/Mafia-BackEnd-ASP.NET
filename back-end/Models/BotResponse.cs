using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class BotResponse
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PlayerInGameStatusId { get; set; }
        public string Response { get; set; }

        public virtual PlayerIngameStatus PlayerInGameStatus { get; set; }
        public virtual Role Role { get; set; }
    }
}
