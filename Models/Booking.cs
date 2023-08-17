using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class Booking
    {
        public Booking()
        {
            BookingServices = new HashSet<BookingService>();
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public double? Total { get; set; }
        public bool? Status { get; set; }
        public string Feedback { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string Score { get; set; }
        public DateTime? CreateDay { get; set; }
        public int? SpaId { get; set; }
        public int? CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Spa Spa { get; set; }
        public virtual ICollection<BookingService> BookingServices { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
