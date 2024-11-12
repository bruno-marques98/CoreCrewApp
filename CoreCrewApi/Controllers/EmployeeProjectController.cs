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
    public class EmployeeProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeProject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeProject>>> GetEmployeeProjects()
        {
            return await _context.EmployeeProjects.ToListAsync();
        }

        // GET: api/EmployeeProject/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeProject>> GetEmployeeProject(int id)
        {
            var employeeProject = await _context.EmployeeProjects.FindAsync(id);

            if (employeeProject == null)
            {
                return NotFound();
            }

            return employeeProject;
        }

        // PUT: api/EmployeeProject/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeProject(int id, EmployeeProject employeeProject)
        {
            if (id != employeeProject.EmployeeID)
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
                if (!EmployeeProjectExists(id))
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

        // POST: api/EmployeeProject
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeProject>> PostEmployeeProject(EmployeeProject employeeProject)
        {
            _context.EmployeeProjects.Add(employeeProject);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeProjectExists(employeeProject.EmployeeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeProject", new { id = employeeProject.EmployeeID }, employeeProject);
        }

        // DELETE: api/EmployeeProject/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeProject(int id)
        {
            var employeeProject = await _context.EmployeeProjects.FindAsync(id);
            if (employeeProject == null)
            {
                return NotFound();
            }

            _context.EmployeeProjects.Remove(employeeProject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeProjectExists(int id)
        {
            return _context.EmployeeProjects.Any(e => e.EmployeeID == id);
        }
    }
}
