using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityReverse.Data;
using ContosoUniversityReverse.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversityReverse.Controllers;

public class InstructorsController : Controller
{
    private readonly ApplicationDbContext _context;

    public InstructorsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchText, string sortOrder, int? page)
    {
        IQueryable<Instructor> query = _context.Instructors
            .Include(x => x.OfficeAssignment)
            .Include(x => x.Courses)
            .AsNoTracking();

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
        }
        else
        {
            query = query.OrderBy(x => EF.Property<object>(x, sortOrder));
        }

        return View(await IndexPagination.QueryAndViewData(query, searchText, sortOrder, page, ViewData, 5));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .Include(x => x.OfficeAssignment)
            .Include(x => x.Courses)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id);

        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .Include(x => x.OfficeAssignment)
            .Include(x => x.Courses)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id);

        if (instructor == null)
        {
            return NotFound();
        }

        return View(instructor);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructorToDelete = await _context.Instructors.FindAsync(id);

        if (instructorToDelete != null)
        {
            var departments = _context.Departments.Where(x => x.InstructorId == id);
            foreach (var department in departments)
            {
                department.Instructor = null;
            }
            _context.Instructors.Remove(instructorToDelete);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }


    private void PopulateCoursesList(Instructor instructor)
    {
        var instructorCourses = new HashSet<int>(instructor.Courses.Select(x => x.CourseId));
        var courses = from c in _context.Courses
                      orderby c.CourseId
                      select new
                      {
                          Value = c.CourseId,
                          Text = $"{c.CourseId} {c.Title}",
                          Selected = instructorCourses.Contains(c.CourseId)
                      };
        ViewBag.Courses = courses;
    }

    private void UpdateInstructorCourses(Instructor instructor, string[] selectedCourses)
    {
        var selectedCoursesHS = new HashSet<string>(selectedCourses);
        var instructorCoursesHS = new HashSet<int>(instructor.Courses.Select(x => x.CourseId));
        var allCourses = _context.Courses;

        foreach (var course in allCourses)
        {
            if (selectedCoursesHS.Contains(course.CourseId.ToString()))
            {
                if (!instructorCoursesHS.Contains(course.CourseId))
                {
                    instructor.Courses.Add(course);
                }
            }
            else
            {
                if (instructorCoursesHS.Contains(course.CourseId))
                {
                    instructor.Courses.Remove(course);
                }
            }
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructor = await _context.Instructors
            .Include(x => x.OfficeAssignment)
            .Include(x => x.Courses)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id);

        if (instructor == null)
        {
            return NotFound();
        }
        PopulateCoursesList(instructor);
        return View(instructor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
    {
        if (id == null)
        {
            return NotFound();
        }

        var instructorToUpdate = await _context.Instructors
            .Include(x => x.OfficeAssignment)
            .Include(x => x.Courses)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (instructorToUpdate == null)
        {
            return NotFound();
        }

        if (await TryUpdateModelAsync<Instructor>(instructorToUpdate, "", x => x.FirstName, x => x.LastName, x => x.HireDate, x => x.OfficeAssignment))
        {
            if (string.IsNullOrEmpty(instructorToUpdate.OfficeAssignment.Location))
            {
                instructorToUpdate.OfficeAssignment = null;
            }
            UpdateInstructorCourses(instructorToUpdate, selectedCourses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopulateCoursesList(instructorToUpdate);
        return View(instructorToUpdate);
    }

    public IActionResult Create()
    {
        var instructor = new Instructor();
        PopulateCoursesList(instructor);
        return View(instructor);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string[] selectedCourses)
    {
        var instructor = new Instructor();

        if (await TryUpdateModelAsync<Instructor>(instructor, "", x => x.FirstName, x => x.LastName, x => x.HireDate, x => x.OfficeAssignment))
        {
            if (string.IsNullOrEmpty(instructor.OfficeAssignment.Location))
            {
                instructor.OfficeAssignment = null;
            }
            UpdateInstructorCourses(instructor, selectedCourses);
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        PopulateCoursesList(instructor);
        return View(instructor);
    }
}
