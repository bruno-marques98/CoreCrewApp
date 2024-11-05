using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly AppDbContext _context;

        public LeaveRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: LeaveRequest
        public async Task<IActionResult> Index()
        {
            var leaveRequests = _context.LeaveRequests
                .Include(lr => lr.Employee); // Include the employee data for each leave request
            return View(await leaveRequests.ToListAsync());
        }

        // GET: LeaveRequest/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee) // Include the employee data
                .FirstOrDefaultAsync(lr => lr.LeaveRequestID == id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // GET: LeaveRequest/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName");
            return View();
        }

        // POST: LeaveRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveRequestID,EmployeeID,StartDate,EndDate,Reason,Status")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                leaveRequest.Status = LeaveStatus.Pending; // Set status to Pending initially
                _context.Add(leaveRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", leaveRequest.EmployeeID);
            return View(leaveRequest);
        }

        // GET: LeaveRequest/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound();
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", leaveRequest.EmployeeID);
            return View(leaveRequest);
        }

        // POST: LeaveRequest/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveRequestID,EmployeeID,StartDate,EndDate,Reason,Status")] LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.LeaveRequestID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveRequestExists(leaveRequest.LeaveRequestID))
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
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", leaveRequest.EmployeeID);
            return View(leaveRequest);
        }

        // GET: LeaveRequest/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveRequest = await _context.LeaveRequests
                .Include(lr => lr.Employee)
                .FirstOrDefaultAsync(m => m.LeaveRequestID == id);

            if (leaveRequest == null)
            {
                return NotFound();
            }

            return View(leaveRequest);
        }

        // POST: LeaveRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            _context.LeaveRequests.Remove(leaveRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveRequestExists(int id)
        {
            return _context.LeaveRequests.Any(lr => lr.LeaveRequestID == id);
        }
    }
}
