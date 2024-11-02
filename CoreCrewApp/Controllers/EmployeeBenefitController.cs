using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    public class EmployeeBenefitController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeBenefitController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeBenefit
        public async Task<IActionResult> Index()
        {
            var employeeBenefits = _context.EmployeeBenefits
                .Include(eb => eb.Employee)
                .Include(eb => eb.Benefit);
            return View(await employeeBenefits.ToListAsync());
        }

        // GET: EmployeeBenefit/Details/5/5
        public async Task<IActionResult> Details(int? employeeId, int? benefitId)
        {
            if (employeeId == null || benefitId == null)
            {
                return NotFound();
            }

            var employeeBenefit = await _context.EmployeeBenefits
                .Include(eb => eb.Employee)
                .Include(eb => eb.Benefit)
                .FirstOrDefaultAsync(eb => eb.EmployeeID == employeeId && eb.BenefitID == benefitId);
            if (employeeBenefit == null)
            {
                return NotFound();
            }

            return View(employeeBenefit);
        }

        // GET: EmployeeBenefit/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName");
            ViewData["BenefitID"] = new SelectList(_context.Benefits, "BenefitID", "Name");
            return View();
        }

        // POST: EmployeeBenefit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,BenefitID,EnrollmentDate")] EmployeeBenefit employeeBenefit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeBenefit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeBenefit.EmployeeID);
            ViewData["BenefitID"] = new SelectList(_context.Benefits, "BenefitID", "Name", employeeBenefit.BenefitID);
            return View(employeeBenefit);
        }

        // GET: EmployeeBenefit/Edit/5/5
        public async Task<IActionResult> Edit(int? employeeId, int? benefitId)
        {
            if (employeeId == null || benefitId == null)
            {
                return NotFound();
            }

            var employeeBenefit = await _context.EmployeeBenefits
                .FindAsync(employeeId, benefitId);
            if (employeeBenefit == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeBenefit.EmployeeID);
            ViewData["BenefitID"] = new SelectList(_context.Benefits, "BenefitID", "Name", employeeBenefit.BenefitID);
            return View(employeeBenefit);
        }

        // POST: EmployeeBenefit/Edit/5/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int employeeId, int benefitId, [Bind("EmployeeID,BenefitID,EnrollmentDate")] EmployeeBenefit employeeBenefit)
        {
            if (employeeId != employeeBenefit.EmployeeID || benefitId != employeeBenefit.BenefitID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeBenefit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeBenefitExists(employeeBenefit.EmployeeID, employeeBenefit.BenefitID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeBenefit.EmployeeID);
            ViewData["BenefitID"] = new SelectList(_context.Benefits, "BenefitID", "Name", employeeBenefit.BenefitID);
            return View(employeeBenefit);
        }

        // GET: EmployeeBenefit/Delete/5/5
        public async Task<IActionResult> Delete(int? employeeId, int? benefitId)
        {
            if (employeeId == null || benefitId == null)
            {
                return NotFound();
            }

            var employeeBenefit = await _context.EmployeeBenefits
                .Include(eb => eb.Employee)
                .Include(eb => eb.Benefit)
                .FirstOrDefaultAsync(eb => eb.EmployeeID == employeeId && eb.BenefitID == benefitId);
            if (employeeBenefit == null)
            {
                return NotFound();
            }

            return View(employeeBenefit);
        }

        // POST: EmployeeBenefit/Delete/5/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId, int benefitId)
        {
            var employeeBenefit = await _context.EmployeeBenefits
                .FindAsync(employeeId, benefitId);
            _context.EmployeeBenefits.Remove(employeeBenefit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeBenefitExists(int employeeId, int benefitId)
        {
            return _context.EmployeeBenefits.Any(e => e.EmployeeID == employeeId && e.BenefitID == benefitId);
        }
    }
}
