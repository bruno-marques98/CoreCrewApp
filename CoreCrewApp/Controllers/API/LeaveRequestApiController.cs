using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeaveRequestApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LeaveRequest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveRequest>>> GetLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .ToListAsync();
            return Ok(leaveRequests);
        }

        // GET: api/LeaveRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveRequest>> GetLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .FirstOrDefaultAsync(lr => lr.LeaveRequestID == id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            return Ok(leaveRequest);
        }

        // POST: api/LeaveRequest
        [HttpPost]
        public async Task<ActionResult<LeaveRequest>> CreateLeaveRequest([FromBody] LeaveRequest leaveRequest)
        {
            if (leaveRequest == null)
            {
                return BadRequest();
            }

            leaveRequest.Status = LeaveStatus.Pending; // Set status to Pending initially
            _context.LeaveRequests.Add(leaveRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLeaveRequest), new { id = leaveRequest.LeaveRequestID }, leaveRequest);
        }

        // PUT: api/LeaveRequest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLeaveRequest(int id, [FromBody] LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.LeaveRequestID)
            {
                return BadRequest();
            }

            _context.Entry(leaveRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveRequestExists(id))
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

        // DELETE: api/LeaveRequest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(lr => lr.LeaveRequestID == id);
        }
    }
}
