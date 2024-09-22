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
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => new DepartmentDTO
             {
                 DepartmentId = src.Department.DepartmentId,
                 DepartmentName = src.Department.DepartmentName,
                 Description = src.Department.Description
             }));
            CreateMap<EmployeeDTO, Employee>()
                .ForMember(dest => dest.EmployeeCode, opt => opt.MapFrom(src => new EmployeeCode(src.EmployeeCode)))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new EmailAddress(src.Email)))
                .ForMember(dest => dest.Department, opt => opt.Ignore());

            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
        }
    }
}
