using API.Instragram.GraphQL.Mutation;
using API.Instragram.GraphQL.Queries;
using GraphQL;
using GraphQL.Types;

namespace API.Instragram.GraphQL.Schemas
{
    public class PostSchema : Schema
    {
        public PostSchema(IDependencyResolver resolver)
        : base(resolver)
        {
            Query = resolver.Resolve<PostQuery>();
            Mutation = resolver.Resolve<PostMutation>();

        }
    }
}
