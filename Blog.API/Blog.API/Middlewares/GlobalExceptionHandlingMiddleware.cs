using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Blog.API.Middlewares;

public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("An unhandled exception occured. Exception: {error}", ex.ToString());

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status500InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = $"An internal server error has occurred",
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
