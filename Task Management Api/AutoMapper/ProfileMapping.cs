using AutoMapper;
using Task_Management_Api.Dtos;
using Task_Management_Api.Models;
using Task = Task_Management_Api.Models.Task;

namespace Task_Management_Api.AutoMapper
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<TaskDto, Task>().ForMember(dest => dest.TaskComments,
                opt => opt.MapFrom(src => 
                src.UsersId!=null ?
                src.UsersId.Select(id => new TaskComment {UserId=id }).ToList()
                :new List<TaskComment> { }
                ));
                
            CreateMap<Task, TaskDto>().ForMember(dest => dest.UsersId,
                    opt => opt.MapFrom(src => 
                    src.TaskComments != null?
                    src.TaskComments.Select(tc => tc.UserId).ToList()
                    :null
                    ));


        }
    }
}
