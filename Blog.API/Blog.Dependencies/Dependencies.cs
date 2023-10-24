using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Dependencies;
public static class Dependencies
{
    public static IServiceCollection ConfigureEnvironment(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDatabase(configuration);
        return services;
    }

    private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration["ConnectionString:String"]!;

        services.AddDbContext<ApiDataContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApiDataContext>()
                .AddDefaultTokenProviders();
    }
}