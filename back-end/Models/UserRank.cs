using System;
using System.Collections.Generic;

#nullable disable

namespace back_end.Models
{
    public partial class UserRank
    {
        public UserRank()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Rank { get; set; }
        public string Description { get; set; }
        public byte[] Badge { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
