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
    public class staffsController : ControllerBase
    {
        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public staffsController(bookings3Context context, IGetallService userservice)
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
                var result = _userservice.GetAllstaff(search, sortby, page, PAGE_SIZE);
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result, totalitems });
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }

        // GET: api/staffs/5
        [HttpGet("getbyid")]
        public async Task<ActionResult<staff>> Getstaff(int id)
        {
            var staff = await _context.staff.FindAsync(id);

            if (staff == null)
            {
                return NotFound();
            }

            return staff;
        }

        // PUT: api/staffs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> Putstaff(int id, staff staff)
        {
            if (id != staff.Id)
            {
                return BadRequest();
            }

            _context.Entry(staff).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!staffExists(id))
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

        // POST: api/staffs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<staff>> Poststaff(staff staff)
        {
            _context.staff.Add(staff);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (staffExists(staff.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("Getstaff", new { id = staff.Id }, staff);
        }

        // DELETE: api/staffs/5
        [HttpDelete]
        public async Task<IActionResult> Deletestaff(int id)
        {
            var staff = await _context.staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            staff.Status = false;
            _context.staff.Update(staff);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Content = "The staff was remove successfully completed" });
        }

        private bool staffExists(int id)
        {
            return _context.staff.Any(e => e.Id == id);
        }
    }
}
