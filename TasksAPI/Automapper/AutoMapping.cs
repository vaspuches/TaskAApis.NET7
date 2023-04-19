using AutoMapper;
using Tasks.Domain.Entities;
using Tasks.Infrastructure.Data.Model;
using Tasks.Web.Dto;

namespace Tasks.Web.Api.Automapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ToDoTaskDto, ToDoTaskEntity>().ReverseMap();
            CreateMap<ToDoTaskEntity, ToDoTask>().ReverseMap();                        
        }
    }
}