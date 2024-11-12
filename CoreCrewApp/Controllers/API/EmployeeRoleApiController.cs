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
    public class EmployeeRoleApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeRoleApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeRole>>> GetEmployeeRoles()
        {
            var employeeRoles = await _context.EmployeeRoles
                .Include(er => er.Employee)
                .Include(er => er.Role)
                .ToListAsync();
            return Ok(employeeRoles);
        }

        // GET: api/EmployeeRole/5/5
        [HttpGet("{employeeId}/{roleId}")]
        public async Task<ActionResult<EmployeeRole>> GetEmployeeRole(int employeeId, int roleId)
        {
            var employeeRole = await _context.EmployeeRoles
                .Include(er => er.Employee)
                .Include(er => er.Role)
                .FirstOrDefaultAsync(er => er.EmployeeID == employeeId && er.RoleID == roleId);

            if (employeeRole == null)
            {
                return NotFound();
            }

            return Ok(employeeRole);
        }

        // POST: api/EmployeeRole
        [HttpPost]
        public async Task<ActionResult<EmployeeRole>> CreateEmployeeRole([FromBody] EmployeeRole employeeRole)
        {
            if (employeeRole == null)
            {
                return BadRequest();
            }

            _context.EmployeeRoles.Add(employeeRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeRole), new { employeeId = employeeRole.EmployeeID, roleId = employeeRole.RoleID }, employeeRole);
        }

        // PUT: api/EmployeeRole/5/5
        [HttpPut("{employeeId}/{roleId}")]
        public async Task<IActionResult> UpdateEmployeeRole(int employeeId, int roleId, [FromBody] EmployeeRole employeeRole)
        {
            if (employeeId != employeeRole.EmployeeID || roleId != employeeRole.RoleID)
            {
                return BadRequest();
            }

            _context.Entry(employeeRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeRoleExists(employeeId, roleId))
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

        // DELETE: api/EmployeeRole/5/5
        [HttpDelete("{employeeId}/{roleId}")]
        public async Task<IActionResult> DeleteEmployeeRole(int employeeId, int roleId)
        {
            var employeeRole = await _context.EmployeeRoles.FindAsync(employeeId, roleId);
            if (employeeRole == null)
            {
                return NotFound();
            }

            _context.EmployeeRoles.Remove(employeeRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeRoleExists(int employeeId, int roleId)
        {
            return _context.EmployeeRoles.Any(er => er.EmployeeID == employeeId && er.RoleID == roleId);
        }
    }
}
