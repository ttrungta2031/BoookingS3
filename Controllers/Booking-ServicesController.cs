using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoookingS3.Models;
using BoookingS3.Common;
using BoookingS3.Services;
using Microsoft.AspNetCore.Authorization;

namespace BoookingS3.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class booking_servicesController : ControllerBase
    {

        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public booking_servicesController(bookings3Context context, IGetallService userservice)
        {
            _context = context;
            _userservice = userservice;
        }

        // GET: api/Users
        [HttpGet]
        public IActionResult GetAllList(int? search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            try
            {
                var result = _userservice.GetAllBookingService(search, sortby, page, PAGE_SIZE);
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result, totalitems});
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }


        // GET: api/bookingServices/5
        [HttpGet("Getbyid")]
        public async Task<ActionResult<BookingService>> GetBookingService(int id)
        {
            var bookingService = await _context.BookingServices.FindAsync(id);

            if (bookingService == null)
            {
                return NotFound();
            }

            return bookingService;
        }

        // PUT: api/bookingServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBookingService(int id, BookingService bookingService)
        {
            if (id != bookingService.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookingService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingServiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpGet("Getallappointment")]
        public async Task<ActionResult> GetAllappointment(string status)
        {
            var result = (from bs in _context.BookingServices
                          join bk in _context.Bookings on bs.BookingId equals bk.Id
                          select new
                          {
                              BookingServiceId = bs.Id,
                              Status = bs.Status,
                              Price = bs.Price,
                              ServiceName = bs.Service.Name,
                              SpaName = bk.Spa.Name,
                              TimeStart = bs.TimeStart,
                              TimeEnd = bs.TimeEnd,
                              UrlImage = bk.Spa.UrlImage
                              


                          }).ToList();
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Contains("true"))
                {
                    result = (from bs in _context.BookingServices
                              join bk in _context.Bookings on bs.BookingId equals bk.Id
                              where bs.Status == true
                              select new
                              {
                                  BookingServiceId = bs.Id,
                                  Status = bs.Status,
                                  Price = bs.Price,
                                  ServiceName = bs.Service.Name,
                                  SpaName = bk.Spa.Name,
                                  TimeStart = bs.TimeStart,
                                  TimeEnd = bs.TimeEnd,
                                  UrlImage = bk.Spa.UrlImage
                              }).ToList();
                }
                if (status.Contains("false"))
                {
                    result = (from bs in _context.BookingServices
                              join bk in _context.Bookings on bs.BookingId equals bk.Id
                              where bs.Status == false
                              select new
                              {
                                  BookingServiceId = bs.Id,
                                  Status = bs.Status,
                                  Price = bs.Price,
                                  ServiceName = bs.Service.Name,
                                  SpaName = bk.Spa.Name,
                                  TimeStart = bs.TimeStart,
                                  TimeEnd = bs.TimeEnd,
                                  UrlImage = bk.Spa.UrlImage
                              }).ToList();
                }
            }






            if (result == null)
            {
                return NotFound();
            }

            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        }







        [HttpGet("Getappointment")]
        [Authorize]
        public async Task<ActionResult> Getappointment(int id, string status)
        {
            var result = (from bs in _context.BookingServices
                          join bk in _context.Bookings on bs.BookingId equals bk.Id
                          where bk.CustomerId == id
                          select new
                          {
                              BookingServiceId = bs.Id,
                              Status = bs.Status,
                              Price = bs.Price,
                              ServiceName = bs.Service.Name,
                              SpaName = bk.Spa.Name,
                              TimeStart = bs.TimeStart,
                              TimeEnd = bs.TimeEnd,
                              UrlImage = bk.Spa.UrlImage



                          }).ToList();
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Contains("true"))
                {
                    result = (from bs in _context.BookingServices
                              join bk in _context.Bookings on bs.BookingId equals bk.Id
                              where bk.CustomerId == id && bs.Status == true
                              select new
                              {
                                  BookingServiceId = bs.Id,
                                  Status = bs.Status,
                                  Price = bs.Price,
                                  ServiceName = bs.Service.Name,
                                  SpaName = bk.Spa.Name,
                                  TimeStart = bs.TimeStart,
                                  TimeEnd = bs.TimeEnd,
                                  UrlImage = bk.Spa.UrlImage
                              }).ToList();
                }
                if (status.Contains("false"))
                {
                    result = (from bs in _context.BookingServices
                              join bk in _context.Bookings on bs.BookingId equals bk.Id
                              where bk.CustomerId == id && bs.Status == false
                              select new
                              {
                                  BookingServiceId = bs.Id,
                                  Status = bs.Status,
                                  Price = bs.Price,
                                  ServiceName = bs.Service.Name,
                                  SpaName = bk.Spa.Name,
                                  TimeStart = bs.TimeStart,
                                  TimeEnd = bs.TimeEnd,
                                  UrlImage = bk.Spa.UrlImage
                              }).ToList();
                }
            }


            if (result == null)
            {
                return NotFound();
            }
            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        
        
     }


        [HttpGet("Getspaownerapm")]
        public async Task<ActionResult> Getappointspaowner(int id, string status)
        {
            var result = (from bs in _context.BookingServices
                          join bk in _context.Bookings on bs.BookingId equals bk.Id
                          where bk.SpaId == id
                          select new
                          {
                              BookingServiceId = bs.Id,
                              Status = bs.Status,
                              Price = bs.Price,
                              ServiceName = bs.Service.Name,
                              SpaName = bk.Spa.Name,
                              TimeStart = bs.TimeStart,
                              TimeEnd = bs.TimeEnd,
                              UrlImage = bk.Spa.UrlImage

                          }).ToList();
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Contains("true"))
                {
                    result = (from bs in _context.BookingServices
                              join bk in _context.Bookings on bs.BookingId equals bk.Id
                              where bk.SpaId == id && bs.Status == true
                              select new
                              {
                                  BookingServiceId = bs.Id,
                                  Status = bs.Status,
                                  Price = bs.Price,
                                  ServiceName = bs.Service.Name,
                                  SpaName = bk.Spa.Name,
                                  TimeStart = bs.TimeStart,
                                  TimeEnd = bs.TimeEnd,
                                  UrlImage = bk.Spa.UrlImage
                              }).ToList();
                }
                if (status.Contains("false"))
                {
                    result = (from bs in _context.BookingServices
                              join bk in _context.Bookings on bs.BookingId equals bk.Id
                              where bk.SpaId == id && bs.Status == false
                              select new
                              {
                                  BookingServiceId = bs.Id,
                                  Status = bs.Status,
                                  Price = bs.Price,
                                  ServiceName = bs.Service.Name,
                                  SpaName = bk.Spa.Name,
                                  TimeStart = bs.TimeStart,
                                  TimeEnd = bs.TimeEnd,
                                  UrlImage = bk.Spa.UrlImage
                              }).ToList();
                }
            }


            if (result == null)
            {
                return NotFound();
            }

            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        }



        [HttpPost]
        public async Task<IActionResult> PostBookingService(int staffid,double price, string timestart, string timeend, int quantity, int serviceid, int spaid, int cusid)
        {
            try
            {
                Booking bk = new Booking();
                bk.Status = false;
                bk.CreateDay = DateTime.Now;
                bk.SpaId = spaid;
                bk.CustomerId = cusid;
                await _context.Bookings.AddAsync(bk);
                _context.SaveChanges();

                var bkexist = GetidBooking(spaid, cusid);

                    if (staffid>0 || bkexist.Id>0 || serviceid >0) { 

                BookingService bs = new BookingService();
                    bs.StaffId = staffid;
                    bs.Price = price;
                    bs.Status = false;
                    bs.Quantity = quantity;
                    bs.BookingId = bkexist.Id;
                    bs.ServiceId = serviceid;
                    bs.TimeStart = timestart;
                    bs.TimeEnd = timeend;

                await _context.BookingServices.AddAsync(bs);
                await _context.SaveChangesAsync();

                }
                return Ok(new { StatusCode = 200, Content = "The booking was add successfully completed" });


            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, message = "Booking failed (" + e.Message + ")" });

            }



        }

        // DELETE: api/bookingServices/5
        [HttpPut("acceptbookingservice")]
        public async Task<IActionResult> acceptBookingService(int id)
        {
            var bookingService = await _context.BookingServices.FindAsync(id);
            if (bookingService == null)
            {
                return NotFound();
            }
            if (bookingService.Status == false)
            {
                bookingService.Status = true;
            }
            else if ( bookingService.Status == true)
            {
                bookingService.Status = false;
            }
            _context.BookingServices.Update(bookingService);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Content = "The BookingService was accepted successfully completed" });

        }

        private bool BookingServiceExists(int id)
        {
            return _context.BookingServices.Any(e => e.Id == id);
        }

        private Booking GetidBooking(int spaid, int cusid)
        {
            var bk =  _context.Bookings.Where(a => a.SpaId == spaid && a.CustomerId == cusid).FirstOrDefault();

            if (bk == null)
            {
                return null;
            }
            return bk;
        }
    }
}
