using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Management_Api.Data;
using Task_Management_Api.Dtos;
using Task_Management_Api.Interfaces;
using Task_Management_Api.Models;
using Task_Management_Api.Shared;

namespace Task_Management_Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TaskService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<State> CreateTask(TaskDto Dto)
        {
            try
            {
                // Fill Name and Discription for Task
                var task = _mapper.Map<Models.Task>(Dto);

                if (Dto.UsersId != null)
                {
                    task.TaskComments = [];
                    foreach (var UserId in Dto.UsersId)
                    {
                        task.TaskComments.Add(
                            new TaskComment { UserId = UserId }
                        );

                    }
                }

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return State.Success;
            }
            catch (Exception ex)
            {
                return State.ServerError;
            }

        }


        public async Task<ServiceResponse<TaskDto>> GetTaskById(int id)
        {
            TaskDto taskdto;

            var task = await _context.Tasks.Include(t => t.TaskComments).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return new ServiceResponse<TaskDto> { State = State.NotFound };
            }
            else
            {
                taskdto = _mapper.Map<TaskDto>(task);

                if (task.TaskComments == null)
                {
                    return new ServiceResponse<TaskDto> { Data = taskdto, State = State.Success };
                }

            }
            return new ServiceResponse<TaskDto> { Data = taskdto, State = State.Success };

        }

        public async Task<ServiceResponse<List<TaskDto>>> GetTasksByUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            //check if no user with that id
            if (user == null)
            {
                return new ServiceResponse<List<TaskDto>>
                {
                    State = State.NotFound,
                    Message = "User Not Found"
                };
            }

            var taskcomments = _context.TaskComments.Where(tc => tc.UserId == id);

            //check if there is no task assigned to that user
            if (! await taskcomments.AnyAsync())
            {
                return new ServiceResponse<List<TaskDto>>
                {
                    State = State.NotFound,
                    Message = "NO Task Found For That User"
                };
            }
            var tasklist = await taskcomments
                .Include(tc => tc.Task)
                .Select(t => t.Task)
                .ToListAsync();

            var taskdtolist = _mapper.Map<List<TaskDto>>(tasklist);

            return new ServiceResponse<List<TaskDto>>
            {
                Data = taskdtolist,
                State = State.Success,              
            };

        }
    }
}
