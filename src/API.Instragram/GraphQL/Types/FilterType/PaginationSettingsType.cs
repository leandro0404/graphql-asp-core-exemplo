using GraphQL.Types;

namespace API.Instragram.GraphQL.Types.Filter
{
    public class PaginationSettingsType : InputObjectGraphType<PaginationSettings>
    {
        public PaginationSettingsType()
        {

            Field(x => x.PageIndex);
            Field(x => x.PageSize);
            Field<SortSettingsType>(typeof(SortSettings).Name);
        }
    }

}
