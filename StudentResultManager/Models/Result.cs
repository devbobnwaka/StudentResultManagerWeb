using System.ComponentModel.DataAnnotations;

namespace StudentResultManager.Models
{
    public class Result
    {
        public int Id { get; set; }
        public string StudentId { get; set; } = null!; 
        public Guid SubjectId { get; set; }
        [Required]
        public int Mark { get; set; }
        public string Grade { get; set; } = null!;

        // Navigation properties
        public Student? Student { get; set; }
        public Subject? Subject { get; set; }
    }
}
