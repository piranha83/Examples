using System;
using System.Reflection;
using AutoMapper;

namespace App.Maps
{
    public static class EntityExtensions
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression,
            Func<PropertyInfo, bool> specification)
        {
            var sourceType = typeof(TSource);
            foreach (var property in sourceType.GetProperties())
            {
                if(specification(property))
                    expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }
    }
}