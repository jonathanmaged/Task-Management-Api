namespace Task_Management_Api.Models
{
    public class TaskComment
    {
        public int TaskId { get; set; }
        public Task Task { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
