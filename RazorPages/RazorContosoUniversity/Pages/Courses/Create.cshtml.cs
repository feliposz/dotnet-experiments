#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContosoUniversity.Data;
using ContosoUniversity.Models;

namespace ContosoUniversity.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateDepartmentDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            var emptyCourse = new Course();

            // Prevent overposting
            if (await TryUpdateModelAsync<Course>(
                emptyCourse,
                "course", // prefix for form value
                s => s.CourseID,
                s => s.DepartmentID,
                s => s.Title,
                s => s.Credits))
            {
                _context.Courses.Add(emptyCourse);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateDepartmentDropDownList(_context, emptyCourse.DepartmentID);
            return Page();
        }
    }
}
