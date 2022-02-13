#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    [Table("Department")]
    [Index(nameof(InstructorId), Name = "IX_Department_InstructorID")]
    public partial class Department
    {
        public Department()
        {
            Courses = new HashSet<Course>();
        }

        [Key]
        [Column("DepartmentID")]
        public int DepartmentId { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Budget { get; set; }
        [Display(Name="Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Column("InstructorID")]
        [Display(Name="Administrator")]
        public int? InstructorId { get; set; }
        public byte[]? RowVersion { get; set; }

        [ForeignKey(nameof(InstructorId))]
        [InverseProperty("Departments")]
        public virtual Instructor? Instructor { get; set; }
        [InverseProperty(nameof(Course.Department))]
        public virtual ICollection<Course> Courses { get; set; }
    }
}
