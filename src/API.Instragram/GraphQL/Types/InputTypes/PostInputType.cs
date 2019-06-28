using API.Instragram.Entities;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Types
{
    public class PostInputType : InputObjectGraphType<Post>
    {
        public PostInputType()
        {

            Field(x => x.Title);
            Field(x => x.Description);
            Field(x => x.AuthorId);

        }
    }
}


