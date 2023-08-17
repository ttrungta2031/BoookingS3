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
    public class staff_servicesController : ControllerBase
    {
        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public staff_servicesController(bookings3Context context, IGetallService userservice)
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
                var result = _userservice.GetAllStaffService(search, sortby, page, PAGE_SIZE);
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result, totalitems });
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }

        // GET: api/staffservices/5
        [HttpGet("getbyid")]
        public async Task<ActionResult<StaffService>> GetStaffService(int id)
        {
            var staffService = await _context.StaffServices.FindAsync(id);

            if (staffService == null)
            {
                return NotFound();
            }

            return staffService;
        }

        // PUT: api/staffservices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutStaffService(int id, StaffService staffService)
        {
            if (id != staffService.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffServiceExists(id))
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

        // POST: api/staffservices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StaffService>> PostStaffService(StaffService staffService)
        {
            _context.StaffServices.Add(staffService);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StaffServiceExists(staffService.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStaffService", new { id = staffService.Id }, staffService);
        }

        // DELETE: api/staffservices/5
        [HttpDelete]
        public async Task<IActionResult> DeleteStaffService(int id)
        {
            var staffService = await _context.StaffServices.FindAsync(id);
            if (staffService == null)
            {
                return NotFound();
            }

            staffService.Status = false;
            _context.StaffServices.Update(staffService);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Content = "The staffService was remove successfully completed" });
        }

        private bool StaffServiceExists(int id)
        {
            return _context.StaffServices.Any(e => e.Id == id);
        }
    }
}
