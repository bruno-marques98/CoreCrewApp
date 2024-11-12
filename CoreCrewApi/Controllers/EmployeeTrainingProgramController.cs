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
    public class EmployeeTrainingProgramController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeTrainingProgramController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeTrainingProgram
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTrainingProgram>>> GetEmployeeTrainingProgram()
        {
            return await _context.EmployeeTrainingProgram.ToListAsync();
        }

        // GET: api/EmployeeTrainingProgram/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTrainingProgram>> GetEmployeeTrainingProgram(int id)
        {
            var employeeTrainingProgram = await _context.EmployeeTrainingProgram.FindAsync(id);

            if (employeeTrainingProgram == null)
            {
                return NotFound();
            }

            return employeeTrainingProgram;
        }

        // PUT: api/EmployeeTrainingProgram/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeTrainingProgram(int id, EmployeeTrainingProgram employeeTrainingProgram)
        {
            if (id != employeeTrainingProgram.EmployeeTrainingProgramID)
            {
                return BadRequest();
            }

            _context.Entry(employeeTrainingProgram).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeTrainingProgramExists(id))
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

        // POST: api/EmployeeTrainingProgram
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeTrainingProgram>> PostEmployeeTrainingProgram(EmployeeTrainingProgram employeeTrainingProgram)
        {
            _context.EmployeeTrainingProgram.Add(employeeTrainingProgram);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeTrainingProgram", new { id = employeeTrainingProgram.EmployeeTrainingProgramID }, employeeTrainingProgram);
        }

        // DELETE: api/EmployeeTrainingProgram/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeTrainingProgram(int id)
        {
            var employeeTrainingProgram = await _context.EmployeeTrainingProgram.FindAsync(id);
            if (employeeTrainingProgram == null)
            {
                return NotFound();
            }

            _context.EmployeeTrainingProgram.Remove(employeeTrainingProgram);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeTrainingProgramExists(int id)
        {
            return _context.EmployeeTrainingProgram.Any(e => e.EmployeeTrainingProgramID == id);
        }
    }
}
