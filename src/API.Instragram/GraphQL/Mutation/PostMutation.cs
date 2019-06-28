using API.Instragram.Entities;
using API.Instragram.GraphQL.Types;
using API.Instragram.Repository;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Mutation
{
    public class PostMutation : ObjectGraphType
    {
        public PostMutation(IPostRepository repository)
        {
            Field<PostType>(
                "createPost",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PostInputType>> { Name = "postInput" }
                ),
                resolve: context =>
                {
                    var post = context.GetArgument<Post>("postInput");
                    return repository.Created(post);
                });
        }
    }
}
