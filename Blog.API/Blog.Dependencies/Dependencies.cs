using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Mapster;
using Mapster;
using MapsterMapper;
using Blog.Infrastructure.Services;
using Blog.Infrastructure.Services.Interfaces;
using Blog.Infrastructure.MediatR.Handlers.Posts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Abstract;

namespace Blog.Dependencies;

public static class Dependencies
{
    public static IServiceCollection ConfigureEnvironment(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureDatabase(configuration);
        services.ConfigureMapster();
        services.ConfigureUnitOfWork();
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

    private static void ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig config = new();
        config.Apply(new MapsterRegister());
        services.AddSingleton(config);

        services.AddSingleton<IMapper>(sp => new ServiceMapper(sp, config));
    }

    private static void ConfigureUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
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