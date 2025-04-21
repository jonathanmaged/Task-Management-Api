using Microsoft.AspNetCore.Mvc;
using Task_Management_Api.Dtos;
using Task_Management_Api.Shared;

namespace Task_Management_Api.Interfaces
{
    public interface ITaskService
    {
        public Task<ServiceResponse<TaskDto>> CreateTask(TaskDto Dto);
        public Task<ServiceResponse<TaskDto>> GetTaskById(int id);
        public Task<ServiceResponse<List<TaskDto>>> GetTasksByUser(int id);
    }
}
