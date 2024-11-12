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
    public class BenefitApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BenefitApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Benefit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Benefit>>> GetBenefits()
        {
            var benefits = await _context.Benefits.ToListAsync();
            return Ok(benefits);
        }

        // GET: api/Benefit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Benefit>> GetBenefit(int id)
        {
            var benefit = await _context.Benefits.FirstOrDefaultAsync(m => m.BenefitID == id);

            if (benefit == null)
            {
                return NotFound();
            }

            return Ok(benefit);
        }

        // POST: api/Benefit
        [HttpPost]
        public async Task<ActionResult<Benefit>> CreateBenefit([FromBody] Benefit benefit)
        {
            if (benefit == null)
            {
                return BadRequest();
            }

            _context.Benefits.Add(benefit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBenefit), new { id = benefit.BenefitID }, benefit);
        }

        // PUT: api/Benefit/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBenefit(int id, [FromBody] Benefit benefit)
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
