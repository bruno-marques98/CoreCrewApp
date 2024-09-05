using CoreCrewApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrewApp.Controllers
{
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
