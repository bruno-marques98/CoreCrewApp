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
    public class PerformanceReviewApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerformanceReviewApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/PerformanceReview
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerformanceReview>>> GetPerformanceReviews()
        {
            var reviews = await _context.PerformanceReviews
                .Include(pr => pr.Employee)
                .ToListAsync();
            return Ok(reviews);
        }

        // GET: api/PerformanceReview/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PerformanceReview>> GetPerformanceReview(int id)
        {
            var performanceReview = await _context.PerformanceReviews
                .Include(pr => pr.Employee)
                .FirstOrDefaultAsync(pr => pr.PerformanceReviewID == id);

            if (performanceReview == null)
            {
                return NotFound();
            }

            return Ok(performanceReview);
        }

        // POST: api/PerformanceReview
        [HttpPost]
        public async Task<ActionResult<PerformanceReview>> CreatePerformanceReview([FromBody] PerformanceReview performanceReview)
        {
            if (performanceReview == null)
            {
                return BadRequest();
            }

            _context.PerformanceReviews.Add(performanceReview);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerformanceReview), new { id = performanceReview.PerformanceReviewID }, performanceReview);
        }

        // PUT: api/PerformanceReview/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerformanceReview(int id, [FromBody] PerformanceReview performanceReview)
        {
            if (id != performanceReview.PerformanceReviewID)
            {
                return BadRequest();
            }

            _context.Entry(performanceReview).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PerformanceReviewExists(id))
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

        // DELETE: api/PerformanceReview/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerformanceReview(int id)
        {
            var performanceReview = await _context.PerformanceReviews.FindAsync(id);
            if (performanceReview == null)
            {
                return NotFound();
            }

            _context.PerformanceReviews.Remove(performanceReview);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PerformanceReviewExists(int id)
        {
            return _context.PerformanceReviews.Any(e => e.PerformanceReviewID == id);
        }
    }
}
