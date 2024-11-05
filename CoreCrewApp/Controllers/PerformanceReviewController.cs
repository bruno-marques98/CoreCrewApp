using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PerformanceReviewController : Controller
    {
        private readonly AppDbContext _context;

        public PerformanceReviewController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PerformanceReview
        public async Task<IActionResult> Index()
        {
            var reviews = _context.PerformanceReviews
                .Include(pr => pr.Employee); // Include Employee info
            return View(await reviews.ToListAsync());
        }

        // GET: PerformanceReview/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceReview = await _context.PerformanceReviews
                .Include(pr => pr.Employee)
                .FirstOrDefaultAsync(pr => pr.PerformanceReviewID == id);

            if (performanceReview == null)
            {
                return NotFound();
            }

            return View(performanceReview);
        }

        // GET: PerformanceReview/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName");
            return View();
        }

        // POST: PerformanceReview/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PerformanceReviewID,EmployeeID,ReviewDate,ReviewComments,Rating")] PerformanceReview performanceReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(performanceReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", performanceReview.EmployeeID);
            return View(performanceReview);
        }

        // GET: PerformanceReview/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceReview = await _context.PerformanceReviews.FindAsync(id);
            if (performanceReview == null)
            {
                return NotFound();
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", performanceReview.EmployeeID);
            return View(performanceReview);
        }

        // POST: PerformanceReview/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PerformanceReviewID,EmployeeID,ReviewDate,ReviewComments,Rating")] PerformanceReview performanceReview)
        {
            if (id != performanceReview.PerformanceReviewID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(performanceReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerformanceReviewExists(performanceReview.PerformanceReviewID))
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

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", performanceReview.EmployeeID);
            return View(performanceReview);
        }

        // GET: PerformanceReview/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var performanceReview = await _context.PerformanceReviews
                .Include(pr => pr.Employee)
                .FirstOrDefaultAsync(pr => pr.PerformanceReviewID == id);

            if (performanceReview == null)
            {
                return NotFound();
            }

            return View(performanceReview);
        }

        // POST: PerformanceReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var performanceReview = await _context.PerformanceReviews.FindAsync(id);
            _context.PerformanceReviews.Remove(performanceReview);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerformanceReviewExists(int id)
        {
            return _context.PerformanceReviews.Any(e => e.PerformanceReviewID == id);
        }
    }
}
