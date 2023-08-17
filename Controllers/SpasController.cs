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
    public class spasController : ControllerBase
    {
        private readonly bookings3Context _context;
        private readonly IGetallService _userservice;
        public spasController(bookings3Context context, IGetallService userservice)
        {
            _context = context;
            _userservice = userservice;
        }

        // GET: api/Users
        [HttpGet("Getallbasic")]
        public IActionResult GetAllList(string search, string sortby, int page = 1, int PAGE_SIZE = 5)
        {
            try
            {
                var result = _userservice.GetAllSpa(search, sortby, page, PAGE_SIZE);
                var totalitems = result.Count();

                return Ok(new { StatusCode = 200, Message = "Load successful", data = result, totalitems });
            }
            catch (Exception e)
            {

                return StatusCode(409, new { StatusCode = 409, Message = e.Message });

            }
        }


        [HttpGet("Getalldetails")]
        public async Task<ActionResult> GetSpas(string search)
        {
            var result = (from s in _context.Spas
                          join us in _context.Users on s.UserId equals us.Id
                          select new
                          {
                              Id = s.Id,
                              UrlImage = s.UrlImage,
                              SpaName = s.Name,
                              FullName = us.FullName,
                              UserName = us.UserName,
                              Address = s.Address,
                              CreateDay = us.CreateDay,
                              Dob = us.Dob,
                              Email = us.Email,
                              Status = s.Status,
                              Phone = us.Phone,
                              Services = s.Services,
                              staff = s.staff



                          }).ToList();



            if (!string.IsNullOrEmpty(search))
            {
                 result = (from s in _context.Spas
                              join us in _context.Users on s.UserId equals us.Id
                              where s.Name.Contains(search)
                              select new
                              {
                                  Id = s.Id,
                                  UrlImage = s.UrlImage,
                                  SpaName = s.Name,
                                  FullName = us.FullName,
                                  UserName = us.UserName,
                                  Address = s.Address,
                                  CreateDay = us.CreateDay,
                                  Dob = us.Dob,
                                  Email = us.Email,
                                  Status = s.Status,
                                  Phone = us.Phone,
                                  Services = s.Services,
                                  staff = s.staff



                              }).ToList();
            }
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        }


        // GET: api/spas/5
        [HttpGet("getbyid")]
        public async Task<ActionResult> GetSpa(int id)
        {
            var result = (from s in _context.Spas
                          join us in _context.Users on s.UserId equals us.Id
                          where s.Id == id
                          select new
                          {
                              Id = s.Id,
                              UrlImage = s.UrlImage,
                              SpaName = s.Name,
                              FullName = us.FullName,
                              UserName = us.UserName,
                              Address = s.Address,
                              CreateDay = us.CreateDay,
                              Dob = us.Dob,
                              Email = us.Email,
                              Status = s.Status,
                              Phone = us.Phone,
                              Services = s.Services,
                              staff = s.staff


                          }).ToList();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(new { StatusCode = 200, Message = "Load successful", data = result });
        }

        // PUT: api/spas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutSpa(int id, Spa spa)
        {
            if (id != spa.Id)
            {
                return BadRequest();
            }

            _context.Entry(spa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpaExists(id))
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

        // POST: api/spas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Spa>> PostSpa(Spa spa)
        {
            _context.Spas.Add(spa);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SpaExists(spa.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSpa", new { id = spa.Id }, spa);
        }

        // DELETE: api/spas/5
        [HttpDelete]
        public async Task<IActionResult> DeleteSpa(int id)
        {
            var spa = await _context.Spas.FindAsync(id);
            if (spa == null)
            {
                return NotFound();
            }

            spa.Status = false;
            _context.Spas.Update(spa);
            await _context.SaveChangesAsync();

            return Ok(new { StatusCode = 200, Content = "The spa was inactive successfully completed" });
        }

        private bool SpaExists(int id)
        {
            return _context.Spas.Any(e => e.Id == id);
        }
    }
}
