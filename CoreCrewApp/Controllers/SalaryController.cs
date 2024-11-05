using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalaryController : Controller
    {
        private readonly AppDbContext _context;

        public SalaryController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Salary
        public async Task<IActionResult> Index()
        {
            var salaries = _context.Salaries.Include(s => s.Employee);
            return View(await salaries.ToListAsync());
        }

        // GET: Salary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.SalaryID == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // GET: Salary/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName");
            return View();
        }

        // POST: Salary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalaryID,EmployeeID,Amount,EffectiveDate,EndDate")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", salary.EmployeeID);
            return View(salary);
        }

        // GET: Salary/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", salary.EmployeeID);
            return View(salary);
        }

        // POST: Salary/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalaryID,EmployeeID,Amount,EffectiveDate,EndDate")] Salary salary)
        {
            if (id != salary.SalaryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.SalaryID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FullName", salary.EmployeeID);
            return View(salary);
        }

        // GET: Salary/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salaries
                .Include(s => s.Employee)
                .FirstOrDefaultAsync(m => m.SalaryID == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // POST: Salary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.SalaryID == id);
        }
    }
}
