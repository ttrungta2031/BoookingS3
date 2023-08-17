using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public double? Amount { get; set; }
        public DateTime? PaymentDay { get; set; }
        public int? BookingId { get; set; }

        public virtual Booking Booking { get; set; }
    }
}
