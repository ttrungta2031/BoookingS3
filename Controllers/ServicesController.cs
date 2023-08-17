using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoookingS3.Models;
using BoookingS3.Services;

namespace BoookingS3.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class servicesController : ControllerBase
    {
        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public servicesController(bookings3Context context, IGetallService userservice)
        {
            _context = context;
            _userservice = userservice;
        }



        [HttpGet("Getallservice")]
        public async Task<ActionResult> Getallservice(string status)
        {
            var testall = (from s in _context.Services
                           join st in _context.ServiceTypes on s.ServiceTypeId equals st.Id
                           join sp in _context.Spas on s.SpaId equals sp.Id
                           select new
                           {
                               Id = s.Id,
                               CreateDay = s.CreateDay,
                               ServiceName = s.Name,
                               Duration = s.Duration,
                               Price = s.Price,
                               ServiceTypeName = st.ServiceName,
                               Spaname = sp.Name,
                               SpaAddress = sp.Address,
                               UrlImage = s.UrlImage,
                               Description = s.Description,
                               Status = s.Status,



                           }).ToList();
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Contains("true"))
                {
                    testall = (from s in _context.Services
                               join st in _context.ServiceTypes on s.ServiceTypeId equals st.Id
                               join sp in _context.Spas on s.SpaId equals sp.Id
                               where s.Status == true
                               select new
                               {
                                   Id = s.Id,
                                   CreateDay = s.CreateDay,
                                   ServiceName = s.Name,
                                   Duration = s.Duration,
                                   Price = s.Price,
                                   ServiceTypeName = st.ServiceName,
                                   Spaname = sp.Name,
                                   SpaAddress = sp.Address,
                                   UrlImage = s.UrlImage,
                                   Description = s.Description,
                                   Status = s.Status,



                               }).ToList();
                }
                if (status.Contains("false"))
                {
                    testall = (from s in _context.Services
                               join st in _context.ServiceTypes on s.ServiceTypeId equals st.Id
                               join sp in _context.Spas on s.SpaId equals sp.Id
                               where s.Status == false
                               select new
                               {
                                   Id = s.Id,
                                   CreateDay = s.CreateDay,
                                   ServiceName = s.Name,
                                   Duration = s.Duration,
                                   Price = s.Price,
                                   ServiceTypeName = st.ServiceName,
                                   Spaname = sp.Name,
                                   SpaAddress = sp.Address,
                                   UrlImage = s.UrlImage,
                                   Description = s.Description,
                                   Status = s.Status,



                               }).ToList();
                }
            }






            if (testall == null)
            {
                return NotFound();
            }

            return Ok(new { StatusCode = 200, Message = "Load successful", data = testall });
        }






        // GET: api/Users
        [HttpGet]
        public IActionResult GetAllList(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            try
            {
                var result = _userservice.GetAllService(search, sortby, page, PAGE_SIZE);
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result});
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }

        // GET: api/services/5
        [HttpGet("getbyid")]
        public async Task<ActionResult> GetService(int id)
        {
            var all = _context.Services.AsQueryable();

            all = _context.Services.Where(us => us.Id.Equals(id));
            /* var testall = (from s in _context.Services
                            join st in _context.ServiceTypes on s.ServiceTypeId equals st.Id
                            join sp in _context.Spas on s.SpaId equals sp.Id
                            join bs in _context.BookingServices on s.BookingServiceId equals bs.Id
                            where s.Id == id

                            select new
                            {
                                Id = s.Id,
                                CreateDay = s.CreateDay,
                                ServiceName = s.Name,
                                Duration = s.Duration,
                                Price = s.Price,
                                ServiceTypeName = st.ServiceName,
                                SpaAddress = sp.Address,


                            }).ToList();
             var service = await _context.Services.FindAsync(id);

             if (testall == null)
             {
                 return NotFound();
             }*/
            var result = all.ToList();

            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        }
      

        // PUT: api/services/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update")]
        public async Task<IActionResult> PutService(Service ser)
        {          

            try
            {
                var services = await _context.Services.FindAsync(ser.Id);
                if (services == null)
                {
                    return NotFound();
                }

                services.UrlImage = ser.UrlImage;
                services.Name = ser.Name;
                services.Price = ser.Price;
                services.Duration = ser.Duration;
                services.Status = ser.Status;
                services.SpaId = ser.SpaId;
                services.ServiceTypeId = ser.ServiceTypeId;
                services.Description = ser.Description;

                _context.Services.Update(services);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 201, Message = "Update Successfull" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                return StatusCode(409, new { StatusCode = 409, Message = e.Message });
            }
        }

        // POST: api/services
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            try
            {
                var serv = new Service();
                {
                    serv.UrlImage = service.UrlImage;
                    serv.Name = service.Name;
                    serv.Price = service.Price;
                    serv.Duration = service.Duration;
                    serv.CreateDay = DateTime.Now;
                    serv.Status = true;
                    serv.SpaId = service.SpaId;
                    serv.ServiceTypeId = service.ServiceTypeId;
                    serv.Description = service.Description;
    }
                _context.Services.Add(serv);
                await _context.SaveChangesAsync();


                return Ok(new { StatusCode = 201, Message = "Add Successfull" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                return StatusCode(409, new { StatusCode = 409, Message = e.Message });
            }
        }

        // DELETE: api/services/5
        [HttpDelete]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            if (service.Status == false)
            {
                service.Status = true;
            }
            else if (service.Status == true)
            {
                service.Status = false;
            }
            _context.Services.Update(service);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Content = "The BookingService was accepted successfully completed" });
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
