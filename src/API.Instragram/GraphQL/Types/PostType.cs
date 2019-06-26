using API.Instragram.Entities;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Types
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType()
        {
            Field(x => x.Id);
            Field(x => x.Title);
            Field(x => x.Description);
            Field(x => x.Created);
            Field(x => x.Likes);
            Field<AuthorType>(typeof(Author).Name);
            Field<ListGraphType<CommentType>>(typeof(Comment).Name);
        }
    }
}
