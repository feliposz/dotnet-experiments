#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;

namespace ContosoUniversity.Pages.Courses
{
    public class DepartmentNamePageModel : PageModel
    {
        public SelectList DepartmentNameSL { get; set; }

        public void PopulateDepartmentDropDownList(SchoolContext _context,
            object selectedDepartment = null)
        {
            var departmentQuery = from d in _context.Departments
                                  orderby d.Name
                                  select d;

            DepartmentNameSL = new SelectList(departmentQuery.AsNoTracking(),
                "DepartmentID", "Name", selectedDepartment);
        }
    }
}