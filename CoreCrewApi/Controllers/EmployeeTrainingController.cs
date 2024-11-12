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
    public class EmployeeTrainingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeTrainingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeTraining
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTraining>>> GetEmployeeTrainings()
        {
            return await _context.EmployeeTrainings.ToListAsync();
        }

        // GET: api/EmployeeTraining/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTraining>> GetEmployeeTraining(int id)
        {
            var employeeTraining = await _context.EmployeeTrainings.FindAsync(id);

            if (employeeTraining == null)
            {
                return NotFound();
            }

            return employeeTraining;
        }

        // PUT: api/EmployeeTraining/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeTraining(int id, EmployeeTraining employeeTraining)
        {
            if (id != employeeTraining.EmployeeID)
            {
                return BadRequest();
            }

            _context.Entry(employeeTraining).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeTrainingExists(id))
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

        // POST: api/EmployeeTraining
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeTraining>> PostEmployeeTraining(EmployeeTraining employeeTraining)
        {
            _context.EmployeeTrainings.Add(employeeTraining);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeTrainingExists(employeeTraining.EmployeeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeTraining", new { id = employeeTraining.EmployeeID }, employeeTraining);
        }

        // DELETE: api/EmployeeTraining/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeTraining(int id)
        {
            var employeeTraining = await _context.EmployeeTrainings.FindAsync(id);
            if (employeeTraining == null)
            {
                return NotFound();
            }

            _context.EmployeeTrainings.Remove(employeeTraining);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeTrainingExists(int id)
        {
            return _context.EmployeeTrainings.Any(e => e.EmployeeID == id);
        }
    }
}
