using CoreCrewApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrewApp.Controllers
{
    public class SalaryController : Controller
    {
        private readonly AppDbContext _context;

        public SalaryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
