using AutoMapper;
using Basic.Application.DTOs;
using Basic.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Basic.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmployeeVm, Employee>().ReverseMap();
            CreateMap<CompanyVm, Company>().ReverseMap();
        }
    }
}
