using CoreCrewApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CoreCrewApp.Controllers
{
    public class TrainingProgramController : Controller
    {
        private readonly AppDbContext _context;

        public TrainingProgramController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
