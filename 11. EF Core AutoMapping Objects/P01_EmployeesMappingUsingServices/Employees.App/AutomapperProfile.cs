namespace Employees.App
{
    using AutoMapper;
    using Employees.DtoModels;
    using Employees.Models;

    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<Employee, PersonalInfoDto>();
            CreateMap<PersonalInfoDto, Employee>();

            CreateMap<Employee, ManagerDto>();

            CreateMap<Employee, EmployeeByAgeDto>();
        }
    }
}
