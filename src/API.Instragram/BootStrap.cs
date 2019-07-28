using API.Instragram.GraphQL.Schemas;
using API.Instragram.Repository;
using API.Instragram.Repository.Context;
using API.Instragram.Repository.FakeRepository;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                .AddGraphTypes(ServiceLifetime.Scoped).AddDataLoader();
        }
    }

    public static class ServicesRepository
    {
        public static void AddRepository(this IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddDbContext<PostDbContext>(option => option.UseInMemoryDatabase(databaseName: "PostDataBaseMemory"));
            services.AddDbContext<PostDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<PostRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

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

    public static class FakeRepositoryExtension
    {
        public static IApplicationBuilder UseFakeRepository(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PostDbContext>();
                FakeRepository.AdicionarDadosTeste(context);
            }
            return app;
        }
    }
}
