using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Dependencies;
public static class Dependencies
{
    public static IServiceCollection ConfigureDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDatabase(configuration);
        return services;
    }

    #region private methods
    private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("ConnectionString");

        services.AddDbContext<ApiDataContext>(options => options.UseSqlServer(connectionString));
        //services.AddDbContext<ApiDataContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0 ,20))));

        services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApiDataContext>()
                .AddDefaultTokenProviders();
    }
    #endregion
}