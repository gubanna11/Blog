using Blog.API.Endpoints;
using Blog.API.Middlewares;
using Blog.Dependencies;
using HealthChecks.UI.Client;
using Blog.Infrastructure.Services;
using Blog.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpanJson.AspNetCore.Formatter;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers().AddSpanJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureEnvironment(builder.Configuration);

builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapCategoriesEndpoints();
app.MapCommentsEndpoints();
app.MapPostsEndpoints();

app.MapControllers();

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });


app.Run();

public partial class Program { }

