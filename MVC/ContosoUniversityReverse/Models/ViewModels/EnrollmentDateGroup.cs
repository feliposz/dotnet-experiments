using System.ComponentModel.DataAnnotations;

namespace ContosoUniversityReverse.Models.ViewModels;

public class EnrollmentDateGroup
{
    public int Year { get; set; }
    public int StudentCount { get; set; }
}