using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace ProductStore.ConfigurationError
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                if (error is AppException)
                {
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
                if (error is ExistModel)
                {
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
                else
                {
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
