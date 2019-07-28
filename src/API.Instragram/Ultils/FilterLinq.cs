using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class FilterLinq<T>
{
    public static Expression<Func<T, Boolean>> GetWherePredicate(params SearchField[] SearchFieldList)
    {
        ParameterExpression pe = Expression.Parameter(typeof(T), typeof(T).Name);
        Expression combined = null;

        if (SearchFieldList != null)
        {
            foreach (var fieldItem in SearchFieldList)
            {
                Expression columnNameProperty = Expression.Property(pe, fieldItem.Name.Split(".")[0]);

                if (fieldItem.Name.Contains("."))
                {
                    string[] itensName = fieldItem.Name.Split(".");
                    var property1 = typeof(T).GetProperty(itensName[0]);
                    var property2 = property1.PropertyType.GetProperty(itensName[1]);
                    var inner = Expression.Property(pe, property1);
                    columnNameProperty = Expression.Property(inner, property2);
                }

                var value = Convert.ChangeType(fieldItem.Value, columnNameProperty.Type);
                Expression columnValue = Expression.Constant(value);
                Expression e1 = Expression.Equal(columnNameProperty, columnValue);
                combined = combined == null ? e1 : Expression.And(combined, e1);
            }
        }
        var predicate = PredicateBuilder.True<T>();

        return combined == null ? predicate : Expression.Lambda<Func<T, Boolean>>(combined, new ParameterExpression[] { pe });
    }


}

public class SearchField
{
    public string Name { get; set; }
    public string @Value { get; set; }
    //public string Operator { get; set; }

    public SearchField(string Name, string @Value)
    {
        this.Name = Name;
        this.@Value = @Value;
        //Operator = "=";
    }
}


public static class InputSearchFieldResolve<T>
{

    public static SearchField[] Resolve(T filter)
    {
        List<SearchField> list = new List<SearchField>();
        foreach (PropertyInfo propertyInfo in filter.GetType().GetProperties())
        {
            if (propertyInfo.GetValue(filter) != null)
            {
                if (propertyInfo.GetValue(filter).GetType().IsPrimitive)
                    list.Add(new SearchField(propertyInfo.Name, propertyInfo.GetValue(filter).ToString()));
                else
                {
                    string NameProperty = propertyInfo.Name;
                    foreach (PropertyInfo pi in propertyInfo.GetValue(filter).GetType().GetProperties())
                    {
                        if (pi.GetValue(propertyInfo.GetValue(filter)) != null)
                            list.Add(new SearchField(NameProperty + "." + pi.Name, pi.GetValue(propertyInfo.GetValue(filter)).ToString()));
                    }
                }
            }
        }

        return list.ToArray();
    }

}

/// <summary>    
/// Enables the efficient, dynamic composition of query predicates.    
/// </summary>    
public static class PredicateBuilder
{
    /// <summary>    
    /// Creates a predicate that evaluates to true.    
    /// </summary>    
    public static Expression<Func<T, bool>> True<T>() { return param => true; }

    /// <summary>    
    /// Creates a predicate that evaluates to false.    
    /// </summary>    
    public static Expression<Func<T, bool>> False<T>() { return param => false; }

    /// <summary>    
    /// Creates a predicate expression from the specified lambda expression.    
    /// </summary>    
    public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

    /// <summary>    
    /// Combines the first predicate with the second using the logical "and".    
    /// </summary>    
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.AndAlso);
    }

    /// <summary>    
    /// Combines the first predicate with the second using the logical "or".    
    /// </summary>    
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
    {
        return first.Compose(second, Expression.OrElse);
    }

    /// <summary>    
    /// Negates the predicate.    
    /// </summary>    
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        var negated = Expression.Not(expression.Body);
        return Expression.Lambda<Func<T, bool>>(negated, expression.Parameters);
    }

    /// <summary>    
    /// Combines the first expression with the second using the specified merge function.    
    /// </summary>    
    static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
    {
        // zip parameters (map from parameters of second to parameters of first)    
        var map = first.Parameters
            .Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);

        // replace parameters in the second lambda expression with the parameters in the first    
        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

        // create a merged lambda expression with parameters from the first expression    
        return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
    }

    class ParameterRebinder : ExpressionVisitor
    {
        readonly Dictionary<ParameterExpression, ParameterExpression> map;

        ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;

            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
