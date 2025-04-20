using AutoMapper;
using Task_Management_Api.Dtos;
using Task = Task_Management_Api.Models.Task;

namespace Task_Management_Api.AutoMapper
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<TaskDto,Task>();
            CreateMap<Task, TaskDto>()
                .ForMember(dest => dest.UsersId,
                    opt => opt.MapFrom(src => src.TaskComments.Select(tc => tc.UserId).ToList()));

        }
    }
}
