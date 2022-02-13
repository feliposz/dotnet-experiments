using ContosoUniversityReverse.Models;

namespace ContosoUniversityReverse.Data;

public static class DbInitializer
{

    public static void Initialize(ApplicationDbContext context)
    {
        RandomDataGenerator rnd = new RandomDataGenerator();

        List<Student> students = GetStudents(rnd, 100);
        context.AddRange(students);

        List<Instructor> instructors = GetInstructors(rnd, 40);
        context.AddRange(instructors);

        List<Department> departments = GetDepartments(rnd, instructors);
        context.AddRange(departments);

        List<Course> courses = GetCourses(rnd, context, departments, 40);
        context.AddRange(courses);

        List<Enrollment> enrollments = GetEnrollments(rnd, students, courses, 200);
        context.AddRange(enrollments);

        AssignInstructorsToCourses(rnd, instructors, courses);

        context.SaveChanges();
    }

    private static void AssignInstructorsToCourses(RandomDataGenerator rnd, List<Instructor> instructors, List<Course> courses)
    {
        for (int i = 0; i < 100; i++)
        {
            var course = rnd.Pick(courses);
            var instructor = rnd.Pick(instructors);
            course.Instructors.Add(instructor);
        }
    }

    private static List<Enrollment> GetEnrollments(RandomDataGenerator rnd, List<Student> students, List<Course> courses, int v)
    {
        var grades = Enum.GetValues<Grade>().ToList();
        var enrollments = new List<Enrollment>();
        for (int i = 0; i < v; i++)
        {
            enrollments.Add(new Enrollment
            {
                Student = rnd.Pick(students),
                Course = rnd.Pick(courses),
                Grade = rnd.Range(1, 10) > 3 ? rnd.Pick(grades) : null
            });
        }
        return enrollments;
    }

    private static List<Course> GetCourses(RandomDataGenerator rnd, ApplicationDbContext context, List<Department> departments, int v)
    {
        var courses = new List<Course>();
        for (int i = 0; i < v; i++)
        {
            var department = rnd.Pick(departments);
            string[] prefix = new string[] { "Basics of", "Theory of", "History of", "Introduction to", "Advanced topics on", "Rethinking", "A new approch to", "Modern", "Classic" };

            // limit length of title
            string title = String.Format("{0} {1}", rnd.Pick(prefix), department.Name);
            if (title.Length > 50)
            {
                title = title.Substring(0, 50);
            }

            // find available id
            int courseNumber = rnd.Range(1000, 9999);
            while (context.Courses.Any(x => x.CourseId == courseNumber))
            {
                courseNumber = rnd.Range(1000, 9999);
            }

            courses.Add(new Course
            {
                CourseId = courseNumber,
                Title = title,
                Credits = rnd.Range(1, 9),
                Department = department
            });
        }
        return courses;
    }

    private static List<Department> GetDepartments(RandomDataGenerator rnd, List<Instructor> instructors)
    {
        var departments = new List<Department>();
        var departmentNames = new string[] { "Aeronautics & Astronautics", "Anesthesia", "Anthropology", "Applied Physics", "Art & Art History", "Biochemistry", "Bioengineering", "Biology", "Biology, Developmental", "Biomedical Informatics", "Business, Graduate School of", "Cardiothoracic Surgery", "Chemical and Systems Biology", "Chemical Engineering", "Chemistry", "Civil & Environmental Engineering", "Classics", "Communication", "Comparative Literature", "Comparative Medicine", "Computer Science", "Dermatology", "Developmental Biology", "Earth System Science", "Earth, Energy & Environmental Sciences, School of", "East Asian Languages and Cultures", "Economics", "Education, Graduate School of", "Electrical Engineering", "Energy Resources Engineering", "Engineering, School of", "English", "French and Italian", "Genetics", "Geological Sciences", "Geophysics", "German Studies", "Health Research & Policy", "History", "Humanities & Sciences, School of", "Iberian & Latin American Cultures", "Law School", "Linguistics", "Management Science & Engineering", "Materials Science & Engineering", "Mathematics", "Mechanical Engineering", "Medicine", "Medicine,  School of", "Microbiology & Immunology", "Molecular & Cellular Physiology", "Music", "Neurobiology", "Neurology & Neurological Sciences", "Neurosurgery", "Obstetrics and Gynecology", "Ophthalmology", "Orthopaedic Surgery", "Otolaryngology (Head and Neck Surgery)", "Pathology", "Pediatrics", "Philosophy", "Photon Science (SLAC)", "Physics", "Political Science", "Psychiatry and Behavioral Sciences", "Psychology", "Radiation Oncology", "Radiology", "Religious Studies", "Slavic Languages and Literature", "Sociology", "Statistics", "Structural Biology", "Surgery", "Theater and Performance Studies", "Urology" };
        foreach (var name in departmentNames)
        {
            departments.Add(new Department
            {
                Name = name,
                Budget = rnd.Range(10, 100) * 1000,
                StartDate = rnd.Date(),
                Instructor = rnd.Range(1, 10) > 2 ? rnd.Pick(instructors) : null
            });
        }
        return departments;
    }

    private static List<Instructor> GetInstructors(RandomDataGenerator rnd, int v)
    {
        var instructors = new List<Instructor>();
        for (int i = 0; i < v; i++)
        {
            instructors.Add(new Instructor()
            {
                LastName = rnd.LastName(),
                FirstName = rnd.FirstName(),
                HireDate = rnd.Date()
            });
        }
        return instructors;
    }

    private static List<Student> GetStudents(RandomDataGenerator rnd, int v)
    {
        var students = new List<Student>();
        for (int i = 0; i < v; i++)
        {
            students.Add(new Student()
            {
                LastName = rnd.LastName(),
                FirstName = rnd.FirstName(),
                EnrollmentDate = rnd.Date()
            });
        }
        return students;
    }


}