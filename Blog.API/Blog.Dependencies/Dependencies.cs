using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Mapster;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Dependencies;

public static class Dependencies
{
    public static IServiceCollection ConfigureEnvironment(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureDatabase(configuration);
        return services;
    }

    private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApiDataContext>(
            options => options.UseSqlServer(configuration["ConnectionString:String"]!));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApiDataContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig config = new();
        config.Apply(new MapsterRegister());
        services.AddSingleton(config);

        services.AddSingleton<IMapper>(sp => new ServiceMapper(sp, config));
    }
}