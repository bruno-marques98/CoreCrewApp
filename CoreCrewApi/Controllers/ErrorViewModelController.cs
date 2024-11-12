using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreCrewApi.Data;
using CoreCrewApi.Models;

namespace CoreCrewApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorViewModelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ErrorViewModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ErrorViewModel
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ErrorViewModel>>> GetErrorViewModel()
        {
            return await _context.ErrorViewModel.ToListAsync();
        }

        // GET: api/ErrorViewModel/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ErrorViewModel>> GetErrorViewModel(string id)
        {
            var errorViewModel = await _context.ErrorViewModel.FindAsync(id);

            if (errorViewModel == null)
            {
                return NotFound();
            }

            return errorViewModel;
        }

        // PUT: api/ErrorViewModel/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutErrorViewModel(string id, ErrorViewModel errorViewModel)
        {
            if (id != errorViewModel.RequestId)
            {
                return BadRequest();
            }

            _context.Entry(errorViewModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ErrorViewModelExists(id))
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

        // POST: api/ErrorViewModel
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ErrorViewModel>> PostErrorViewModel(ErrorViewModel errorViewModel)
        {
            _context.ErrorViewModel.Add(errorViewModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ErrorViewModelExists(errorViewModel.RequestId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetErrorViewModel", new { id = errorViewModel.RequestId }, errorViewModel);
        }

        // DELETE: api/ErrorViewModel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteErrorViewModel(string id)
        {
            var errorViewModel = await _context.ErrorViewModel.FindAsync(id);
            if (errorViewModel == null)
            {
                return NotFound();
            }

            _context.ErrorViewModel.Remove(errorViewModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ErrorViewModelExists(string id)
        {
            return _context.ErrorViewModel.Any(e => e.RequestId == id);
        }
    }
}
