using CoreCrewApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrewApp.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
