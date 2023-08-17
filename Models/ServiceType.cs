using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            Services = new HashSet<Service>();
        }

        public int Id { get; set; }
        public string ServiceName { get; set; }
        public bool? Status { get; set; }
        public int? SpaId { get; set; }

        public virtual Spa Spa { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
