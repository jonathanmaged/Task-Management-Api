namespace Task_Management_Api.Dtos
{
    public class TaskDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int>? UsersId { get; set; }
    }
}
