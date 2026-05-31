using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime? TaskDueDate { get; set; }

        [Required]
        public string TaskStatus { get; set; }

        public string? TaskRemarks { get; set; }

       public int TeamMemberId { get; set; }

        [ForeignKey("TeamMemberId")]
        public TeamMember TeamMember { get; set; }

        public DateTime CreatedOn { get; set; }

    }

  
}
