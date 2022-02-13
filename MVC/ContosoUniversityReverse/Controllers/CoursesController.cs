using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversityReverse.Data;
using ContosoUniversityReverse.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ContosoUniversityReverse.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchText, string sortOrder, int? page)
        {
            IQueryable<Course> query = _context.Courses.Include(x => x.Department);

            if (!String.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.Title.Contains(searchText) || x.Department.Name.Contains(searchText));
            }

            if (String.IsNullOrWhiteSpace(sortOrder))
            {
                sortOrder = "CourseId";
            }

            if (sortOrder == "Department")
            {
                query = query.OrderBy(x => x.Department.Name);
            }
            else if (sortOrder == "Department_desc")
            {
                query = query.OrderByDescending(x => x.Department.Name);
            }
            else if (sortOrder.EndsWith("_desc"))
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
            var course = await _context.Courses
                .Include(x => x.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        private void PopulateDepartmentsDropDownList(object? departmentId = null)
        {
            var query = from d in _context.Departments
                        orderby d.Name
                        select d;

            ViewBag.DepartmentID = new SelectList(query.AsNoTracking(), "DepartmentId", "Name", departmentId);
        }

        public IActionResult Create()
        {
            PopulateDepartmentsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .Include(x => x.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            PopulateDepartmentsDropDownList(course.DepartmentId);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseToUpdate = await _context.Courses.SingleOrDefaultAsync(x => x.CourseId == id);
            if (courseToUpdate == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync<Course>(courseToUpdate, "", x => x.Title, x => x.DepartmentId))
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentId);
            return View(courseToUpdate);
        }

        public async Task<IActionResult> Delete(int? id, bool? errorOnDelete = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .Include(x => x.Department)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            if (errorOnDelete.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Error trying to delete course.";
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseToDelete = await _context.Courses.FindAsync(id);
            if (courseToDelete != null)
            {
                try
                {
                    _context.Courses.Remove(courseToDelete);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return RedirectToAction(nameof(Delete), new { id = id, errorOnDelete = true });
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}