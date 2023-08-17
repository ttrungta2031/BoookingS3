using BoookingS3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoookingS3.Services
{
    public class Getallservice : IGetallService
    {
        private readonly bookings3Context _context;
       // public static int PAGE_SIZE { get; set; } = 5;
        public Getallservice(bookings3Context context)
        {
            _context = context;
        }
       
        public  List<User> GetAllUser(string search, string role,string sortby, int page = 1, int PAGE_SIZE = 5)
            {

                #region filter
                //search filter
                var allUsers = _context.Users.AsQueryable();
           

            if (!string.IsNullOrEmpty(role))
            {
                allUsers = _context.Users.Where(us =>  us.Role.Name.Contains(role));

            }


            if (!string.IsNullOrEmpty(search))
                {
                    allUsers = _context.Users.Where(us => us.UserName.Contains(search) && us.Role.Name.Contains(role));

                }
           
        #endregion
        #region sortby
        //sortby
        allUsers = allUsers.OrderBy(us => us.Id);

                if (!string.IsNullOrEmpty(sortby))
                {
                    switch (sortby)
                    {
                        case "id_desc": allUsers = allUsers.OrderByDescending(us => us.Id); break;
                        case "name_asc": allUsers = allUsers.OrderBy(us => us.FullName); break;
                    case "inactive": allUsers = allUsers.Where(us => us.Status == false); break;
                    case "active": allUsers = allUsers.Where(us => us.Status == true); break;
                }
                }
            #endregion
            #region paging
            allUsers = allUsers.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = allUsers.Select(us => new User
            {
                UserName = us.UserName,
                FullName = us.FullName,
                Email = us.Email,
                Address = us.Address,
                Id = us.Id,
                Code = us.Code,
                CreateDay = us.CreateDay,
                Dob = us.Dob,
                Password= us.Password,
                Phone = us.Phone,
                Role = us.Role,
                RoleId = us.RoleId,
                Status = us.Status
            });
            return result.ToList();
        }







        public List<UserRole> GetAllUserRole(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {

            #region filter
            //search filter
            var all = _context.UserRoles.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.UserRoles.Where(us => us.Name.Contains(search));

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                    case "name_asc": all = all.OrderBy(us => us.Name); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new UserRole
            {
                Id = us.Id,
                Name = us.Name

            }); 
            return result.ToList();
        }

        public List<staff> GetAllstaff(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {

            #region filter
            //search filter
            var all = _context.staff.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.staff.Where(us => us.FullName.Contains(search));

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                    case "name_asc": all = all.OrderBy(us => us.FullName); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new staff
            {
                Id = us.Id,
                FullName = us.FullName

            });
            return result.ToList();
        }

        public List<StaffService> GetAllStaffService(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var all = _context.StaffServices.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.StaffServices.Where(us => us.LevelExp.Contains(search));

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                    case "name_asc": all = all.OrderBy(us => us.Staff); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new StaffService
            {
                Id = us.Id,
                Service = us.Service,
                Status = us.Status,
                LevelExp = us.LevelExp


            });
            return result.ToList();
        }

        public List<Spa> GetAllSpa(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var result123 = (from s in _context.Spas
                          join us in _context.Users on s.UserId equals us.Id
                          select new
                          {
                              Id = s.Id,
                              UrlImage = s.UrlImage,
                              Address = s.Address,
                              Status = s.Status,
                              CreateDay = us.CreateDay,
                              Dob = us.Dob,
                              Email = us.Email,
                              FullName = us.FullName,
                              UserName = us.UserName,
                              Phone = us.Phone



                          }).ToList();
            var all = _context.Spas.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.Spas.Include(us => us.User).Where(us => us.Address.Contains(search));
                result123 = (from s in _context.Spas
                             join us in _context.Users on s.UserId equals us.Id
                             where us.UserName.Contains(search)
                             select new
                             {
                                 Id = s.Id,
                                 UrlImage = s.UrlImage,
                                 Address = s.Address,
                                 Status = s.Status,
                                 CreateDay = us.CreateDay,
                                 Dob = us.Dob,
                                 Email = us.Email,
                                 FullName = us.FullName,
                                 UserName = us.UserName,
                                 Phone = us.Phone
                             }).ToList();
            }
            #endregion
            #region sortby
            //sortby
              all = all.OrderBy(us => us.Id);
          



            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
   
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new Spa
            {
                Id = us.Id,
                UrlImage = us.UrlImage,
                Address = us.Address,
                Status = us.Status,
             



            }) ;
            return result.ToList();
        }

        public List<ServiceType> GetAllServiceType(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var all = _context.ServiceTypes.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.ServiceTypes.Where(us => us.ServiceName.Contains(search));

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                    case "name_asc": all = all.OrderBy(us => us.ServiceName); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new ServiceType
            {
                Id = us.Id,
                ServiceName = us.ServiceName,
               

            }) ;
            return result.ToList();
        }

        public List<ServiceEvidence> GetAllServiceEvidence(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var all = _context.ServiceEvidences.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.ServiceEvidences.Where(us => us.ImageUrl.Contains(search));

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new ServiceEvidence
            {
                Id = us.Id,
                CreateDay = us.CreateDay,
                ImageUrl = us.ImageUrl


            });
            return result.ToList();
        }

        public List<Service> GetAllService(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var testall = (from s in _context.Services
                           join st in _context.ServiceTypes on s.ServiceTypeId equals st.Id
                           join sp in _context.Spas on s.SpaId equals sp.Id
                           join bs in _context.BookingServices on s.BookingServiceId equals bs.Id


                           select new
                           {
                               Id = s.Id,
                               CreateDay = s.CreateDay,
                               ServiceName = s.Name,
                               Duration = s.Duration,
                               Price = s.Price,
                               ServiceTypeName = st.ServiceName,
                               SpaAddress = sp.Address,
                               UrlImage = s.UrlImage,
                            //   ServiceType=s.ServiceType,
                               Status = s.Status,



                           }).ToList();


            









            var all = _context.Services.AsQueryable();
            var allz = _context.Services.Include(x => x.ServiceType).Select(x=> x.ServiceType.ServiceName).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.Services.Where(us => us.Name.Contains(search));
              

            }

            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                    case "name_asc": all = all.OrderBy(us => us.Name); break;
                    case "inactive": all = all.Where(us => us.Status == false); break;
                    case "active": all = all.Where(us => us.Status == true); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion

           
            //  var testrs = all.Include(x => x.ServiceTypeId)
            var result = all.Select(us => new Service
            {

                Id = us.Id,
                Name = us.Name,
                CreateDay = us.CreateDay,
                Duration = us.Duration,
                Price = us.Price,
                UrlImage = us.UrlImage,
                Status = us.Status,
                Description = us.Description,
                
            });




            return result.ToList();
        }

        public List<Payment> GetAllPayment(DateTime? search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var all = _context.Payments.AsQueryable();
            if (search !=null)
            {
                all = _context.Payments.Where(us => us.PaymentDay == search);

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new Payment
            {
                Id = us.Id,
                PaymentDay = us.PaymentDay,
                Amount = us.Amount



            });
            return result.ToList();
        }

        public List<Customer> GetAllCustomer(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var all = _context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.Customers.Where(us => us.Hobby.Contains(search));

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new Customer
            {
                Id = us.Id,                
                Gender=us.Gender,
                Hobby=us.Hobby,
                Skin=us.Skin


            });
            return result.ToList();
        }

        public List<BookingService> GetAllBookingService(int? search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var all = _context.BookingServices.AsQueryable();
            if (search != null)
            {
                all = _context.BookingServices.Where(us => us.Id == search );

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion
            var result = all.Select(us => new BookingService
            {
                Id = us.Id,
                Price =us.Price,
                TimeStart=us.TimeStart,
                TimeEnd = us.TimeEnd,
                Quantity = us.Quantity,
                Staff = us.Staff,
                Service = us.Service,
                Booking = us.Booking,
                 Status = us.Status,
                 StaffId = us.StaffId,
                ServiceId = us.ServiceId,
                BookingId = us.BookingId
                 


            });
            return result.ToList();
        }

        public List<Booking> GetAllBooking(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            #region filter
            //search filter
            var all = _context.Bookings.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                all = _context.Bookings.Where(us => us.Feedback.Contains(search));

            }
            #endregion
            #region sortby
            //sortby
            all = all.OrderBy(us => us.Id);

            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby)
                {
                    case "id_desc": all = all.OrderByDescending(us => us.Id); break;
                }
            }
            #endregion
            #region paging
            all = all.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
          
            #endregion
            var result = all.Select(us => new Booking
            {
                Id = us.Id,
                Feedback = us.Feedback,
                Score=us.Score,
                TimeStart =us.TimeStart,
                TimeEnd =us.TimeEnd,
                Total =us.Total,
                CreateDay=us.CreateDay,
                Status = us.Status,
                Spa = us.Spa,
                Customer = us.Customer,
                SpaId = us.SpaId,
                CustomerId = us.CustomerId
            });
            return result.ToList();
        }
    }
}

