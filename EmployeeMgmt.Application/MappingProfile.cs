using AutoMapper;
using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeMgmt.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
            .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => src.EmployeeCode.Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => new EmployeeCode(src.EmployeeCode)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new EmailAddress(src.Email)));
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
        }
    }
}
