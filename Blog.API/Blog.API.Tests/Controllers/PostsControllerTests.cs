using Blog.API.Controllers;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Core.MediatR.Queries.Posts;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ReturnsExtensions;

namespace Blog.API.Tests.Controllers;

public sealed class PostsControllerTests
{
    private readonly PostsController _controller;
    private readonly IMediator _mediator;
    private readonly Faker<PostResponse> _postFaker;

    public PostsControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new PostsController(_mediator);
        _postFaker = new Faker<PostResponse>()
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
    public async void CreatePost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();
        CreatePostRequest createPost = new()
        {
            CategoryId = post.CategoryId,
            Content = post.Content,
            IsActive = post.IsActive,
            Title = post.Title,
            UserId = post.UserId,
        };

        _mediator.Send(Arg.Any<CreatePostCommand>())
            .ReturnsForAnyArgs(post);

        //Act
        var response = (await _controller.CreatePost(createPost, CancellationToken.None) as OkObjectResult)!;
        var result = (response.Value as PostResponse)!;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<PostResponse>();
        result.Should().BeEquivalentTo(post, opts =>
            opts.Excluding(c => c.PostId)
        );
    }

    #endregion

    #region GetPosts

    [Fact]
    public async void GetPosts_WhenCalled_ReturnOk()
    {
        //Arrange
        var posts = _postFaker.Generate(10);

        _mediator.Send(Arg.Any<GetPostsQuery>())
            .ReturnsForAnyArgs(posts);

        //Act
        var response = (await _controller.GetPosts(CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as List<PostResponse>;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<List<PostResponse>>();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(posts);
    }

    [Fact]
    public async void GetPosts_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var posts = _postFaker.Generate(0);

        _mediator.Send(Arg.Any<GetPostsQuery>())
            .ReturnsForAnyArgs(posts);

        //Act
        var response = await _controller.GetPosts(CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region GetPost

    [Fact]
    public async void GetPost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();

        _mediator.Send(Arg.Any<GetPostByIdQuery>())
            .ReturnsForAnyArgs(post);

        //Act
        var response = (await _controller.GetPostById(post.PostId, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as PostResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<PostResponse>();
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async void GetPost_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetPostByIdQuery>())
            .ReturnsNullForAnyArgs();

        //Act
        var response = await _controller.GetPostById(Guid.NewGuid(), CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region UpdatePost

    [Fact]
    public async void UpdatePost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();
        UpdatePostRequest updatePost = new()
        {
            CategoryId = post.CategoryId,
            Content = post.Content,
            IsActive = post.IsActive,
            PostId = post.PostId,
            PublishDate = post.PublishDate,
            Title = post.Title,
            UserId = post.UserId,
        };

        _mediator.Send(Arg.Any<UpdatePostCommand>())
            .ReturnsForAnyArgs(post);

        //Act
        var response = (await _controller.UpdatePost(updatePost, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as PostResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<PostResponse>();
        result.Should().BeEquivalentTo(post);
    }

    [Fact]
    public async void UpdatePost_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var post = _postFaker.Generate();
        UpdatePostRequest updatePost = new()
        {
            CategoryId = post.CategoryId,
            Content = post.Content,
            IsActive = post.IsActive,
            PostId = post.PostId,
            PublishDate = post.PublishDate,
            Title = post.Title,
            UserId = post.UserId,
        };

        _mediator.Send(Arg.Any<UpdatePostCommand>())
            .ReturnsNullForAnyArgs();

        //Act
        var response = await _controller.UpdatePost(updatePost, CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region DeletePost

    [Fact]
    public async void DeletePost_WhenCalled_ReturnOk()
    {
        //Arrange
        var post = _postFaker.Generate();

        _mediator.Send(Arg.Any<DeletePostCommand>())
            .ReturnsForAnyArgs(post);

        //Act
        var response =
            (await _controller.DeletePost(post.PostId, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as PostResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<PostResponse>();
    }

    [Fact]
    public async void DeletePost_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Send(Arg.Any<DeletePostCommand>())
            .ReturnsNullForAnyArgs();

        //Act
        var response = await _controller.DeletePost(Guid.NewGuid(), CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion
}