using API.Instragram.GraphQL.Types;
using GraphQL.Types;
using System.Linq;
using API.Instragram.GraphQL.Types.Filter;
using API.Instragram.Repository;

namespace API.Instragram.GraphQL.Queries
{
    public class PostQuery : ObjectGraphType
    {
        public PostQuery(IPostRepository repository)
        {
            Field<ListGraphType<PostType>>(
               "post",
               arguments: new QueryArguments(new QueryArgument<PaginationSettingsType> { Name = "pageSettings" }),
               resolve: context =>
               {
                   var pageSettings = context.GetArgument<PaginationSettings>("pageSettings", new PaginationSettings());
                   return repository.Get().AsQueryable().Page(pageSettings).ToList();
               }
           );
        }


    }
}
