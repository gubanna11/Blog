using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Services;
using Blog.Infrastructure.Services.Interfaces;
using Blog.Infrastructure.MediatR.Handlers.Posts;
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
        services.ConfigureServices();
        services.ConfigureMediatR();
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

    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();

        services.AddScoped<IUserService, UserService>();
    }

    private static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetPostsHandler>());
    }
}