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
    public class SalaryApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalaryApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Salary
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salary>>> GetSalaries()
        {
            var salaries = await _context.Salaries
                .Include(s => s.Employee)  // Include Employee data
                .ToListAsync();
            return Ok(salaries);
        }

        // GET: api/Salary/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Salary>> GetSalary(int id)
        {
            var salary = await _context.Salaries
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.SalaryID == id);

            if (salary == null)
            {
                return NotFound();
            }

            return Ok(salary);
        }

        // POST: api/Salary
        [HttpPost]
        public async Task<ActionResult<Salary>> CreateSalary([FromBody] Salary salary)
        {
            if (salary == null)
            {
                return BadRequest();
            }

            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalary), new { id = salary.SalaryID }, salary);
        }

        // PUT: api/Salary/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSalary(int id, [FromBody] Salary salary)
        {
            if (id != salary.SalaryID)
            {
                return BadRequest();
            }

            _context.Entry(salary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryExists(id))
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

        // DELETE: api/Salary/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.SalaryID == id);
        }
    }
}
