using Blog.API.Controllers;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Core.MediatR.Queries.Posts;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Tests.Controllers;

public sealed class PostsControllerTests
{
    private readonly PostsController _controller;
    private readonly Mock<IMediator> _mediator;
    private readonly Faker<Post> _postFaker;

    public PostsControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new PostsController(_mediator.Object);
        _postFaker = new Faker<Post>()
            .RuleFor(p => p.PostId, f => f.Random.Guid())
            .RuleFor(p => p.Title, f => f.Lorem.Sentence(5))
            .RuleFor(p => p.Content, f => f.Lorem.Paragraph(5))
            .RuleFor(p => p.UserId, f => f.Random.Guid().ToString())
            .RuleFor(p => p.PublishDate, f => f.Date.Past())
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .RuleFor(p => p.CategoryId, f => f.Random.Guid());
    }

    #region CreatePost

    [Fact]
    public void CreatePost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();
        CreatePostRequest createPost = new(post.Title, post.Content, post.UserId, post.PublishDate, post.IsActive,
            post.CategoryId);

        _mediator.Setup(m => m.Send(It.IsAny<CreatePostCommand>(), default))
            .ReturnsAsync(post);

        //Act
        var response = (_controller.CreatePost(createPost, CancellationToken.None).Result as OkObjectResult)!;
        var result = (response.Value as Post)!;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Post>();
        result.Should().BeEquivalentTo(post, opts =>
            opts.Excluding(c => c.PostId)
        );
    }

    #endregion

    #region GetPosts

    [Fact]
    public void GetPosts_WhenCalled_ReturnOk()
    {
        //Arrange
        var posts = _postFaker.Generate(10);

        _mediator.Setup(m => m.Send(It.IsAny<GetPostsQuery>(), default))
            .ReturnsAsync(posts);

        //Act
        var response = (_controller.GetPosts(CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as List<Post>;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<List<Post>>();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public void GetPosts_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var posts = _postFaker.Generate(0);

        _mediator.Setup(m => m.Send(It.IsAny<GetPostsQuery>(), default))
            .ReturnsAsync(posts);

        //Act
        var response = _controller.GetPosts(CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region GetPost

    [Fact]
    public void GetPost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();

        _mediator.Setup(m => m.Send(It.IsAny<GetPostByIdQuery>(), default))
            .ReturnsAsync(post);

        //Act
        var response =
            (_controller.GetPostById(post.PostId, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Post;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Post>();
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public void GetPost_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Setup(m => m.Send(It.IsAny<GetPostByIdQuery>(), default))
            .ReturnsAsync((Post)null!);

        //Act
        var response = _controller.GetPostById(Guid.NewGuid(), CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region UpdatePost

    [Fact]
    public void UpdatePost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();
        UpdatePostRequest updatePost = new(post.PostId, post.Title, post.Content, post.UserId, post.PublishDate,
            post.IsActive, post.CategoryId);

        _mediator.Setup(m => m.Send(It.IsAny<UpdatePostCommand>(), default))
            .ReturnsAsync(post);

        //Act
        var response = (_controller.UpdatePost(updatePost, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Post;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Post>();
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public void UpdatePost_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var post = _postFaker.Generate();
        UpdatePostRequest updatePost = new(post.PostId, post.Title, post.Content, post.UserId, post.PublishDate,
            post.IsActive, post.CategoryId);

        _mediator.Setup(m => m.Send(It.IsAny<UpdatePostCommand>(), default))
            .ReturnsAsync((Post)null!);

        //Act
        var response = _controller.UpdatePost(updatePost, CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region DeletePost

    [Fact]
    public void DeletePost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();

        _mediator.Setup(m => m.Send(It.IsAny<DeletePostCommand>(), default))
            .ReturnsAsync(post);

        //Act
        var response =
            (_controller.DeletePost(post.PostId, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Post;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Post>();
    }

    [Fact]
    public void DeletePost_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Setup(m => m.Send(It.IsAny<DeletePostCommand>(), default))
            .ReturnsAsync((Post)null!);

        //Act
        var response = _controller.DeletePost(Guid.NewGuid(), CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion
}