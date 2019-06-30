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
        return combined == null ? null : Expression.Lambda<Func<T, Boolean>>(combined, new ParameterExpression[] { pe });
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
