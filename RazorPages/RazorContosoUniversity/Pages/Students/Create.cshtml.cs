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

namespace ContosoUniversity.Pages.Students
{
    public class StudentVM
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }

    public class CreateModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public CreateModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public StudentVM Student { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            /*
            // Prevent overposting with TryUpdate (https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-6.0#overposting)
            var emptyStudent = new Student();

            if (await TryUpdateModelAsync<Student>(
                emptyStudent,
                "student", // prefix for form value
                s => s.FirstMidName,
                s => s.LastName,
                s => s.EnrollmentDate))
            {
                _context.Students.Add(emptyStudent);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            return Page();
            */
            
            // Prevent overposting with a view model (https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/crud?view=aspnetcore-6.0#view-model)
            var entry = _context.Add(new Student());
            entry.CurrentValues.SetValues(Student);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
