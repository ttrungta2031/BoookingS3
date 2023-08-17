using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class StaffService
    {
        public int Id { get; set; }
        public bool? Status { get; set; }
        public string LevelExp { get; set; }
        public int? StaffId { get; set; }
        public int? ServiceId { get; set; }

        public virtual Service Service { get; set; }
        public virtual staff Staff { get; set; }
    }
}
