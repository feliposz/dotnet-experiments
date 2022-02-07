using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityMvc.Data;
using ContosoUniversityMvc.Models;
using ContosoUniversityMvc.Models.SchoolViewModels;

namespace ContosoUniversityMvc.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InstructorsController> _logger;

        public InstructorsController(ApplicationDbContext context, ILogger<InstructorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Instructors
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                // .Include(i => i.CourseAssignments)
                //     .ThenInclude(a => a.Course)
                //         .ThenInclude(c => c.Enrollments)
                //             .ThenInclude(e => e.Student)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(a => a.Course)
                        .ThenInclude(c => c.Department)
                //.AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = viewModel.Instructors.Single(i => i.ID == id.Value);
                viewModel.Courses = instructor.CourseAssignments.Select(a => a.Course);
            }

            if (courseId != null)
            {
                ViewData["CourseID"] = courseId.Value;
                //Course course = viewModel.Courses.Single(c => c.CourseID == courseId.Value);
                Course course = viewModel.Courses.Single(c => c.CourseID == courseId.Value);
                await _context.Entry(course).Collection(c => c.Enrollments).LoadAsync(); // Collection -> one-to-many
                foreach (var enrollment in course.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(e => e.Student).LoadAsync(); // Reference -> many-to-one
                }
                viewModel.Enrollments = course.Enrollments;
            }
            return View(viewModel);
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            var instructor = new Instructor();
            PopulateAssignedCourseData(instructor);
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstMidName,HireDate")] Instructor instructor, string[] selectedCourses)
        {
            // NOTE: courses are added even if model is invalid, so that selected courses are kept when page is redisplayed
            if (selectedCourses != null)
            {
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment { InstructorID = instructor.ID, CourseID = int.Parse(course) };
                    instructor.CourseAssignments.Add(courseToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .AsNoTracking()
                .SingleOrDefaultAsync(i => i.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }
            PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
            ViewBag.Courses = viewModel;
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCourses)
        {
            var instructorToUpdate = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                    .ThenInclude(a => a.Course)
                .FirstOrDefaultAsync(i => i.ID == id);

            if (instructorToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Instructor>(
                instructorToUpdate,
                "",
                i => i.FirstMidName,
                i => i.LastName,
                i => i.HireDate,
                i => i.OfficeAssignment))
            {
                // If user erased location, then remove reference to Office Assignment. EF will delete the record on DB.
                if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
                {
                    instructorToUpdate.OfficeAssignment = null;
                }
                UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError("Edit Instructor", ex);
                    ModelState.AddModelError("", "Unable to save changes.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedCourseData(instructorToUpdate);
            UpdateInstructorCourses(selectedCourses, instructorToUpdate);
            return View(instructorToUpdate);
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.CourseAssignments = new List<CourseAssignment>();
                return;
            }
            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (instructorToUpdate.CourseAssignments.Select(c => c.Course.CourseID));
            foreach (var course in _context.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                {
                    if (!instructorCourses.Contains(course.CourseID))
                    {
                        var courseToAdd = new CourseAssignment
                        {
                            InstructorID = instructorToUpdate.ID,
                            CourseID = course.CourseID
                        };
                        instructorToUpdate.CourseAssignments.Add(courseToAdd);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseID))
                    {
                        var courseToRemove = instructorToUpdate.CourseAssignments
                            .FirstOrDefault(a => a.CourseID == course.CourseID);
                        _context.CourseAssignments.Remove(courseToRemove!);
                    }
                }
            }
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors
                .Include(i => i.CourseAssignments) // Track assignments to allow EF to apply the cascade delete properly
                .SingleAsync(i => i.ID == id);

            // If this instructor is assigned to any department, clear the assignment
            var departments = await _context.Departments
                .Where(d => d.InstructorID == id)
                .ToListAsync();
            departments.ForEach(d => d.InstructorID = null);

            _context.Instructors.Remove(instructor!);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
    }
}
