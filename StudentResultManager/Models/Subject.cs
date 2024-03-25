using System.ComponentModel.DataAnnotations;

namespace StudentResultManager.Models
{
    public class Subject
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public ICollection<Result>? Results { get; set; }

    }
}
