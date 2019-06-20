using GraphQL.Types;

namespace API.Instragram.GraphQL.Types.Filter
{
    public class SortSettingsType : InputObjectGraphType<SortSettings>
    {
        public SortSettingsType()
        {
            Field(x => x.OrderBy);
            Field<SortDirectionEnumType>("direction");
        }
    }
    public class SortDirectionEnumType : EnumerationGraphType<SortDirection>
    {
    }
}
