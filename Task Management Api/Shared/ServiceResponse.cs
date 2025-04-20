namespace Task_Management_Api.Shared
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public State? State { get; set; }
        public String? Message  { get; set; }
    }
}
