using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BoookingS3.Models;
using BoookingS3.Common;
using Microsoft.AspNetCore.Authorization;
using BoookingS3.Services;

namespace BoookingS3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class usersController : ControllerBase
    {
        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public usersController(bookings3Context context, IGetallService userservice)
        {
            _context = context;
            _userservice = userservice;
        }




        [HttpGet("Getalldetails")]
        public async Task<ActionResult> Getalldetail()
        {
            var result = (from us in _context.Users
                          select new
                          {
                              Id = us.Id,
                              FullName = us.FullName,
                              UserName = us.UserName,
                              Address = us.Address,
                              CreateDay = us.CreateDay,
                              Dob = us.Dob,
                              Email = us.Email,
                              Status = us.Status,
                              Phone = us.Phone,
                              Password = us.Password,
                              Customer = us.Customers,
                              Role = us.Role

                          }).ToList();



          
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        }





        // GET: api/Users
        [HttpGet]
        public  IActionResult GetAllList(string search, string role, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            try
            {
                var result =  _userservice.GetAllUser(search,role,sortby, page, PAGE_SIZE );
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result, totalitems });
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }

        // GET: api/Users/5
        [HttpGet("getbyid")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }



        [HttpGet("(Codebyemail)")]
        public async Task<ActionResult> Codebyemail(string email)
        {
            var result = (from us in _context.Users
                          where us.Email == email
                          select new
                          {
                              us.Code
                          }).ToList();

            return Ok(result);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser( User user)
        {
            try
            {
                var useraccount = new User();
                {
                    useraccount.UserName = user.UserName;
                    useraccount.Password = user.Password;
                    useraccount.Address = user.Address;
                    useraccount.CreateDay = DateTime.Now;
                    useraccount.Email = user.Email;
                    useraccount.FullName = user.FullName;
                    useraccount.Phone = user.Phone;
                    useraccount.RoleId = user.RoleId;
                    useraccount.Status = user.Status;
                    useraccount.Dob = DateTime.Today;
                }
                _context.Users.Add(useraccount);
                await _context.SaveChangesAsync();


                return Ok(new { StatusCode = 201, Message = "Add Successfull" });
            }catch(Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                return StatusCode(409, new { StatusCode = 409, Message = e.Message });
            }
        }

        // DELETE: api/Users/5
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Status == true)
            {
                user.Status = false;
            }
            else user.Status = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Content = "The user was active/inactive successfully completed" });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        

    }
}
