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
    public class TrainingProgramApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainingProgramApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TrainingProgram
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingProgram>>> GetTrainingPrograms()
        {
            var trainingPrograms = await _context.TrainingPrograms.Include(tp => tp.Trainer).ToListAsync();
            return Ok(trainingPrograms);
        }

        // GET: api/TrainingProgram/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingProgram>> GetTrainingProgram(int id)
        {
            var trainingProgram = await _context.TrainingPrograms
                .Include(tp => tp.Trainer)
                .FirstOrDefaultAsync(m => m.TrainingProgramID == id);

            if (trainingProgram == null)
            {
                return NotFound();
            }

            return Ok(trainingProgram);
        }

        // POST: api/TrainingProgram
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<TrainingProgram>> PostTrainingProgram([FromBody] TrainingProgram trainingProgram)
        {
            if (ModelState.IsValid)
            {
                _context.TrainingPrograms.Add(trainingProgram);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetTrainingProgram), new { id = trainingProgram.TrainingProgramID }, trainingProgram);
            }

            return BadRequest(ModelState);
        }

        // PUT: api/TrainingProgram/5
        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PutTrainingProgram(int id, [FromBody] TrainingProgram trainingProgram)
        {
            if (id != trainingProgram.TrainingProgramID)
            {
                return BadRequest();
            }

            _context.Entry(trainingProgram).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingProgramExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // 204 No Content
        }

        // DELETE: api/TrainingProgram/5
        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainingProgram(int id)
        {
            var trainingProgram = await _context.TrainingPrograms.FindAsync(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            _context.TrainingPrograms.Remove(trainingProgram);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        private bool TrainingProgramExists(int id)
        {
            return _context.TrainingPrograms.Any(e => e.TrainingProgramID == id);
        }
    }
}
