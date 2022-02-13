using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityReverse.Data;
using ContosoUniversityReverse.Models;
using ContosoUniversityReverse.Models.ViewModels;

namespace ContosoUniversityReverse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            var query =
                from s in _context.Students
                group s by s.EnrollmentDate.Year into groupDate
                select new EnrollmentDateGroup
                {
                    Year = groupDate.Key,
                    StudentCount = groupDate.Count()
                };
            return View(await query.ToListAsync());
        }
    }
}