using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ContosoUniversityReverse.Data;
using ContosoUniversityReverse.Models;

namespace ContosoUniversityReverse.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchText, string sortOrder, int? page)
        {
            IQueryable<Student> query = _context.Students;

            if (!String.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.LastName.Contains(searchText) || x.FirstName.Contains(searchText));
            }

            if (String.IsNullOrWhiteSpace(sortOrder))
            {
                sortOrder = "LastName";
            }

            if (sortOrder.EndsWith("_desc"))
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, sortOrder.Replace("_desc", "")));
            } else
            {
                query = query.OrderBy(x => EF.Property<object>(x, sortOrder));
            }

            return View(await IndexPagination.QueryAndViewData(query, searchText, sortOrder, page, ViewData));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(x => x.Enrollments)
                    .ThenInclude(x => x.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            var studentToUpdate = await _context.Students.FindAsync(id);
            if (studentToUpdate == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (await TryUpdateModelAsync<Student>(
                        studentToUpdate,
                        "",
                        x => x.FirstName,
                        x => x.LastName,
                        x => x.EnrollmentDate))
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }
            return View(studentToUpdate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var studentToDelete = await _context.Students.FindAsync(id);
            if (studentToDelete == null)
            {
                return NotFound();
            }
            try
            {
                _context.Students.Remove(studentToDelete);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }
            return View(studentToDelete);
        }
    }
}
