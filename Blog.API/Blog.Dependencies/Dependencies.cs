using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Entities;
using Blog.Core.Validators.Categories;
using Blog.Core.Validators.Comments;
using Blog.Core.Validators.Posts;
using Blog.Infrastructure.Abstract;
using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Mapster;
using Blog.Infrastructure.MediatR.Handlers.Posts;
using Blog.Infrastructure.Services;
using Blog.Infrastructure.Services.Interfaces;
using FluentValidation;
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
        services.ConfigureMapster();
        services.ConfigureUnitOfWork();
        services.ConfigureServices();
        services.ConfigureMediatR();
        services.ConfigureValidators();

        return services;
    }

    private static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApiDataContext>(
            options => options.UseSqlServer(configuration["ConnectionStrings:Database"]!));

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

    private static void ConfigureValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateCategoryRequest>, CreateCategoryRequestValidator>();
        services.AddScoped<IValidator<UpdateCategoryRequest>, UpdateCategoryRequestValidator>();

        services.AddScoped<IValidator<CreateCommentRequest>, CreateCommentRequestValidator>();
        services.AddScoped<IValidator<UpdateCommentRequest>, UpdateCommentRequestValidator>();

        services.AddScoped<IValidator<CreatePostRequest>, CreatePostRequestValidator>();
        services.AddScoped<IValidator<UpdatePostRequest>, UpdatePostRequestValidator>();
    }
}