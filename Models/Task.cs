namespace TaskLabBackend.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string TaskTitle { get; set; }

        public string TaskDescription { get; set; }

        public DateOnly TaskDueDate { get; set; }

        public string TaskStatus { get; set; }

        public string TaskRemarks { get; set; }

        public DateTime CreatedOn { get; set; }


    }

  
}
