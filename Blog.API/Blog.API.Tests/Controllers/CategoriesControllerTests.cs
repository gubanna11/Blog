using Blog.API.Controllers;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.MediatR.Queries.Categories;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Tests.Controllers;

public sealed class CategoriesControllerTests
{
    private readonly Faker<Category> _categoryFaker;
    private readonly CategoriesController _controller;
    private readonly Mock<IMediator> _mediator;

    public CategoriesControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new CategoriesController(_mediator.Object);
        _categoryFaker = new Faker<Category>()
            .RuleFor(c => c.CategoryId, f => f.Random.Guid())
            .RuleFor(c => c.Name, f => f.Name.JobArea());
    }

    #region CreateCategory

    [Fact]
    public void CreateCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();
        CreateCategoryRequest createCategory = new(category.Name);

        _mediator.Setup(m => m.Send(It.IsAny<CreateCategoryCommand>(), default))
            .ReturnsAsync(category);

        //Act
        var response = (_controller.CreateCategory(createCategory, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Category;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Category>();
        result.Should().BeEquivalentTo(category, opts =>
            opts.Excluding(c => c.CategoryId)
        );
    }

    #endregion

    #region GetCategories

    [Fact]
    public void GetCategories_WhenCalled_ReturnOk()
    {
        //Arrange
        var categories = _categoryFaker.Generate(10);

        _mediator.Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), default))
            .ReturnsAsync(categories);

        //Act
        var response = (_controller.GetCategories(CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as List<Category>;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<List<Category>>();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(categories);
    }

    [Fact]
    public void GetCategories_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var categories = _categoryFaker.Generate(0);

        _mediator.Setup(m => m.Send(It.IsAny<GetCategoriesQuery>(), default))
            .ReturnsAsync(categories);

        //Act
        var response = _controller.GetCategories(CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region GetCategory

    [Fact]
    public void GetCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();

        _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), default))
            .ReturnsAsync(category);

        //Act
        var response =
            (_controller.GetCategoryById(category.CategoryId, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Category;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Category>();
        result.Should().BeEquivalentTo(category);
    }

    [Fact]
    public void GetCategory_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Setup(m => m.Send(It.IsAny<GetCategoryByIdQuery>(), default))
            .ReturnsAsync((Category)null!);

        //Act
        var response = _controller.GetCategoryById(Guid.NewGuid(), CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region UpdateCategory

    [Fact]
    public void UpdateCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();
        UpdateCategoryRequest updateCategory = new(category.CategoryId, category.Name);

        _mediator.Setup(m => m.Send(It.IsAny<UpdateCategoryCommand>(), default))
            .ReturnsAsync(category);

        //Act
        var response = (_controller.UpdateCategory(updateCategory, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Category;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Category>();
        result.Should().BeEquivalentTo(category);
    }

    [Fact]
    public void UpdateCategory_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var category = _categoryFaker.Generate();
        UpdateCategoryRequest updateCategory = new(category.CategoryId, category.Name);

        _mediator.Setup(m => m.Send(It.IsAny<UpdateCategoryCommand>(), default))
            .ReturnsAsync((Category)null!);

        //Act
        var response = _controller.UpdateCategory(updateCategory, CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region DeleteCategory

    [Fact]
    public void DeleteCategory_WhenCalled_ReturnOk()
    {
        //Arrange
        var category = _categoryFaker.Generate();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), default))
            .ReturnsAsync(category);

        //Act
        var response =
            (_controller.DeleteCategory(category.CategoryId, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Category;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Category>();
    }

    [Fact]
    public void DeleteCategory_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Setup(m => m.Send(It.IsAny<DeleteCategoryCommand>(), default))
            .ReturnsAsync((Category)null!);

        //Act
        var response = _controller.DeleteCategory(Guid.NewGuid(), CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion
}