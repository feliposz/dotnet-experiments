using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversityReverse.Data;
using ContosoUniversityReverse.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ContosoUniversityReverse.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchText, string sortOrder, int? page)
        {
            IQueryable<Department> query = _context.Departments.Include(x => x.Instructor);

            if (!String.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.Name.Contains(searchText) || x.Instructor.LastName.Contains(searchText) || x.Instructor.FirstName.Contains(searchText));
            }

            if (String.IsNullOrWhiteSpace(sortOrder))
            {
                sortOrder = "Name";
            }

            if (sortOrder == "Instructor")
            {
                query = query.OrderBy(x => x.Instructor.LastName).ThenBy(x => x.Instructor.FirstName);
            }
            else if (sortOrder == "Instructor_desc")
            {
                query = query.OrderByDescending(x => x.Instructor.LastName).ThenByDescending(x => x.Instructor.FirstName);
            }
            else if (sortOrder.EndsWith("_desc"))
            {
                query = query.OrderByDescending(x => EF.Property<object>(x, sortOrder.Replace("_desc", "")));
            }
            else
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
            var department = await _context.Departments
                .Include(x => x.Instructor)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        private void PopulateInstructorsDropDownList(object? instructorId = null)
        {
            var query = from x in _context.Instructors
                        orderby x.LastName
                        select x;

            ViewBag.InstructorID = new SelectList(query.AsNoTracking(), "Id", "FullName", instructorId);
        }

        public IActionResult Create()
        {
            PopulateInstructorsDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateInstructorsDropDownList(department.InstructorId);
            return View(department);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments
                .Include(x => x.Instructor)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }
            PopulateInstructorsDropDownList(department.InstructorId);
            return View(department);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var departmentToUpdate = await _context.Departments.SingleOrDefaultAsync(x => x.DepartmentId == id);
            if (departmentToUpdate == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync<Department>(departmentToUpdate, "", x => x.Name, x => x.Budget, x => x.StartDate, x => x.InstructorId))
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            PopulateInstructorsDropDownList(departmentToUpdate.InstructorId);
            return View(departmentToUpdate);
        }

        public async Task<IActionResult> Delete(int? id, bool? errorOnDelete = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments
                .Include(x => x.Instructor)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.DepartmentId == id);
            if (department == null)
            {
                return NotFound();
            }
            if (errorOnDelete.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Error trying to delete department.";
            }
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var departmentToDelete = await _context.Departments.FindAsync(id);
            if (departmentToDelete != null)
            {
                try
                {
                    _context.Departments.Remove(departmentToDelete);
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