using Blog.API.Controllers;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.MediatR.Queries.Categories;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Tests.Controllers;

public sealed class CategoriesControllerTests
{
    private readonly Faker<CategoryResponse> _categoryFaker;
    private readonly CategoriesController _controller;
    private readonly IMediator _mediator;

    public CategoriesControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new CategoriesController(_mediator);
        _categoryFaker = new Faker<CategoryResponse>()
            .RuleFor(c => c.CategoryId, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Name.JobArea());
    }

    #region CreateCategory

    [Fact]
    public async void CreateCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();
        CreateCategoryRequest createCategory = new(category.Name);

        _mediator.Send(Arg.Any<CreateCategoryCommand>())
            .ReturnsForAnyArgs(category);

        //Act
        var response = (await _controller.CreateCategory(createCategory, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CategoryResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CategoryResponse>();
        result.Should().BeEquivalentTo(category, opts =>
            opts.Excluding(c => c.CategoryId)
        );
    }

    #endregion

    #region GetCategories

    [Fact]
    public async void GetCategories_WhenCalled_ReturnOk()
    {
        //Arrange
        var categories = _categoryFaker.Generate(10);

        _mediator.Send(Arg.Any<GetCategoriesQuery>())
            .ReturnsForAnyArgs(categories);

        //Act
        var response = (await _controller.GetCategories(CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as List<CategoryResponse>;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<List<CategoryResponse>>();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public async void GetCategories_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var categories = _categoryFaker.Generate(0);

        _mediator.Send(Arg.Any<GetCategoriesQuery>())
            .ReturnsForAnyArgs(categories);

        //Act
        var response = await _controller.GetCategories(CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region GetCategory

    [Fact]
    public async void GetCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();

        _mediator.Send(Arg.Any<GetCategoryByIdQuery>())
            .ReturnsForAnyArgs(category);

        //Act
        var response = (await _controller.GetCategoryById(category.CategoryId, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CategoryResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CategoryResponse>();
        result.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async void GetCategory_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetCategoryByIdQuery>())
            .ReturnsForAnyArgs((CategoryResponse)null!);

        //Act
        var response = await _controller.GetCategoryById(Guid.NewGuid(), CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region UpdateCategory

    [Fact]
    public async void UpdateCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();
        UpdateCategoryRequest updateCategory = new(category.CategoryId, category.Name);

        _mediator.Send(Arg.Any<UpdateCategoryCommand>())
            .ReturnsForAnyArgs(category);

        //Act
        var response = (await _controller.UpdateCategory(updateCategory, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CategoryResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CategoryResponse>();
        result.Should().BeEquivalentTo(category);
    }

    [Fact]
    public async void UpdateCategory_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var category = _categoryFaker.Generate();
        UpdateCategoryRequest updateCategory = new(category.CategoryId, category.Name);

        _mediator.Send(Arg.Any<UpdateCategoryCommand>())
            .ReturnsForAnyArgs((CategoryResponse)null!);

        //Act
        var response = await _controller.UpdateCategory(updateCategory, CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region DeleteCategory

    [Fact]
    public async void DeleteCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();

        _mediator.Send(Arg.Any<DeleteCategoryCommand>())
            .ReturnsForAnyArgs(category);

        //Act
        var response =
            (await _controller.DeleteCategory(category.CategoryId, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CategoryResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CategoryResponse>();
    }

    [Fact]
    public async void DeleteCategory_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Send(Arg.Any<DeleteCategoryCommand>())
            .ReturnsForAnyArgs((CategoryResponse)null!);

        //Act
        var response = await _controller.DeleteCategory(Guid.NewGuid(), CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion
}