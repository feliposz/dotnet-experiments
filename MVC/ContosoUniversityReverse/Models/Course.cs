#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    [Table("Course")]
    [Index(nameof(DepartmentId), Name = "IX_Course_DepartmentID")]
    public partial class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
            Instructors = new HashSet<Instructor>();
        }

        [Key]
        [Column("CourseID")]
        [Display(Name = "Number")]
        public int CourseId { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string? Title { get; set; }
        public int Credits { get; set; }

        [Display(Name = "Department")]
        [Column("DepartmentID")]
        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        [InverseProperty("Courses")]
        public virtual Department Department { get; set; } = null!;
        [InverseProperty(nameof(Enrollment.Course))]
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty(nameof(Instructor.Courses))]
        public virtual ICollection<Instructor> Instructors { get; set; }
    }
}
