using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCrewApi.Data;
using CoreCrewApi.Models;

namespace CoreCrewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeRole>>> GetEmployeeRoles()
        {
            return await _context.EmployeeRoles.ToListAsync();
        }

        // GET: api/EmployeeRole/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeRole>> GetEmployeeRole(int id)
        {
            var employeeRole = await _context.EmployeeRoles.FindAsync(id);

            if (employeeRole == null)
            {
                return NotFound();
            }

            return employeeRole;
        }

        // PUT: api/EmployeeRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeRole(int id, EmployeeRole employeeRole)
        {
            if (id != employeeRole.EmployeeID)
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
                if (!EmployeeRoleExists(id))
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

        // POST: api/EmployeeRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeRole>> PostEmployeeRole(EmployeeRole employeeRole)
        {
            _context.EmployeeRoles.Add(employeeRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeRoleExists(employeeRole.EmployeeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeRole", new { id = employeeRole.EmployeeID }, employeeRole);
        }

        // DELETE: api/EmployeeRole/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeRole(int id)
        {
            var employeeRole = await _context.EmployeeRoles.FindAsync(id);
            if (employeeRole == null)
            {
                return NotFound();
            }

            _context.EmployeeRoles.Remove(employeeRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeRoleExists(int id)
        {
            return _context.EmployeeRoles.Any(e => e.EmployeeID == id);
        }
    }
}
