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
    public class EmployeeBenefitController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeBenefitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeBenefit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeBenefit>>> GetEmployeeBenefits()
        {
            return await _context.EmployeeBenefits.ToListAsync();
        }

        // GET: api/EmployeeBenefit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeBenefit>> GetEmployeeBenefit(int id)
        {
            var employeeBenefit = await _context.EmployeeBenefits.FindAsync(id);

            if (employeeBenefit == null)
            {
                return NotFound();
            }

            return employeeBenefit;
        }

        // PUT: api/EmployeeBenefit/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeBenefit(int id, EmployeeBenefit employeeBenefit)
        {
            if (id != employeeBenefit.EmployeeID)
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
                if (!EmployeeBenefitExists(id))
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

        // POST: api/EmployeeBenefit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeBenefit>> PostEmployeeBenefit(EmployeeBenefit employeeBenefit)
        {
            _context.EmployeeBenefits.Add(employeeBenefit);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeBenefitExists(employeeBenefit.EmployeeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeBenefit", new { id = employeeBenefit.EmployeeID }, employeeBenefit);
        }

        // DELETE: api/EmployeeBenefit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeBenefit(int id)
        {
            var employeeBenefit = await _context.EmployeeBenefits.FindAsync(id);
            if (employeeBenefit == null)
            {
                return NotFound();
            }

            _context.EmployeeBenefits.Remove(employeeBenefit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeBenefitExists(int id)
        {
            return _context.EmployeeBenefits.Any(e => e.EmployeeID == id);
        }
    }
}
