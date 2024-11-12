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
    public class EmployeeBenefitApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeBenefitApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeBenefit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeBenefit>>> GetEmployeeBenefits()
        {
            var employeeBenefits = await _context.EmployeeBenefits
                .Include(eb => eb.Employee)
                .Include(eb => eb.Benefit)
                .ToListAsync();
            return Ok(employeeBenefits);
        }

        // GET: api/EmployeeBenefit/5/5
        [HttpGet("{employeeId}/{benefitId}")]
        public async Task<ActionResult<EmployeeBenefit>> GetEmployeeBenefit(int employeeId, int benefitId)
        {
            var employeeBenefit = await _context.EmployeeBenefits
                .Include(eb => eb.Employee)
                .Include(eb => eb.Benefit)
                .FirstOrDefaultAsync(eb => eb.EmployeeID == employeeId && eb.BenefitID == benefitId);

            if (employeeBenefit == null)
            {
                return NotFound();
            }

            return Ok(employeeBenefit);
        }

        // POST: api/EmployeeBenefit
        [HttpPost]
        public async Task<ActionResult<EmployeeBenefit>> CreateEmployeeBenefit([FromBody] EmployeeBenefit employeeBenefit)
        {
            if (employeeBenefit == null)
            {
                return BadRequest();
            }

            _context.EmployeeBenefits.Add(employeeBenefit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeBenefit), new { employeeId = employeeBenefit.EmployeeID, benefitId = employeeBenefit.BenefitID }, employeeBenefit);
        }

        // PUT: api/EmployeeBenefit/5/5
        [HttpPut("{employeeId}/{benefitId}")]
        public async Task<IActionResult> UpdateEmployeeBenefit(int employeeId, int benefitId, [FromBody] EmployeeBenefit employeeBenefit)
        {
            if (employeeId != employeeBenefit.EmployeeID || benefitId != employeeBenefit.BenefitID)
            {
                return BadRequest();
            }

            _context.Entry(employeeBenefit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeBenefitExists(employeeId, benefitId))
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

        // DELETE: api/EmployeeBenefit/5/5
        [HttpDelete("{employeeId}/{benefitId}")]
        public async Task<IActionResult> DeleteEmployeeBenefit(int employeeId, int benefitId)
        {
            var employeeBenefit = await _context.EmployeeBenefits
                .FindAsync(employeeId, benefitId);
            if (employeeBenefit == null)
            {
                return NotFound();
            }

            _context.EmployeeBenefits.Remove(employeeBenefit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeBenefitExists(int employeeId, int benefitId)
        {
            return _context.EmployeeBenefits.Any(e => e.EmployeeID == employeeId && e.BenefitID == benefitId);
        }
    }
}
