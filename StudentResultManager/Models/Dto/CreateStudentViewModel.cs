using System.ComponentModel.DataAnnotations;

namespace StudentResultManager.Models.Dto
{
    public class CreateStudentViewModel
    {
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        public Class? Class { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment date")]
        [Required]
        public DateTime? EnrollmentDate { get; set; }
        [Display(Name = "Contact Information")]
        public string ContactInformation { get; set; } = null!;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name ="Photo")]
        public IFormFile? PhotoPath { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
