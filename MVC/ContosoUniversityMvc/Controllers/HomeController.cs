using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityMvc.Models.SchoolViewModels;
using ContosoUniversityMvc.Data;

namespace ContosoUniversityMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> About()
    {
        // IQueryable<EnrollmentDateGroup> data =
        //     from s in _context.Students
        //     group s by s.EnrollmentDate into dateGroup
        //     select new EnrollmentDateGroup
        //     {
        //         EnrollmentDate = dateGroup.Key,
        //         StudentCount = dateGroup.Count()
        //     };
        // return View(await data.AsNoTracking().ToListAsync());

        // Execute a raw SQL query using ADO.net directly

        var groups = new List<EnrollmentDateGroup>();

        // Get the DB connection from Entity Framework
        var conn = _context.Database.GetDbConnection();
        try
        {
            await conn.OpenAsync();
            using (var cmd = conn.CreateCommand())
            {
                string query = @"SELECT EnrollmentDate, COUNT(*) AS StudentCount
                                FROM Student
                                GROUP BY EnrollmentDate";
                cmd.CommandText = query;

                var reader = await cmd.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        var row = new EnrollmentDateGroup
                        {
                            EnrollmentDate = reader.GetDateTime(0),
                            StudentCount = reader.GetInt32(1)
                        };
                        groups.Add(row);
                    }
                }
                reader.Dispose();
            }
        }
        finally
        {
            conn.Close();
        }
        return View(groups);
    }
}
