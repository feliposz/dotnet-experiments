using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityReverse.Models
{
    public partial class AspNetUserToken
    {
        [Key]
        public string UserId { get; set; } = null!;
        [Key]
        [StringLength(128)]
        public string LoginProvider { get; set; } = null!;
        [Key]
        [StringLength(128)]
        public string Name { get; set; } = null!;
        public string? Value { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(AspNetUser.AspNetUserTokens))]
        public virtual AspNetUser User { get; set; } = null!;
    }
}
