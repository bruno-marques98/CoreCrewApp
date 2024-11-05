using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BenefitController : Controller
    {
        private readonly AppDbContext _context;

        public BenefitController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Benefit
        public async Task<IActionResult> Index()
        {
            return View(await _context.Benefits.ToListAsync());
        }

        // GET: Benefit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefit = await _context.Benefits
                .FirstOrDefaultAsync(m => m.BenefitID == id);
            if (benefit == null)
            {
                return NotFound();
            }

            return View(benefit);
        }

        // GET: Benefit/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Benefit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BenefitID,Name,Description,Cost")] Benefit benefit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(benefit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(benefit);
        }

        // GET: Benefit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefit = await _context.Benefits.FindAsync(id);
            if (benefit == null)
            {
                return NotFound();
            }
            return View(benefit);
        }

        // POST: Benefit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BenefitID,Name,Description,Cost")] Benefit benefit)
        {
            if (id != benefit.BenefitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benefit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BenefitExists(benefit.BenefitID))
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
            return View(benefit);
        }

        // GET: Benefit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var benefit = await _context.Benefits
                .FirstOrDefaultAsync(m => m.BenefitID == id);
            if (benefit == null)
            {
                return NotFound();
            }

            return View(benefit);
        }

        // POST: Benefit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var benefit = await _context.Benefits.FindAsync(id);
            _context.Benefits.Remove(benefit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BenefitExists(int id)
        {
            return _context.Benefits.Any(e => e.BenefitID == id);
        }
    }
}
