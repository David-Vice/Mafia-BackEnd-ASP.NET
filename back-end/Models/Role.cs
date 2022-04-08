using System;
using System.Collections.Generic;

namespace back_end.Models
{
    public partial class Role
    {
        public Role()
        {
            GameSessionUsers = new HashSet<GameSessionUser>();
        }

        public int Id { get; set; }
        public string Rolename { get; set; } = null!;

        public virtual ICollection<GameSessionUser> GameSessionUsers { get; set; }
    }
}
