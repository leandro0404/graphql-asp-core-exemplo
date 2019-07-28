using API.Instragram.Entities;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Types
{
    public class CommentType : ObjectGraphType<Comment>
    {
        public const string Name = "comments";
        public CommentType()
        {           
            Field(x=>x.Id);
            Field(x => x.Text);
            Field(x => x.Created);
            Field<AuthorType>(typeof(Author).Name);

        }
    }
}
