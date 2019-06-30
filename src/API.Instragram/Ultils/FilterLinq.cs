using System;
using System.Linq.Expressions;

public class FilterLinq<T>
{
    public static Expression<Func<T, Boolean>> GetWherePredicate(params SearchField[] SearchFieldList)
    {

        //the 'IN' parameter for expression ie T=> condition
        ParameterExpression pe = Expression.Parameter(typeof(T), typeof(T).Name);

        //combine them with and 1=1 Like no expression
        Expression combined = null;

        if (SearchFieldList != null)
        {
            foreach (var fieldItem in SearchFieldList)
            {

                //var pe = Expression.Parameter(typeof(object1));
                //var property1 = typeof(object1).GetProperty(Name1);
                //var property2 = property1.PropertyType.GetProperty(Name2);
                //var inner = Expression.Property(pe, property1);
                //var outer = Expression.Property(inner, property2);


                if (fieldItem.Name.Contains("."))
                {
                    string[] itensName = fieldItem.Name.Split(".");
                    //Expression for accessing Fields name property
                    Expression columnNameProperty1 = Expression.Property(pe, itensName[0]);


                    var property1 = typeof(API.Instragram.Entities.Post).GetProperty(itensName[0]);
                    var property2 = property1.PropertyType.GetProperty(itensName[1]);
                    var inner = Expression.Property(pe, property1);
                    var outer = Expression.Property(inner, property2);

                    var value = Convert.ChangeType(fieldItem.Value, outer.Type);

                    //the name constant to match 
                    Expression columnValue = Expression.Constant(value);

                    //the first expression: PatientantLastName = ?
                    Expression e1 = Expression.Equal(outer, columnValue);

                    if (combined == null)
                    {
                        combined = e1;
                    }
                    else
                    {
                        combined = Expression.And(combined, e1);
                    }
                }

                else
                {
                    //Expression for accessing Fields name property
                    Expression columnNameProperty = Expression.Property(pe, fieldItem.Name);


                    var value = Convert.ChangeType(fieldItem.Value, columnNameProperty.Type);

                    //the name constant to match 
                    Expression columnValue = Expression.Constant(value);

                    //the first expression: PatientantLastName = ?
                    Expression e1 = Expression.Equal(columnNameProperty, columnValue);

                    if (combined == null)
                    {
                        combined = e1;
                    }
                    else
                    {
                        combined = Expression.And(combined, e1);
                    }
                }
            }
        }

        //create and return the predicate
        return Expression.Lambda<Func<T, Boolean>>(combined, new ParameterExpression[] { pe });
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

