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

namespace ContosoUniversity.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName");
            return Page();
        }

        [BindProperty]
        public Department Department { get; set; }
        public SelectList InstructorNameSL { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyDepartment = new Department();

            if (await TryUpdateModelAsync<Department>(
                emptyDepartment, 
                "Department",
                d => d.Name,
                d => d.Budget,
                d => d.StartDate,
                d => d.InstructorID))
            {
                _context.Departments.Add(emptyDepartment);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            InstructorNameSL = new SelectList(_context.Instructors, 
                "ID", "FullName", emptyDepartment.InstructorID);
            return Page();
        }
    }
}
