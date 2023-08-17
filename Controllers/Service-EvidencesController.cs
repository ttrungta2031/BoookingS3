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
    public class service_evidencesController : ControllerBase
    {
        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public service_evidencesController(bookings3Context context, IGetallService userservice)
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
                var result = _userservice.GetAllServiceEvidence(search, sortby, page, PAGE_SIZE);
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result, totalitems });
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }
    

        // GET: api/serviceevidences/5
        [HttpGet("getbyid")]
        public async Task<ActionResult<ServiceEvidence>> GetServiceEvidence(int id)
        {
            var serviceEvidence = await _context.ServiceEvidences.FindAsync(id);

            if (serviceEvidence == null)
            {
                return NotFound();
            }

            return serviceEvidence;
        }

        // PUT: api/serviceevidences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutServiceEvidence(int id, ServiceEvidence serviceEvidence)
        {
            if (id != serviceEvidence.Id)
            {
                return BadRequest();
            }

            _context.Entry(serviceEvidence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceEvidenceExists(id))
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

        // POST: api/serviceevidences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceEvidence>> PostServiceEvidence(ServiceEvidence serviceEvidence)
        {
            _context.ServiceEvidences.Add(serviceEvidence);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServiceEvidenceExists(serviceEvidence.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServiceEvidence", new { id = serviceEvidence.Id }, serviceEvidence);
        }

        // DELETE: api/serviceevidences/5
        [HttpDelete]
        public async Task<IActionResult> DeleteServiceEvidence(int id)
        {
            var serviceEvidence = await _context.ServiceEvidences.FindAsync(id);
            if (serviceEvidence == null)
            {
                return NotFound();
            }

            _context.ServiceEvidences.Remove(serviceEvidence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceEvidenceExists(int id)
        {
            return _context.ServiceEvidences.Any(e => e.Id == id);
        }
    }
}
