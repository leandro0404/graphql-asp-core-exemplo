using API.Instragram.Entities;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Types
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType()
        {
            Name = "post";
            Field(x => x.Id);
            Field(x => x.Title);
            Field(x => x.Description);
            Field(x => x.Created);
            Field(x => x.Likes);
            Field<AuthorType>(AuthorType.Name);
            Field<ListGraphType<CommentType>>(CommentType.Name);
        }
    }
}
