using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    public class TrainingProgramController : Controller
    {
        private readonly AppDbContext _context;

        public TrainingProgramController(AppDbContext context)
        {
            _context = context;
        }
        // GET: TrainingProgram
        public async Task<IActionResult> Index()
        {
            var trainingPrograms = _context.TrainingPrograms.Include(tp => tp.Trainer);
            return View(await trainingPrograms.ToListAsync());
        }

        // GET: TrainingProgram/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingProgram = await _context.TrainingPrograms
                .Include(tp => tp.Trainer)
                .FirstOrDefaultAsync(m => m.TrainingProgramID == id);

            if (trainingProgram == null)
            {
                return NotFound();
            }

            return View(trainingProgram);
        }

        // GET: TrainingProgram/Create
        public IActionResult Create()
        {
            ViewData["TrainerID"] = new SelectList(_context.Employees, "EmployeeID", "FullName"); // Assuming FullName property exists
            return View();
        }

        // POST: TrainingProgram/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingProgramID,ProgramName,Description,StartDate,EndDate,TrainerID")] TrainingProgram trainingProgram)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingProgram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainerID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", trainingProgram.TrainerID);
            return View(trainingProgram);
        }

        // GET: TrainingProgram/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingProgram = await _context.TrainingPrograms.FindAsync(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }
            ViewData["TrainerID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", trainingProgram.TrainerID);
            return View(trainingProgram);
        }

        // POST: TrainingProgram/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingProgramID,ProgramName,Description,StartDate,EndDate,TrainerID")] TrainingProgram trainingProgram)
        {
            if (id != trainingProgram.TrainingProgramID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingProgram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingProgramExists(trainingProgram.TrainingProgramID))
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
            ViewData["TrainerID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", trainingProgram.TrainerID);
            return View(trainingProgram);
        }

        // GET: TrainingProgram/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingProgram = await _context.TrainingPrograms
                .Include(tp => tp.Trainer)
                .FirstOrDefaultAsync(m => m.TrainingProgramID == id);

            if (trainingProgram == null)
            {
                return NotFound();
            }

            return View(trainingProgram);
        }

        // POST: TrainingProgram/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingProgram = await _context.TrainingPrograms.FindAsync(id);
            _context.TrainingPrograms.Remove(trainingProgram);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingProgramExists(int id)
        {
            return _context.TrainingPrograms.Any(e => e.TrainingProgramID == id);
        }
    }
}
