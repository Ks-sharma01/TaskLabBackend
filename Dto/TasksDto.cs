namespace TaskLabBackend.Dto
{
    public class TasksDto
    {
        public string TaskTitle { get; set; }

        public string TaskDescription { get; set; }

        public DateTime? TaskDueDate { get; set; }

        public string TaskStatus { get; set; }

        public string TaskRemarks { get; set; }

    }
}
