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

namespace BoookingS3.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class bookingsController : ControllerBase
    {
        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public bookingsController(bookings3Context context, IGetallService userservice)
        {
            _context = context;
            _userservice = userservice;

        }

        // GET: api/Users
        [HttpGet]
        
        public IActionResult GetAllList(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            try
            {

                var result = _userservice.GetAllBooking(search, sortby, page, PAGE_SIZE);
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result, totalitems });
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }



       /* [HttpGet("Getalldetails")]
        public async Task<ActionResult> GetBookings()
        {

            var result = (from b in _context.Bookings
                          join cu in _context.Customers on b.CustomerId equals cu.Id
                          join sp in _context.Spas on b.SpaId equals sp.Id
                          select new
                          {




                          }).ToList();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        }
*/
        // PUT: api/bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBooking(int id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        /*[HttpGet("test")]
        public async Task<ActionResult<User>> Bookingbyacc()
        {
            try
            {
                var userLogin = await _userManager.GetUserAsync(HttpContext.User);
                //var evid = eventt.Id;
                var result = await (from User in _context.Users
                                    where userLogin.Id == User.Id

                                    select new
                                    {
                                        User.Id,
                                        User.FullName,
                                        User.Email
                                    }).ToListAsync();

                if (!result.Any())
                {
                    return BadRequest(new { StatusCodes = 404, Message = "The requested resource was not found" });
                }
                else
                {
                    return Ok(new { StatusCode = 200, message = "The request was successfully completed", data = result });
                }
            }
            catch (Exception e)
            {
                return StatusCode(409, new { StatusCode = 200, message = e.Message });
            }

        }*/


        [HttpPost("bookingspa")]
        public async Task<IActionResult> PostBooking(int spaid, int cusid )
        {
            try {



                Booking bk = new Booking();
                bk.Status = true;
                bk.CreateDay = DateTime.Now;
                bk.SpaId = spaid;
                bk.CustomerId = cusid;
                await _context.Bookings.AddAsync(bk);
                await _context.SaveChangesAsync();
          
                
                    return Ok(new { StatusCode = 200, Content = "The booking was create successfully completed" });
                

               
            }
            catch(Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, message = "Book failed (" + e.Message + ")" });

            }


          
        }

        [HttpPut("feedbackspa")]
        public async Task<IActionResult> feedbackSpa(int id, string feedback, string score)
        {
            try
            {
                var booking = await _context.Bookings.FindAsync(id);
                if (booking == null)
                {
                    return BadRequest(" The BookingID is not exited!!");
                }
                
                Booking bk = new Booking();

                bk.Id = id;
                bk.Feedback = feedback;
                bk.Score = score;
                 _context.Bookings.Update(bk);
                await _context.SaveChangesAsync();


                return Ok(new { StatusCode = 200, Content = "The booking was update successfully completed" });



            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, message = "Book failed (" + e.Message + ")" });

            }



        }


        // DELETE: api/bookings/5
        [HttpDelete]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = false;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Content = "The booking was remove successfully completed" }) ;
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }
    }
}
