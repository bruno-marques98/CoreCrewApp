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
    public class AuditLogApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditLogApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/AuditLog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs()
        {
            var auditLogs = await _context.AuditLogs.ToListAsync();
            return Ok(auditLogs); // Return a list of audit logs as JSON
        }

        // GET: api/AuditLog/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuditLog>> GetAuditLog(int id)
        {
            var auditLog = await _context.AuditLogs
                .FirstOrDefaultAsync(m => m.AuditLogID == id);

            if (auditLog == null)
            {
                return NotFound(); // Return 404 if the log is not found
            }

            return Ok(auditLog); // Return the specific audit log as JSON
        }

        // POST: api/AuditLog
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AuditLog>> CreateAuditLog([FromBody] AuditLog auditLog)
        {
            if (auditLog == null)
            {
                return BadRequest(); // Return 400 if input is null or invalid
            }

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuditLog), new { id = auditLog.AuditLogID }, auditLog); // Return 201 with the newly created log
        }

        // PUT: api/AuditLog/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAuditLog(int id, [FromBody] AuditLog auditLog)
        {
            if (id != auditLog.AuditLogID)
            {
                return BadRequest(); // Return 400 if IDs don't match
            }

            try
            {
                _context.Entry(auditLog).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditLogExists(id))
                {
                    return NotFound(); // Return 404 if the log does not exist
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Return 204 No Content on success
        }

        // DELETE: api/AuditLog/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAuditLog(int id)
        {
            var auditLog = await _context.AuditLogs.FindAsync(id);
            if (auditLog == null)
            {
                return NotFound(); // Return 404 if the log does not exist
            }

            _context.AuditLogs.Remove(auditLog);
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content on success
        }

        private bool AuditLogExists(int id)
        {
            return _context.AuditLogs.Any(e => e.AuditLogID == id);
        }
    }
}
