namespace TaskLabBackend.Dto
{
    public class UpdateTaskDto
    {
        public string TaskTitle { get; set; }

        public string TaskDescription { get; set; }

        public DateOnly TaskDueDate { get; set; }

        public string TaskStatus { get; set; }

        public string TaskRemarks { get; set; }
    }
}
