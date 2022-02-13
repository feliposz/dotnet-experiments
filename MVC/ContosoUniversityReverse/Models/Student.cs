#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    [Table("Student")]
    public partial class Student : Person
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        // [Key]
        // [Column("ID")]
        // public int Id { get; set; }

        // [ForeignKey(nameof(Id))]
        // [InverseProperty(nameof(Person.Student))]
        // public virtual Person IdNavigation { get; set; } = null!;
        [InverseProperty(nameof(Enrollment.Student))]
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
