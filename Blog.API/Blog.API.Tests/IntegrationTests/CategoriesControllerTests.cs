using System.Net;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Bogus;
using Meziantou.Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.API.Tests.IntegrationTests;

[DisableParallelization]
public sealed class CategoriesControllerTests
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Faker<Category> _categoryFaker;

    public CategoriesControllerTests()
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
                        opts.UseInMemoryDatabase("InMemoryDatabaseCategories");
                    });
                });
            });
        _client = _factory.CreateClient();
        _categoryFaker = new Faker<Category>()
            .RuleFor(c => c.CategoryId, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Name.JobArea())
            .RuleFor(c => c.Posts, Enumerable.Empty<Post>());
    }

    #region GetCategories

    [Fact]
    public async Task GetCategories_WithCategories_ReturnsOkResultWithCategories()
    {
        // Arrange
        var expectedCategories = _categoryFaker.Generate(10);

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Categories.RemoveRange(dbContext.Categories);
            await dbContext.Categories.AddRangeAsync(expectedCategories);
            await dbContext.SaveChangesAsync();
        }


        // Act
        var response = await _client.GetAsync("/api/categories");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var categories = SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Category[]>(responseContent)!;

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Should().BeEquivalentTo(expectedCategories);
    }

    #endregion


    #region GetCategory

    [Fact]
    public async Task GetCategory_WhenCategory_ReturnOkCategory()
    {
        //Arrange
        var category = _categoryFaker.Generate();

        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var response = await _client.GetAsync($"/api/categories/{category.CategoryId}");
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        var result = SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Category>(responseContent)!;

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async Task GetCategory_WhenNoCategory_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Categories.RemoveRange(dbContext.Categories);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var response = await _client.GetAsync($"/api/categories/{Guid.Empty}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region CreateCategory

    [Fact]
    public async Task CreateCategory_ReturnOk()
    {
        // Arrange
        var category = _categoryFaker.Generate();
        CreateCategoryRequest createCategoryRequest = new()
        {
            Name = category.Name,
        };
        /*var createCategoryRequest = new CreateCategoryRequest()
        {
            Name = "Test category"
        };*/
        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(createCategoryRequest),
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var postResponse = await _client.PostAsync("/api/categories", content);
        postResponse.EnsureSuccessStatusCode();
        var createdCategory =
            SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Category>(await postResponse.Content.ReadAsStringAsync());

        var getResponse = await _client.GetAsync("/api/categories");
        getResponse.EnsureSuccessStatusCode();

        var responseContent = await getResponse.Content.ReadAsStringAsync();

        var categories = SpanJson.JsonSerializer.Generic.Utf16.Deserialize<Category[]>(responseContent)!;

        // Assert
        postResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        categories.Should().Contain(c => c.CategoryId == createdCategory.CategoryId);
    }

    #endregion

    #region UpdateCategory

    [Fact]
    public async Task UpdateCategory_WhenCategory_ReturnOk()
    {
        //Arrange
        Category category = new()
        {
            CategoryId = Guid.NewGuid(),
            Name = "Test"
        };
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
        }

        var updateCategoryRequest = new UpdateCategoryRequest()
        {
            CategoryId = category.CategoryId,
            Name = "NewName",
        };
        var content = new StringContent(SpanJson.JsonSerializer.Generic.Utf16.Serialize(updateCategoryRequest),
            System.Text.Encoding.UTF8,
            "application/json");


        //Act
        var putResponse = await _client.PutAsync("/api/categories", content);

        //Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCategory_WhenNoCategory_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Categories.RemoveRange(dbContext.Categories);
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
        var putResponse = await _client.PutAsync("/api/categories", content);

        //Assert
        putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region DeleteCategory

    [Fact]
    public async Task DeleteCategory_WhenCategory_ReturnOk()
    {
        //Arrange
        Category category = new()
        {
            CategoryId = Guid.NewGuid(),
            Name = "Test"
        };
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/categories/{category.CategoryId}");

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteCategory_WhenNoCategory_ReturnNotFound()
    {
        //Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApiDataContext>();
            await dbContext.Database.EnsureCreatedAsync();
            dbContext.Categories.RemoveRange(dbContext.Categories);
            await dbContext.SaveChangesAsync();
        }

        //Act
        var deleteResponse = await _client.DeleteAsync($"/api/categories/{Guid.NewGuid()}");

        //Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}