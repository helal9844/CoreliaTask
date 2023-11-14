using AutoMapper;
using BL.DTOs;
using CoreliaTask.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
        }
    }
}
