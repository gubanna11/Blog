using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpanJson;
using System;
using System.Threading.Tasks;

namespace Blog.API.Middlewares;

public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = $"An internal server error has occurred: {ex.Message}",
            };

            byte[] json = JsonSerializer.Generic.Utf8.Serialize(problemDetails);

            context.Response.ContentType = "application/json";

            await context.Response.Body.WriteAsync(json);
        }
    }
}
