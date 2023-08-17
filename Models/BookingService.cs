using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class BookingService
    {
        public BookingService()
        {
            ServiceEvidences = new HashSet<ServiceEvidence>();
        }

        public int Id { get; set; }
        public int? StaffId { get; set; }
        public double? Price { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int? Quantity { get; set; }
        public int? BookingId { get; set; }
        public int? ServiceId { get; set; }
        public bool? Status { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Service Service { get; set; }
        public virtual staff Staff { get; set; }
        public virtual ICollection<ServiceEvidence> ServiceEvidences { get; set; }
    }
}
