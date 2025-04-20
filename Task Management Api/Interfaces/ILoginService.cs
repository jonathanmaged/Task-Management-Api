using Task_Management_Api.Models;

namespace Task_Management_Api.Interfaces
{
    public interface ILoginService
    {
        public string Login(LoginModel login);

    }
}
