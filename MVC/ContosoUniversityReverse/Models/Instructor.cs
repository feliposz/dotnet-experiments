using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    [Table("Instructor")]
    public partial class Instructor : Person
    {
        public Instructor()
        {
            Departments = new HashSet<Department>();
            Courses = new HashSet<Course>();
        }

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        // [Key]
        // [Column("ID")]
        // public int Id { get; set; }

        // [ForeignKey(nameof(Id))]
        // [InverseProperty(nameof(Person.Instructor))]
        // public virtual Person IdNavigation { get; set; } = null!;
        [InverseProperty("Instructor")]
        public virtual OfficeAssignment OfficeAssignment { get; set; } = null!;
        [InverseProperty(nameof(Department.Instructor))]
        public virtual ICollection<Department> Departments { get; set; }

        [ForeignKey("InstructorId")]
        [InverseProperty(nameof(Course.Instructors))]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
