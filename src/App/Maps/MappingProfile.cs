using System;
using System.Reflection;
using App.Models;
using AutoMapper;

namespace App.Maps
{
    public class MappingProfile : Profile
    {
        static Func<PropertyInfo, bool> entity = (p) => p.Name.ToUpper().EndsWith("ID");
        public MappingProfile()
        {            
            CreateMap<Department, Department>()
                //.ReverseMap()
                .Ignore(entity);

            CreateMap<Employee, Employee>()
                .Ignore(entity);
        }
    }
}