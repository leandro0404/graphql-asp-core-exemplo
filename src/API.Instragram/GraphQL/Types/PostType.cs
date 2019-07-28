using API.Instragram.Entities;
using API.Instragram.Filter;
using API.Instragram.GraphQL.Types.FilterType;
using API.Instragram.Repository;
using GraphQL.DataLoader;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace API.Instragram.GraphQL.Types
{
    public class PostType : ObjectGraphType<Post>
    {
        public PostType(IDataLoaderContextAccessor accessor, IPostRepository repository)
        {
            Field(x => x.Id);
            Field(x => x.Title);
            Field(x => x.Description);
            Field(x => x.Created);
            Field(x => x.Likes);
            Field<AuthorType>(typeof(Author).Name);
            Field<ListGraphType<CommentType>, IEnumerable<Comment>>()
              .Name("Comment")
              .Argument<PaginationSettingsType>("pageSettings", "pageSettings")
              .ResolveAsync(context =>
              {
                  var pageSettings = context.GetArgument<PaginationSettings>("pageSettings", new PaginationSettings());
                  var loader = accessor.Context.GetOrAddCollectionBatchLoader<int, Comment>("GetCommentsByIdAsync", (id) => repository.GetCommentsByIdAsync(id, pageSettings));
                  return loader.LoadAsync(context.Source.Id);

              });
        }
    }
}
