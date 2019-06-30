using API.Instragram.Entities;
using API.Instragram.GraphQL.Types.FilterType;
using API.Instragram.Repository;
using GraphQL.Types;
using System.Linq;

namespace API.Instragram.GraphQL.Types
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType(IPostRepository repository)
        {
            Field(x => x.Id);
            Field(x => x.Title);
            Field(x => x.Description);
            Field(x => x.Created);
            Field(x => x.Likes);
            Field<AuthorType>(typeof(Author).Name);
            Field<ListGraphType<CommentType>>(
              typeof(Comment).Name,
              arguments: new QueryArguments(new QueryArgument<PaginationSettingsType> { Name = "pageSettings" }),
              resolve: context =>
              {
                  var postId = context.Source.Id;
                  var pageSettings = context.GetArgument<PaginationSettings>("pageSettings", new PaginationSettings());
                  return repository.GetComments(postId).AsQueryable().Page(pageSettings).ToList();
              }
          );
        }
    }
}
