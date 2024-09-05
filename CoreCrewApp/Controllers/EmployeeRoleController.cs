﻿using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers
{
    public class EmployeeRoleController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeRoleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeRole
        public async Task<IActionResult> Index()
        {
            var employeeRoles = _context.EmployeeRoles
                .Include(er => er.Employee)
                .Include(er => er.Role);
            return View(await employeeRoles.ToListAsync());
        }

        // GET: EmployeeRole/Details/5/5
        public async Task<IActionResult> Details(int? employeeId, int? roleId)
        {
            if (employeeId == null || roleId == null)
            {
                return NotFound();
            }

            var employeeRole = await _context.EmployeeRoles
                .Include(er => er.Employee)
                .Include(er => er.Role)
                .FirstOrDefaultAsync(er => er.EmployeeID == employeeId && er.RoleID == roleId);

            if (employeeRole == null)
            {
                return NotFound();
            }

            return View(employeeRole);
        }

        // GET: EmployeeRole/Create
        public IActionResult Create()
        {
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName");
            ViewData["RoleID"] = new SelectList(_context.Roles, "RoleID", "RoleName");
            return View();
        }

        // POST: EmployeeRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeID,RoleID,AssignedDate")] EmployeeRole employeeRole)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employeeRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeRole.EmployeeID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "RoleID", "RoleName", employeeRole.RoleID);
            return View(employeeRole);
        }

        // GET: EmployeeRole/Edit/5/5
        public async Task<IActionResult> Edit(int? employeeId, int? roleId)
        {
            if (employeeId == null || roleId == null)
            {
                return NotFound();
            }

            var employeeRole = await _context.EmployeeRoles
                .FindAsync(employeeId, roleId);

            if (employeeRole == null)
            {
                return NotFound();
            }

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeRole.EmployeeID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "RoleID", "RoleName", employeeRole.RoleID);
            return View(employeeRole);
        }

        // POST: EmployeeRole/Edit/5/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int employeeId, int roleId, [Bind("EmployeeID,RoleID,AssignedDate")] EmployeeRole employeeRole)
        {
            if (employeeId != employeeRole.EmployeeID || roleId != employeeRole.RoleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeRoleExists(employeeRole.EmployeeID, employeeRole.RoleID))
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

            ViewData["EmployeeID"] = new SelectList(_context.Employees, "EmployeeID", "FirstName", employeeRole.EmployeeID);
            ViewData["RoleID"] = new SelectList(_context.Roles, "RoleID", "RoleName", employeeRole.RoleID);
            return View(employeeRole);
        }

        // GET: EmployeeRole/Delete/5/5
        public async Task<IActionResult> Delete(int? employeeId, int? roleId)
        {
            if (employeeId == null || roleId == null)
            {
                return NotFound();
            }

            var employeeRole = await _context.EmployeeRoles
                .Include(er => er.Employee)
                .Include(er => er.Role)
                .FirstOrDefaultAsync(er => er.EmployeeID == employeeId && er.RoleID == roleId);

            if (employeeRole == null)
            {
                return NotFound();
            }

            return View(employeeRole);
        }

        // POST: EmployeeRole/Delete/5/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId, int roleId)
        {
            var employeeRole = await _context.EmployeeRoles.FindAsync(employeeId, roleId);
            _context.EmployeeRoles.Remove(employeeRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeRoleExists(int employeeId, int roleId)
        {
            return _context.EmployeeRoles.Any(er => er.EmployeeID == employeeId && er.RoleID == roleId);
        }
    }
}