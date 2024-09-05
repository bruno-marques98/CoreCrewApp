using CoreCrewApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrewApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
