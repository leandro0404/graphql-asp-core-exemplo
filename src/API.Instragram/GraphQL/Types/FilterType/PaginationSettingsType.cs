using GraphQL.Types;

namespace API.Instragram.GraphQL.Types.FilterType
{
    public class PaginationSettingsType : InputObjectGraphType<PaginationSettings>
    {
        public PaginationSettingsType()
        {

            Field(x => x.PageIndex, nullable: true);
            Field(x => x.PageSize, nullable: true);
            Field<SortSettingsType>(typeof(SortSettings).Name);
        }
    }

}
