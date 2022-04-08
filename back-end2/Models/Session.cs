using System;
using System.Collections.Generic;

namespace back_end.Models
{
    public partial class Session
    {
        public int Id { get; set; }
        public string SessionDescription { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
