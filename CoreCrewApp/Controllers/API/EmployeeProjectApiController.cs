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
    public class EmployeeProjectApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeProjectApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeProject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeProject>>> GetEmployeeProjects()
        {
            var employeeProjects = await _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .ToListAsync();
            return Ok(employeeProjects);
        }

        // GET: api/EmployeeProject/5/5
        [HttpGet("{employeeId}/{projectId}")]
        public async Task<ActionResult<EmployeeProject>> GetEmployeeProject(int employeeId, int projectId)
        {
            var employeeProject = await _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .FirstOrDefaultAsync(ep => ep.EmployeeID == employeeId && ep.ProjectID == projectId);

            if (employeeProject == null)
            {
                return NotFound();
            }

            return Ok(employeeProject);
        }

        // POST: api/EmployeeProject
        [HttpPost]
        public async Task<ActionResult<EmployeeProject>> CreateEmployeeProject([FromBody] EmployeeProject employeeProject)
        {
            if (employeeProject == null)
            {
                return BadRequest();
            }

            _context.EmployeeProjects.Add(employeeProject);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeProject), new { employeeId = employeeProject.EmployeeID, projectId = employeeProject.ProjectID }, employeeProject);
        }

        // PUT: api/EmployeeProject/5/5
        [HttpPut("{employeeId}/{projectId}")]
        public async Task<IActionResult> UpdateEmployeeProject(int employeeId, int projectId, [FromBody] EmployeeProject employeeProject)
        {
            if (employeeId != employeeProject.EmployeeID || projectId != employeeProject.ProjectID)
            {
                return BadRequest();
            }

            _context.Entry(employeeProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeProjectExists(employeeId, projectId))
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

        // DELETE: api/EmployeeProject/5/5
        [HttpDelete("{employeeId}/{projectId}")]
        public async Task<IActionResult> DeleteEmployeeProject(int employeeId, int projectId)
        {
            var employeeProject = await _context.EmployeeProjects.FindAsync(employeeId, projectId);
            if (employeeProject == null)
            {
                return NotFound();
            }

            _context.EmployeeProjects.Remove(employeeProject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeProjectExists(int employeeId, int projectId)
        {
            return _context.EmployeeProjects.Any(ep => ep.EmployeeID == employeeId && ep.ProjectID == projectId);
        }
    }
}
