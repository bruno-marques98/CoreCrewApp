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
    public class BenefitController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BenefitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Benefit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Benefit>>> GetBenefits()
        {
            return await _context.Benefits.ToListAsync();
        }

        // GET: api/Benefit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Benefit>> GetBenefit(int id)
        {
            var benefit = await _context.Benefits.FindAsync(id);

            if (benefit == null)
            {
                return NotFound();
            }

            return benefit;
        }

        // PUT: api/Benefit/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBenefit(int id, Benefit benefit)
        {
            if (id != benefit.BenefitID)
            {
                return BadRequest();
            }

            _context.Entry(benefit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BenefitExists(id))
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

        // POST: api/Benefit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Benefit>> PostBenefit(Benefit benefit)
        {
            _context.Benefits.Add(benefit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBenefit", new { id = benefit.BenefitID }, benefit);
        }

        // DELETE: api/Benefit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBenefit(int id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null)
            {
                return NotFound();
            }

            _context.Benefits.Remove(benefit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BenefitExists(int id)
        {
            return _context.Benefits.Any(e => e.BenefitID == id);
        }
    }
}
