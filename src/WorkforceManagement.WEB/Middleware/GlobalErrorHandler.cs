using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WorkforceManagement.WEB.Middleware
{
    [ExcludeFromCodeCoverage]
    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case InvalidOperationException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;

                    case ArgumentException:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;

                    case UnauthorizedAccessException:
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        break;

                    default:
                        // Unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new
                {
                    ExceptionType = $"{error.GetType().Name}",
                    DefaultMessage = "There was an exception while trying to process your requests",
                    SpecificMessage = $"{error.Message}"
                });

                await response.WriteAsync(result);
            }
        }
    }
}
