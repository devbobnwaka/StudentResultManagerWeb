using Microsoft.AspNetCore.Identity;
using StudentResultManager.Models;
using System.ComponentModel.DataAnnotations;

namespace StudentResultManager.Data
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string? FirstName { get; set; } 
        [Required]
        public string? LastName { get; set; } 
        public Student? Student { get; set; }

    }
}
