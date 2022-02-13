#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    [Table("OfficeAssignment")]
    public partial class OfficeAssignment
    {
        [Key]
        [Column("InstructorID")]
        public int InstructorId { get; set; }
        [StringLength(50)]
        public string Location { get; set; }

        [ForeignKey(nameof(InstructorId))]
        [InverseProperty("OfficeAssignment")]
        public virtual Instructor Instructor { get; set; } = null!;
    }
}
