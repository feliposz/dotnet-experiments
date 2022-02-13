using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    [Table("Person")]
    public partial class Person
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Full Name")]
        public string FullName { get { return LastName + ", " + FirstName; } }

        // [InverseProperty("IdNavigation")]
        // public virtual Instructor Instructor { get; set; } = null!;
        // [InverseProperty("IdNavigation")]
        // public virtual Student Student { get; set; } = null!;
    }
}
