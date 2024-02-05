using AutoMapper;
using JetstreamSkiserviceAPI.DTO;
using JetstreamSkiserviceAPI.Models;

namespace JetstreamSkiserviceAPI.Mappers
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile() 
        {

            CreateMap<Registration, RegistrationDto>();
            CreateMap<RegistrationDto, Registration>();

            CreateMap<Registration, RegistrationDto>().ReverseMap();
            CreateMap<CreateRegistrationDto, Registration>().ReverseMap();

            CreateMap<Priority, PriorityDto>();
            CreateMap<PriorityDto, Priority>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<Status, StatusDto>();
            CreateMap<StatusDto, Status>();
        }
    }
}
