using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string Hobby { get; set; }
        public string Skin { get; set; }
        public string Gender { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
