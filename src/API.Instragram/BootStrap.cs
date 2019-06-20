using API.Instragram.GraphQL.Schemas;
using API.Instragram.Repository;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Instragram
{
    public static class ServicesGraphQl
    {
        public static void AddGraphQl(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<PostSchema>();

            services.AddGraphQL(o => { o.ExposeExceptions = false; })
                .AddGraphTypes(ServiceLifetime.Scoped);
        }
    }

    public static class ServicesRepository
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddSingleton<IPostRepostirory, PostRepository>();

        }
    }

    public static class GraphQlMiddleWareExtension
    {
        public static IApplicationBuilder UseGraphQL(this IApplicationBuilder app)
        {
            app.UseGraphQL<PostSchema>();
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());
            return app;
        }
    }
}
