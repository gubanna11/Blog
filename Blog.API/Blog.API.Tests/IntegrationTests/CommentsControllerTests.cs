using System.Net;
using System.Text.Json;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Contracts.Controllers.Comments;
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
public sealed class CommentsControllerTests
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Faker<Comment> _commentFaker;
    private readonly User _mockUser;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CommentsControllerTests()
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
                        opts.UseInMemoryDatabase("InMemoryDatabaseComments");
                    });
                });
            });
        _client = _factory.CreateClient();
        _mockUser = MockUser();
        _commentFaker = new Faker<Comment>()
            .RuleFor(c => c.CommentId, f => f.Random.Guid())
            .RuleFor(c => c.Content, f => f.Lorem.Paragraph())
            .RuleFor(c => c.PostId, f => f.Random.Guid())
            .RuleFor(c => c.PublishDate, f => f.Date.Past())
            .RuleFor(c => c.ParentCommentId, f => f.Random.Guid())
            .RuleFor(c => c.UserId, _mockUser.Id);

        AddUserToData();
    }

    #region GetComments

    [Fact]
    public async Task GetComments_WhenComments_ReturnsOkResultWithComments()
    {
        // Arrange
        var expectedComments = _commentFaker.Generate(10);

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Comments.RemoveRange(dbContext.Comments);
            await dbContext.Comments.AddRangeAsync(expectedComments);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await _client.GetAsync("/api/comments");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var comments = JsonSerializer.Deserialize<Comment[]>(responseContent, _jsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        comments.Should().BeEquivalentTo(expectedComments, options => options
            .Using<Comment>(ctx => ctx.Subject.User.Should().BeEquivalentTo(_mockUser))
            .WhenTypeIs<Comment>());
    }

    #endregion

    #region GetComment

    [Fact]
    public async Task GetComment_WhenComment_ReturnOkCategory()
    {
        //Arrange
        var comment = _commentFaker.Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var response = await _client.GetAsync($"/api/comments/{comment.CommentId}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Comment>(responseContent, _jsonSerializerOptions);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(comment, options => options
            .Using<Comment>(ctx => ctx.Subject.User.Should().BeEquivalentTo(_mockUser))
            .WhenTypeIs<Comment>());
    }

    [Fact]
    public async Task GetComment_WhenNoComment_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Comments.RemoveRange(dbContext.Comments);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var response = await _client.GetAsync($"/api/comments/{Guid.Empty}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region CreateComment

    [Fact]
    public async Task CreateComment_ReturnOk()
    {
        // Arrange
        var comment = _commentFaker.Generate();

        CreateCommentRequest createCommentRequest = new()
        {
            Content = comment.Content,
            ParentCommentId = comment.ParentCommentId,
            PostId = comment.PostId,
            UserId = comment.UserId,
        };
        
        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(createCommentRequest),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var postResponse = await _client.PostAsync("/api/comments", content);
        postResponse.EnsureSuccessStatusCode();
        var createdComment =
            SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Comment>(await postResponse.Content.ReadAsStringAsync());

        var getResponse = await _client.GetAsync("/api/comments");
        getResponse.EnsureSuccessStatusCode();

        var responseContent = await getResponse.Content.ReadAsStringAsync();

        var comments = SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Comment[]>(responseContent)!;

        // Assert
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        comments.Should().Contain(c => c.CommentId == createdComment.CommentId);
    }

    #endregion

    #region UpdateComment

    [Fact]
    public async Task UpdateComment_WhenComment_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }

        UpdateCommentRequest updateCommentRequest = new()
        {
            CommentId = comment.CommentId,
            Content = comment.Content,
            ParentCommentId = comment.ParentCommentId,
            PostId = comment.PostId,
            UserId = comment.UserId,
            PublishDate = comment.PublishDate,
        };
        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(updateCommentRequest),
            System.Text.Encoding.UTF8,
            "application/json");


        //Act
        var putResponse = await _client.PutAsync("/api/comments", content);

        //Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateComment_WhenNoComment_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Comments.RemoveRange(dbContext.Comments);
            await dbContext.SaveChangesAsync();
        }

        var updateCategoryRequest = new UpdateCategoryRequest()
        {
            CategoryId = Guid.NewGuid(),
            Name = "NewName"
        };
        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(updateCategoryRequest),
            System.Text.Encoding.UTF8,
            "application/json");


        //Act
        var putResponse = await _client.PutAsync("/api/comments", content);

        //Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region DeleteComment

    [Fact]
    public async Task DeleteComment_WhenComment_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/comments/{comment.CommentId}");

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteComment_WhenNoComment_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Comments.RemoveRange(dbContext.Comments);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/comments/{Guid.NewGuid()}");

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

    private void AddUserToData()
    {
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        userManager.CreateAsync(_mockUser).Wait();
    }
}