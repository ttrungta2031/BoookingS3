using System;
using System.Collections.Generic;

#nullable disable

namespace BoookingS3.Models
{
    public partial class User
    {
        public User()
        {
            Customers = new HashSet<Customer>();
            Spas = new HashSet<Spa>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public int? Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? CreateDay { get; set; }
        public bool? Status { get; set; }
        public int? RoleId { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Spa> Spas { get; set; }
    }
}
