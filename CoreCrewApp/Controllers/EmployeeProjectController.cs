using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    [Authorize]
    public class EmployeeProjectController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeProjectController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeProject
        public async Task<IActionResult> Index()
        {
            var employeeProjects = _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project);
            return View(await employeeProjects.ToListAsync());
        }

        // GET: EmployeeProject/Details/5/5
        public async Task<IActionResult> Details(int? employeeId, int? projectId)
        {
            if (employeeId == null || projectId == null)
            {
                return NotFound();
            }

            var employeeProject = await _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .FirstOrDefaultAsync(ep => ep.EmployeeID == employeeId && ep.ProjectID == projectId);
            if (employeeProject == null)
            {
                return NotFound();
            }

            return View(employeeProject);
        }

        // GET: EmployeeProject/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName");
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Name");
            return View();
        }

        // POST: EmployeeProject/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,ProjectID,AssignmentDate")] EmployeeProject employeeProject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeProject.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Name", employeeProject.ProjectID);
            return View(employeeProject);
        }

        // GET: EmployeeProject/Edit/5/5
        public async Task<IActionResult> Edit(int? employeeId, int? projectId)
        {
            if (employeeId == null || projectId == null)
            {
                return NotFound();
            }

            var employeeProject = await _context.EmployeeProjects
                .FindAsync(employeeId, projectId);
            if (employeeProject == null)
            {
                return NotFound();
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeProject.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Name", employeeProject.ProjectID);
            return View(employeeProject);
        }

        // POST: EmployeeProject/Edit/5/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int employeeId, int projectId, [Bind("EmployeeID,ProjectID,AssignmentDate")] EmployeeProject employeeProject)
        {
            if (employeeId != employeeProject.EmployeeID || projectId != employeeProject.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeProjectExists(employeeProject.EmployeeID, employeeProject.ProjectID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeProject.EmployeeID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "ProjectID", "Name", employeeProject.ProjectID);
            return View(employeeProject);
        }

        // GET: EmployeeProject/Delete/5/5
        public async Task<IActionResult> Delete(int? employeeId, int? projectId)
        {
            if (employeeId == null || projectId == null)
            {
                return NotFound();
            }

            var employeeProject = await _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .FirstOrDefaultAsync(ep => ep.EmployeeID == employeeId && ep.ProjectID == projectId);
            if (employeeProject == null)
            {
                return NotFound();
            }

            return View(employeeProject);
        }

        // POST: EmployeeProject/Delete/5/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId, int projectId)
        {
            var employeeProject = await _context.EmployeeProjects.FindAsync(employeeId, projectId);
            _context.EmployeeProjects.Remove(employeeProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeProjectExists(int employeeId, int projectId)
        {
            return _context.EmployeeProjects.Any(ep => ep.EmployeeID == employeeId && ep.ProjectID == projectId);
        }
    }
}
