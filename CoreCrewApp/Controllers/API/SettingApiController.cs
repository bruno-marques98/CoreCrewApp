using CoreCrewApp.Data;
using CoreCrewApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreCrewApp.Controllers.API
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SettingApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Setting
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Setting>>> GetSettings()
        {
            var settings = await _context.Settings.ToListAsync();
            return Ok(settings);
        }

        // GET: api/Setting/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Setting>> GetSetting(int id)
        {
            var setting = await _context.Settings
                .FirstOrDefaultAsync(m => m.SettingID == id);

            if (setting == null)
            {
                return NotFound();
            }

            return Ok(setting);
        }

        // POST: api/Setting
        [HttpPost]
        public async Task<ActionResult<Setting>> CreateSetting([FromBody] Setting setting)
        {
            if (setting == null)
            {
                return BadRequest();
            }

            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSetting), new { id = setting.SettingID }, setting);
        }

        // PUT: api/Setting/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSetting(int id, [FromBody] Setting setting)
        {
            if (id != setting.SettingID)
            {
                return BadRequest();
            }

            _context.Entry(setting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SettingExists(id))
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

        // DELETE: api/Setting/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSetting(int id)
        {
            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            _context.Settings.Remove(setting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SettingExists(int id)
        {
            return _context.Settings.Any(e => e.SettingID == id);
        }
    }
}
