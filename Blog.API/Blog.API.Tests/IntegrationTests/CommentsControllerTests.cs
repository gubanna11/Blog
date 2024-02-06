using System.Net;
using System.Text.Json;
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
            .RuleFor(c => c.UserId, _mockUser.Id);

        AddUserToData();
    }

    #region GetComment

    [Fact]
    public async Task GetComment_WhenComment_ReturnOkComment()
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

        comment.User = _mockUser;

        //Act
        var response = await _client.GetAsync($"/api/comments/{comment.CommentId}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Comment>(responseContent, _jsonSerializerOptions);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(comment);
    }

    [Fact]
    public async Task GetComment_WhenNoComment_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }

        //Act
        var response = await _client.GetAsync($"/api/comments/{Guid.Empty}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

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
            await dbContext.Comments.AddRangeAsync(expectedComments);
            await dbContext.SaveChangesAsync();
        }

        foreach (var comment in expectedComments)
        {
            comment.User = _mockUser;
        }

        // Act
        var response = await _client.GetAsync("/api/comments");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var comments = JsonSerializer.Deserialize<Comment[]>(responseContent, _jsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        comments.Should().BeOfType<Comment[]>();
    }

    [Fact]
    public async Task GetComments_WhenNoComments_ReturnsNotFound()
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();

            await dbContext.Database.EnsureCreatedAsync();
        }

        // Act
        var response = await _client.GetAsync("/api/comments");

        // Assert
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

        var getResponse = await _client.GetAsync($"/api/comments/{createdComment.CommentId}");
        getResponse.EnsureSuccessStatusCode();

        var responseContent = await getResponse.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Comment>(responseContent, _jsonSerializerOptions);

        // Assert
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeOfType<Comment>();
    }

    #endregion

    #region UpdateComment

    [Fact]
    public async Task UpdateComment_WhenComment_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        var newComment = _commentFaker.Generate();

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
            Content = newComment.Content,
            ParentCommentId = newComment.ParentCommentId,
            PostId = newComment.PostId,
            UserId = newComment.UserId,
            PublishDate = newComment.PublishDate,
        };

        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(updateCommentRequest),
            System.Text.Encoding.UTF8,
            "application/json");

        //Act
        var putResponse = await _client.PutAsync("/api/comments", content);
        putResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/comments/{comment.CommentId}");
        getResponse.EnsureSuccessStatusCode();

        var responseContent = await getResponse.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<Comment>(responseContent, _jsonSerializerOptions);

        //Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(newComment, opts =>
            opts.Excluding(c => c.CommentId).Excluding(c => c.User)
        );
    }

    [Fact]
    public async Task UpdateComment_WhenNoComment_ReturnNotFound()
    {
        //Arrange
        var comment = _commentFaker.Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
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
        deleteResponse.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync($"/api/comments/{comment.CommentId}");

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteComment_WhenNoComment_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/comments/{Guid.Empty}");

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

    private async void AddUserToData()
    {
        using var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

        await userManager.CreateAsync(_mockUser);
    }
}