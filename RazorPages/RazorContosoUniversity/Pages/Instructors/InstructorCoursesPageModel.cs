#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Models.SchoolViewModels;

namespace ContosoUniversity.Pages.Instructors
{
    public class InstructorCoursesPageModel : PageModel
    {
        public List<AssignedCourseData> AssignedCourseDataList;

        public void PopulateAssignedCourseData(SchoolContext context,
            Instructor instructor)
        {
            // load all courses
            var allCourses = context.Courses;

            // make a set with courses already assigned to instructor
            var instructorCourses = new HashSet<int>(
                instructor.Courses.Select(c => c.CourseID));
                
            // build a list with course id, title and assign status
            AssignedCourseDataList = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                AssignedCourseDataList.Add(new AssignedCourseData
                {
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }
        }
    }
}