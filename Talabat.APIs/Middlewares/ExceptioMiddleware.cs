using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    public class ExceptioMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptioMiddleware> logger;
        private readonly IHostEnvironment env;

        public ExceptioMiddleware(RequestDelegate next,ILogger<ExceptioMiddleware> logger , IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.env = environment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, ex.Message);
                //Log Exception in Database [production]
                context.Response.ContentType= "application/json";
                context.Response.StatusCode =(int) HttpStatusCode.InternalServerError;

                var response = env.IsDevelopment() ?
                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString())
                    : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json=JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
