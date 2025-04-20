namespace Task_Management_Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskComment>? TaskComments { get; set; }

    }
}
