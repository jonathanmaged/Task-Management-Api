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
        public async Task<ServiceResponse<TaskDto>> CreateTaskAsync(TaskDto Dto)
        {
            try
            {
                //check the usersid list if any userid doesnt exist
                var response = await CheckUsersExistenceAsync(Dto);
                if (response.State == State.NotFound) {
                    return response;
                }

                // map TaskDto comes from User into Task Element to save in the Databsae
                var task = _mapper.Map<Models.Task>(Dto);

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return response;
            }
            catch (Exception ex)
            {
                //Handle if the Database server crashes or the data didnt saved
                
                return new ServiceResponse<TaskDto> { State = State.ServerError};
            }

        }

        public async Task<ServiceResponse<TaskDto>> GetTaskByIdAsync(int id)
        {
            TaskDto taskdto;

            var task = await _context.Tasks
                .Include(t => t.TaskComments)
                .FirstOrDefaultAsync(t => t.Id == id);

            //check if no task exist for the given Id 
            if (task == null){
                return new ServiceResponse<TaskDto> { State = State.NotFound };
            }
            //map Task comes from the database to the TaskDto returned to the user
            taskdto = _mapper.Map<TaskDto>(task);
  
            return new ServiceResponse<TaskDto> { Data = taskdto, State = State.Success };

        }

        public async Task<ServiceResponse<List<TaskDto>>> GetTasksByUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            //check if no user exist with that id
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
            //get the assigned task to that user

            var tasklist = await taskcomments
               .Include(tc => tc.Task)
               .ThenInclude(t => t.TaskComments)
               .Select(tc => tc.Task)
               .ToListAsync();

            var taskdtolist = _mapper.Map<List<TaskDto>>(tasklist);

            return new ServiceResponse<List<TaskDto>>
            {
                Data = taskdtolist,
                State = State.Success,              
            };

        }
        private async Task<ServiceResponse<TaskDto>> CheckUsersExistenceAsync(TaskDto dto) {
            if (dto.UsersId != null) {
                foreach (var userid in dto.UsersId)
                {
                    if (!await _context.Users.AnyAsync(u => u.Id == userid))
                    {
                        return new ServiceResponse<TaskDto>
                        {
                            Message = $"user with id {userid} doesnt exist",
                            State = State.NotFound
                        };
                    }
                }
            }

            return new ServiceResponse<TaskDto> { State = State.Success} ;

        }
    
    }
}
