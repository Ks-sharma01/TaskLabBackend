using System.ComponentModel.DataAnnotations;

namespace TaskLabBackend.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TaskTitle { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [Required]
        public DateOnly TaskDueDate { get; set; }

        [Required]
        public string TaskStatus { get; set; }

        public string? TaskRemarks { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;


    }

  
}
