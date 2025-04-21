using Microsoft.AspNetCore.Mvc;
using Task_Management_Api.Dtos;
using Task_Management_Api.Shared;

namespace Task_Management_Api.Interfaces
{
    public interface ITaskService
    {
        public Task<ServiceResponse<TaskDto>> CreateTaskAsync(TaskDto Dto);
        public Task<ServiceResponse<TaskDto>> GetTaskByIdAsync(int id);
        public Task<ServiceResponse<List<TaskDto>>> GetTasksByUserAsync(int id);
    }
}
