using API.Instragram.Repository.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace API.Instragram
{
    public class DataBaseChangeMiddleware
    {
        private readonly RequestDelegate next;

        public DataBaseChangeMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            var httpRequest = httpContext.Request;
            var databaseQuerystringParameter = httpRequest.Query["database"].ToString();
            httpContext.Items.Add("URL_database", databaseQuerystringParameter);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("                 adicionei o header  de data_base");
            return next(httpContext);
        }
    }
}
