using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class Service
    {
        public Service()
        {
            BookingServices = new HashSet<BookingService>();
            StaffServices = new HashSet<StaffService>();
        }

        public int Id { get; set; }
        public string UrlImage { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int? Duration { get; set; }
        public DateTime? CreateDay { get; set; }
        public bool? Status { get; set; }
        public int? SpaId { get; set; }
        public int? BookingServiceId { get; set; }
        public int? ServiceTypeId { get; set; }
        public string Description { get; set; }

        public virtual ServiceType ServiceType { get; set; }
        public virtual Spa Spa { get; set; }
        public virtual ICollection<BookingService> BookingServices { get; set; }
        public virtual ICollection<StaffService> StaffServices { get; set; }
    }
}
