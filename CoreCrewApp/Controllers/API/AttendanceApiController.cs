using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers.API
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AttendanceApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Attendance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
            var attendances = await _context.Attendances.Include(a => a.Employee).ToListAsync();
            return Ok(attendances); // Return a list of attendances as JSON
        }

        // GET: api/Attendance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.AttendanceId == id);

            if (attendance == null)
            {
                return NotFound(); // Return 404 if not found
            }

            return Ok(attendance); // Return the specific attendance record as JSON
        }

        // POST: api/Attendance
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Attendance>> CreateAttendance([FromBody] Attendance attendance)
        {
            if (attendance == null)
            {
                return BadRequest(); // Return 400 if the input is null
            }

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttendance), new { id = attendance.AttendanceId }, attendance);
        }

        // PUT: api/Attendance/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] Attendance attendance)
        {
            if (id != attendance.AttendanceId)
            {
                return BadRequest(); // Return 400 if IDs don't match
            }

            try
            {
                _context.Entry(attendance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(id))
                {
                    return NotFound(); // Return 404 if attendance not found
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Return 204 No Content on success
        }

        // DELETE: api/Attendance/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound(); // Return 404 if not found
            }

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content on success
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(a => a.AttendanceId == id);
        }
    }
}
