using Microsoft.AspNetCore.Identity;
using StudentResultManager.Data;
using System.ComponentModel.DataAnnotations;

namespace StudentResultManager.Models
{
    public enum Class
    {
        None,
        JSS1,
        JSS2,
        JSS3,
        SSS1,
        SSS2,
        SSS3
    }
    public class Student
    {
        public string Id { get; set; }
        [Required]
        public Class? Class { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment date")]
        public DateTime? EnrollmentDate { get; set; }
        public string ContactInformation { get; set; } = null!;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        public string? PhotoPath { get; set; }
        // Navigation property to ApplicationUser
        public string ApplicationUserId { get; set; }  // Required foreign key property
        public ApplicationUser ApplicationUser { get; set; } // Required reference navigation to principal
        public ICollection<Result>? Results { get; set; }
        
    }
}
