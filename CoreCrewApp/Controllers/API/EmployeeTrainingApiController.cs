using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers.API
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTrainingApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeTrainingApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeTraining
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTraining>>> GetEmployeeTrainings()
        {
            var employeeTrainings = await _context.EmployeeTrainings
                .Include(et => et.Employee)
                .Include(et => et.TrainingProgram)
                .ToListAsync();
            return Ok(employeeTrainings);
        }

        // GET: api/EmployeeTraining/5/5
        [HttpGet("{employeeId}/{trainingProgramId}")]
        public async Task<ActionResult<EmployeeTraining>> GetEmployeeTraining(int employeeId, int trainingProgramId)
        {
            var employeeTraining = await _context.EmployeeTrainings
                .Include(et => et.Employee)
                .Include(et => et.TrainingProgram)
                .FirstOrDefaultAsync(et => et.EmployeeID == employeeId && et.TrainingProgramID == trainingProgramId);

            if (employeeTraining == null)
            {
                return NotFound();
            }

            return Ok(employeeTraining);
        }

        // POST: api/EmployeeTraining
        [HttpPost]
        public async Task<ActionResult<EmployeeTraining>> CreateEmployeeTraining([FromBody] EmployeeTraining employeeTraining)
        {
            if (employeeTraining == null)
            {
                return BadRequest();
            }

            _context.EmployeeTrainings.Add(employeeTraining);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployeeTraining), new { employeeId = employeeTraining.EmployeeID, trainingProgramId = employeeTraining.TrainingProgramID }, employeeTraining);
        }

        // PUT: api/EmployeeTraining/5/5
        [HttpPut("{employeeId}/{trainingProgramId}")]
        public async Task<IActionResult> UpdateEmployeeTraining(int employeeId, int trainingProgramId, [FromBody] EmployeeTraining employeeTraining)
        {
            if (employeeId != employeeTraining.EmployeeID || trainingProgramId != employeeTraining.TrainingProgramID)
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
                if (!EmployeeTrainingExists(employeeId, trainingProgramId))
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

        // DELETE: api/EmployeeTraining/5/5
        [HttpDelete("{employeeId}/{trainingProgramId}")]
        public async Task<IActionResult> DeleteEmployeeTraining(int employeeId, int trainingProgramId)
        {
            var employeeTraining = await _context.EmployeeTrainings.FindAsync(employeeId, trainingProgramId);
            if (employeeTraining == null)
            {
                return NotFound();
            }

            _context.EmployeeTrainings.Remove(employeeTraining);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeTrainingExists(int employeeId, int trainingProgramId)
        {
            return _context.EmployeeTrainings.Any(et => et.EmployeeID == employeeId && et.TrainingProgramID == trainingProgramId);
        }
    }
}
