using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class ServiceEvidence
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreateDay { get; set; }
        public int? BookingServiceId { get; set; }

        public virtual BookingService BookingService { get; set; }
    }
}
