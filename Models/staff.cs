using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class staff
    {
        public staff()
        {
            BookingServices = new HashSet<BookingService>();
            StaffServices = new HashSet<StaffService>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string Dob { get; set; }
        public int? Phone { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDay { get; set; }
        public bool? Status { get; set; }
        public int? SpaId { get; set; }
        public int? BookingId { get; set; }

        public virtual Spa Spa { get; set; }
        public virtual ICollection<BookingService> BookingServices { get; set; }
        public virtual ICollection<StaffService> StaffServices { get; set; }
    }
}
