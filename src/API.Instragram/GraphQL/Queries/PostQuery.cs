using API.Instragram.GraphQL.Types;
using GraphQL.Types;
using System.Linq;
using API.Instragram.GraphQL.Types.FilterType;
using API.Instragram.Repository;
using API.Instragram.Filter;


namespace API.Instragram.GraphQL.Queries
{
    public class PostQuery : ObjectGraphType
    {
        public PostQuery(IPostRepository repository)
        {
            Field<ListGraphType<PostType>>(
               "post",
               arguments: new QueryArguments(new QueryArgument<PostFilterType> { Name = "filter" }, new QueryArgument<PaginationSettingsType> { Name = "pageSettings" }),
               resolve: context =>
               {
                   var postFilter = context.GetArgument<PostFilter>("filter", new PostFilter());
                   var pageSettings = context.GetArgument<PaginationSettings>("pageSettings", new PaginationSettings());

                   var input = InputSearchFieldResolve<PostFilter>.Resolve(postFilter);
                   var query = FilterLinq<API.Instragram.Entities.Post>
                   .GetWherePredicate(input);

                   if(query != null)
                   return repository.Get().Where(query).Page(pageSettings);
                   else
                       return repository.Get().Page(pageSettings).ToList();

               }
           );
        }


    }
}
