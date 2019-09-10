using API.Instragram.GraphQL.Schemas;
using API.Instragram.Repository;
using API.Instragram.Repository.Context;
using API.Instragram.Repository.FakeRepository;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.IO;
using System.Text;

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
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddDbContext<PostDbContext>(((serviceProvider, options) =>
            {
                var httpContext = serviceProvider.GetService<IHttpContextAccessor>().HttpContext;

                //não  estou conseguindo pegar o content body
                //var bodyParameter = GetRawBodyString(httpContext, Encoding.UTF8);
                var databaseQuerystringParameter = httpContext.Request.Query["database"].ToString();


                var db2ConnectionString = Configuration.GetConnectionString("DefaultConnection");

                //if (bodyParameter.Contains("filter"))
                if (!String.IsNullOrEmpty(databaseQuerystringParameter))
                {

                    db2ConnectionString = Configuration.GetConnectionString("database1");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine("                 mudei a conection string do banco");
                }

                options.UseInMemoryDatabase(databaseName: db2ConnectionString);



            }));

            services.AddScoped<IPostRepository, PostRepository>();

       

        }
        public static string GetRawBodyString(this HttpContext httpContext, Encoding encoding)
        {
            var body = "";

            using (var reader = new StreamReader(httpContext.Request.Body))
            {
                body = reader.ReadToEnd();

            }
            return body;
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
