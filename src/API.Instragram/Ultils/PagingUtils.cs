using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class PaginationSettings
{
    public PaginationSettings()
    {
        PageIndex = 0;
        PageSize = 15;
        SortSettings = new SortSettings { OrderBy = "", Direction = SortDirection.Asc };
    }
    public int PageSize { get; set; }
    public int PageIndex { get; set; }
    public SortSettings SortSettings { get; set; }
}

public class SortSettings
{
    public string OrderBy { get; set; }
    public SortDirection? Direction { get; set; }

    public SortSettings()
    {
        Direction = SortDirection.Asc;
    }
}

public enum SortDirection
{
    Asc,
    Desc
}

public static class PagingUtils
{
    public static IQueryable<TSource> Page<TSource>(this IQueryable<TSource> en, PaginationSettings pageSettings)
    {

        return en.OrderByCustom(pageSettings.SortSettings.OrderBy, pageSettings.SortSettings)
            .Take(pageSettings.PageSize)
            .Skip(pageSettings.PageIndex * pageSettings.PageSize);
    }

    public static IQueryable<TEntity> OrderByCustom<TEntity>(this IQueryable<TEntity> source, string orderByProperty,SortSettings sortSettings)
    {
        string command = sortSettings.Direction.Value != 0 ? "OrderByDescending" : "OrderBy";
        var type = typeof(TEntity);
        var property = type.GetProperty(orderByProperty,    BindingFlags.SetProperty |
                                                            BindingFlags.IgnoreCase |
                                                            BindingFlags.Public |
                                                            BindingFlags.Instance);
        if (property == null)
            property = type.GetProperties()[0];

        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                      source.Expression, Expression.Quote(orderByExpression));
        return source.Provider.CreateQuery<TEntity>(resultExpression);
    }

}



