using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    public class EmployeeTrainingController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeTrainingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeTraining
        public async Task<IActionResult> Index()
        {
            var employeeTrainings = _context.EmployeeTrainings
                .Include(et => et.Employee)
                .Include(et => et.TrainingProgram);
            return View(await employeeTrainings.ToListAsync());
        }

        // GET: EmployeeTraining/Details/5/5
        public async Task<IActionResult> Details(int? employeeId, int? trainingProgramId)
        {
            if (employeeId == null || trainingProgramId == null)
            {
                return NotFound();
            }

            var employeeTraining = await _context.EmployeeTrainings
                .Include(et => et.Employee)
                .Include(et => et.TrainingProgram)
                .FirstOrDefaultAsync(et => et.EmployeeID == employeeId && et.TrainingProgramID == trainingProgramId);

            if (employeeTraining == null)
            {
                return NotFound();
            }

            return View(employeeTraining);
        }

        // GET: EmployeeTraining/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName");
            ViewData["TrainingProgramID"] = new SelectList(_context.TrainingPrograms, "TrainingProgramID", "ProgramName");
            return View();
        }

        // POST: EmployeeTraining/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,TrainingProgramID,EnrollmentDate,CompletionDate")] EmployeeTraining employeeTraining)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeTraining);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeTraining.EmployeeID);
            ViewData["TrainingProgramID"] = new SelectList(_context.TrainingPrograms, "TrainingProgramID", "ProgramName", employeeTraining.TrainingProgramID);
            return View(employeeTraining);
        }

        // GET: EmployeeTraining/Edit/5/5
        public async Task<IActionResult> Edit(int? employeeId, int? trainingProgramId)
        {
            if (employeeId == null || trainingProgramId == null)
            {
                return NotFound();
            }

            var employeeTraining = await _context.EmployeeTrainings
                .FindAsync(employeeId, trainingProgramId);

            if (employeeTraining == null)
            {
                return NotFound();
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeTraining.EmployeeID);
            ViewData["TrainingProgramID"] = new SelectList(_context.TrainingPrograms, "TrainingProgramID", "ProgramName", employeeTraining.TrainingProgramID);
            return View(employeeTraining);
        }

        // POST: EmployeeTraining/Edit/5/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int employeeId, int trainingProgramId, [Bind("EmployeeID,TrainingProgramID,EnrollmentDate,CompletionDate")] EmployeeTraining employeeTraining)
        {
            if (employeeId != employeeTraining.EmployeeID || trainingProgramId != employeeTraining.TrainingProgramID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeTraining);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeTrainingExists(employeeTraining.EmployeeID, employeeTraining.TrainingProgramID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeTraining.EmployeeID);
            ViewData["TrainingProgramID"] = new SelectList(_context.TrainingPrograms, "TrainingProgramID", "ProgramName", employeeTraining.TrainingProgramID);
            return View(employeeTraining);
        }

        // GET: EmployeeTraining/Delete/5/5
        public async Task<IActionResult> Delete(int? employeeId, int? trainingProgramId)
        {
            if (employeeId == null || trainingProgramId == null)
            {
                return NotFound();
            }

            var employeeTraining = await _context.EmployeeTrainings
                .Include(et => et.Employee)
                .Include(et => et.TrainingProgram)
                .FirstOrDefaultAsync(et => et.EmployeeID == employeeId && et.TrainingProgramID == trainingProgramId);

            if (employeeTraining == null)
            {
                return NotFound();
            }

            return View(employeeTraining);
        }

        // POST: EmployeeTraining/Delete/5/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId, int trainingProgramId)
        {
            var employeeTraining = await _context.EmployeeTrainings.FindAsync(employeeId, trainingProgramId);
            _context.EmployeeTrainings.Remove(employeeTraining);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeTrainingExists(int employeeId, int trainingProgramId)
        {
            return _context.EmployeeTrainings.Any(et => et.EmployeeID == employeeId && et.TrainingProgramID == trainingProgramId);
        }
    }
}
