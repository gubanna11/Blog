using System.Net;
using System.Text.Json;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Bogus;
using Meziantou.Xunit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.API.Tests.IntegrationTests;

[DisableParallelization]
public sealed class PostsControllerTests
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Faker<Post> _postFaker;
    private readonly User _mockUser;
    private readonly Category _mockCategory;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PostsControllerTests()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b =>
            {
                b.ConfigureServices(services =>
                {
                    var descriptor =
                        services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApiDataContext>));

                    if (descriptor is not null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApiDataContext>(opts =>
                    {
                        opts.UseInMemoryDatabase("InMemoryDatabasePosts");
                    });
                });
            });
        _client = _factory.CreateClient();
        _mockUser = MockUser();
        _mockCategory = MockCategory();
        _postFaker = new Faker<Post>()
            .RuleFor(p => p.PostId, f => f.Random.Guid())
            .RuleFor(p => p.Title, f => f.Lorem.Sentence(5))
            .RuleFor(p => p.Content, f => f.Lorem.Paragraph(5))
            .RuleFor(p => p.UserId, _mockUser.Id)
            .RuleFor(p => p.PublishDate, f => f.Date.Past())
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .RuleFor(p => p.CategoryId, _mockCategory.CategoryId);

        AddUserToData();
        AddCategoryToData();
    }

    #region GetPost

    [Fact]
    public async Task GetPost_WhenPost_ReturnOkPost()
    {
        //Arrange
        var post = _postFaker.Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Posts.AddAsync(post);
            await dbContext.SaveChangesAsync();
        }

        post.User = _mockUser;
        post.Category = _mockCategory;

        //Act
        var response = await _client.GetAsync($"/api/posts/{post.PostId}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Post>(responseContent, _jsonSerializerOptions);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async Task GetPost_WhenNoPost_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }

        //Act
        var response = await _client.GetAsync($"/api/posts/{Guid.Empty}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region GetPosts

    [Fact]
    public async Task GetPosts_WhenPosts_ReturnsOkResultWithPosts()
    {
        // Arrange
        var expectedPosts = _postFaker.Generate(10);

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();

            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Posts.AddRangeAsync(expectedPosts);
            await dbContext.SaveChangesAsync();
        }

        foreach (var post in expectedPosts)
        {
            post.User = _mockUser;
            post.Category = _mockCategory;
        }

        // Act
        var response = await _client.GetAsync("/api/posts");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var posts = JsonSerializer.Deserialize<Post[]>(responseContent, _jsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        posts.Should().BeEquivalentTo(expectedPosts);
    }

    [Fact]
    public async Task GetPosts_WhenNoPosts_ReturnsNotFound()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();

            await dbContext.Database.EnsureCreatedAsync();
        }

        // Act
        var response = await _client.GetAsync("/api/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion


    #region CreatePost

    [Fact]
    public async Task CreatePost_ReturnOk()
    {
        // Arrange
        var post = _postFaker.Generate();

        CreatePostRequest createPostRequest = new()
        {
            CategoryId = post.CategoryId,
            Content = post.Content,
            IsActive = post.IsActive,
            Title = post.Title,
            UserId = post.UserId,
        };

        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(createPostRequest),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var postResponse = await _client.PostAsync("/api/posts", content);
        postResponse.EnsureSuccessStatusCode();
        var createdPost =
            SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Post>(await postResponse.Content.ReadAsStringAsync());

        var getResponse = await _client.GetAsync($"/api/posts/{createdPost.PostId}");
        getResponse.EnsureSuccessStatusCode();

        var responseContent = await getResponse.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Post>(responseContent, _jsonSerializerOptions);

        // Assert
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeOfType<Post>();
    }

    #endregion

    #region UpdatePost

    [Fact]
    public async Task UpdatePost_WhenPost_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();
        var newPost = _postFaker.Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();

            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();
        }

        newPost.Category = _mockCategory;

        UpdatePostRequest updatePostRequest = new()
        {
            PostId = post.PostId,
            CategoryId = newPost.CategoryId,
            Content = newPost.Content,
            IsActive = newPost.IsActive,
            PublishDate = newPost.PublishDate,
            Title = newPost.Title,
            UserId = newPost.UserId,
        };

        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(updatePostRequest),
            System.Text.Encoding.UTF8,
            "application/json");

        //Act
        var putResponse = await _client.PutAsync("/api/posts", content);
        putResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/posts/{post.PostId}");
        getResponse.EnsureSuccessStatusCode();

        var responseContent = await getResponse.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Post>(responseContent, _jsonSerializerOptions);

        //Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(newPost, opts =>
            opts.Excluding(p => p.PostId).Excluding(c => c.User)
                .Excluding(p => p.Category!.Posts)
        );
    }

    [Fact]
    public async Task UpdatePost_WhenNoPost_ReturnNotFound()
    {
        //Arrange
        var post = _postFaker.Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }

        UpdatePostRequest updatePostRequest = new()
        {
            CategoryId = post.CategoryId,
            Content = post.Content,
            IsActive = post.IsActive,
            PostId = post.PostId,
            PublishDate = post.PublishDate,
            Title = post.Title,
            UserId = post.UserId,
        };
        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(updatePostRequest),
            System.Text.Encoding.UTF8,
            "application/json");

        //Act
        var putResponse = await _client.PutAsync("/api/posts", content);

        //Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region DeletePost

    [Fact]
    public async Task DeletePost_WhenPost_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();

            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Posts.Add(post);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/posts/{post.PostId}");
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/posts/{post.PostId}");

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePost_WhenNoPost_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/posts/{Guid.Empty}");

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    private static User MockUser()
    {
        PasswordHasher<User> hasher = new();
        const string mockUser = "Mock";
        const string mockPassword = "Secret123$";
        const string mockEmail = "mock@example.com";

        User user = new()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = mockUser,
            NormalizedUserName = mockUser.ToUpper(),
            Email = mockEmail,
            NormalizedEmail = mockEmail.ToUpper(),
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = false,
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };
        user.PasswordHash = hasher.HashPassword(user, mockPassword);

        return user;
    }

    private static Category MockCategory()
    {
        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.CategoryId, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Name.JobArea());

        return categoryFaker.Generate();
    }

    private async void AddUserToData()
    {
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        await userManager.CreateAsync(_mockUser);
    }

    private async void AddCategoryToData()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Categories.AddAsync(_mockCategory);
        await dbContext.SaveChangesAsync();
    }
}