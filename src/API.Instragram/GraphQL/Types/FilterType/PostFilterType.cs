using API.Instragram.Filter;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Types.FilterType
{
    public class PostFilterType : InputObjectGraphType<PostFilter>
    {
        public PostFilterType()
        {

            Field(x => x.Id, nullable :true);
            Field(x => x.Title, nullable: true);
            Field(x => x.Description, nullable: true);
            Field<AuthorFilterType>("author");

        }
    }

    public class AuthorFilterType : InputObjectGraphType<AuthorFilter>
    {
        public AuthorFilterType()
        {

            Field(x => x.Id, nullable: true);
            Field(x => x.Name, nullable: true);
           
        }
    }
}
