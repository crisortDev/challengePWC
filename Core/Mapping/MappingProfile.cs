using AutoMapper;
using Challenge.Core.Domain;
using Challenge.Core.Domain.Entities;
using Challenge.Core.DTOs;
using Challenge.Core.DTOs.Tasks;
using Challenge.Core.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TaskDto = Challenge.Core.DTOs.Tasks.TaskDto;
using UserDto = Challenge.Core.DTOs.Users.UserDto;


namespace Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<TaskItem, TaskDto>()
                .ForMember(d => d.AssigneeName, o => o.MapFrom(s => s.Assignee.Name));
            CreateMap<CreateTaskDto, TaskItem>();
            CreateMap<UpdateTaskDto, TaskItem>();

            CreateMap<TaskEvent, TaskEventDto>();
        }
    }
}
