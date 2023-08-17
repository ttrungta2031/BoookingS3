using BoookingS3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoookingS3.Services
{
    public interface IGetallService
    {
        List<User> GetAllUser(string search, string role, string sortby,  int page, int PAGE_SIZE);
        List<UserRole> GetAllUserRole(string search, string sortby, int page, int PAGE_SIZE);
        List<staff> GetAllstaff(string search, string sortby, int page, int PAGE_SIZE);
        List<StaffService> GetAllStaffService(string search, string sortby, int page, int PAGE_SIZE);
        List<Spa> GetAllSpa(string search, string sortby, int page, int PAGE_SIZE);
        List<ServiceType> GetAllServiceType(string search, string sortby, int page, int PAGE_SIZE);

        List<ServiceEvidence> GetAllServiceEvidence(string search, string sortby, int page, int PAGE_SIZE);

        List<Service> GetAllService(string search, string sortby, int page, int PAGE_SIZE);
        List<Payment> GetAllPayment(DateTime? search, string sortby, int page, int PAGE_SIZE);
        List<Customer> GetAllCustomer(string search, string sortby, int page, int PAGE_SIZE);
        List<BookingService> GetAllBookingService(int? search, string sortby, int page, int PAGE_SIZE);
        List<Booking> GetAllBooking(string search, string sortby, int page, int PAGE_SIZE);


    }
}
