using System;
using System.Linq.Expressions;
using AutoMapper;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs
{
    public static class Extensions
    {
        public static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>>         selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
