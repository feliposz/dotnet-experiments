using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    public enum Grade { A, B, C, D, F };

    [Table("Enrollment")]
    [Index(nameof(CourseId), Name = "IX_Enrollment_CourseID")]
    [Index(nameof(StudentId), Name = "IX_Enrollment_StudentID")]
    public partial class Enrollment
    {
        [Key]
        [Column("EnrollmentID")]
        public int EnrollmentId { get; set; }
        [Column("CourseID")]
        public int CourseId { get; set; }
        [Column("StudentID")]
        public int StudentId { get; set; }

        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        [ForeignKey(nameof(CourseId))]
        [InverseProperty("Enrollments")]
        public virtual Course Course { get; set; } = null!;
        [ForeignKey(nameof(StudentId))]
        [InverseProperty("Enrollments")]
        public virtual Student Student { get; set; } = null!;
    }
}
