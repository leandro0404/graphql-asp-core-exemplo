using API.Instragram.Entities;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Types
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Field(x=>x.Id);
            Field(x => x.Name);

        }
    }
}
