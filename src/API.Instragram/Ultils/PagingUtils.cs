using System.Linq;
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
}

public enum SortDirection
{
    Asc,
    Desc
}

public static class PagingUtils
{
    public static IQueryable<T> Page<T>(this IQueryable<T> en, PaginationSettings pageSettings)
    {
        return en.OrderByCustom(pageSettings.SortSettings).Skip(pageSettings.PageIndex * pageSettings.PageSize).Take(pageSettings.PageSize);
    }
    public static IQueryable<T> OrderByCustom<T>(this IQueryable<T> en, SortSettings sortSettings)
    {
        return sortSettings.Direction.Value == 0 ? en.OrderBy(x => GetPropertyValue(x, sortSettings.OrderBy)) : en.OrderByDescending(x => GetPropertyValue(x, sortSettings.OrderBy));
    }
    private static object GetPropertyValue(object obj, string propertyName)
    {

        System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName,
                                                                                BindingFlags.SetProperty |
                                                                                BindingFlags.IgnoreCase |
                                                                                BindingFlags.Public |
                                                                                BindingFlags.Instance);


        return (propertyInfo == null || (!propertyInfo.PropertyType.IsPrimitive)) ? null : propertyInfo.GetValue(obj, null);
    }
}






