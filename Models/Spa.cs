using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class Spa
    {
        public Spa()
        {
            Bookings = new HashSet<Booking>();
            ServiceTypes = new HashSet<ServiceType>();
            Services = new HashSet<Service>();
            staff = new HashSet<staff>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public string Address { get; set; }
        public bool? Status { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<ServiceType> ServiceTypes { get; set; }
        public virtual ICollection<Service> Services { get; set; }
        public virtual ICollection<staff> staff { get; set; }
    }
}
