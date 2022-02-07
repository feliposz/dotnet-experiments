#nullable disable
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ContosoUniversityMvc.Models;

namespace ContosoUniversityMvc.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<OfficeAssignment> OfficeAssigments { get; set; }
    public DbSet<CourseAssignment> CourseAssignments { get; set; }
    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Course>().ToTable(nameof(Course));
        modelBuilder.Entity<Student>().ToTable(nameof(Student));
        modelBuilder.Entity<Enrollment>().ToTable(nameof(Enrollment));
        modelBuilder.Entity<Instructor>().ToTable(nameof(Instructor));
        modelBuilder.Entity<Department>().ToTable(nameof(Department));
        modelBuilder.Entity<OfficeAssignment>().ToTable(nameof(OfficeAssignment));
        modelBuilder.Entity<CourseAssignment>().ToTable(nameof(CourseAssignment));
        modelBuilder.Entity<Person>().ToTable(nameof(Person));

        // composite primary key has to be defined by using fluent API
        modelBuilder.Entity<CourseAssignment>()
            .HasKey(c => new { c.CourseID, c.InstructorID });
    }
}
