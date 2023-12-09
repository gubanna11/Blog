using Blog.API.Controllers;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Core.MediatR.Queries.Comments;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ReturnsExtensions;

namespace Blog.API.Tests.Controllers;

public sealed class CommentsControllerTests
{
    private readonly Faker<CommentResponse> _commentFaker;
    private readonly CommentsController _controller;
    private readonly IMediator _mediator;

    public CommentsControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new CommentsController(_mediator);
        _commentFaker = new Faker<CommentResponse>()
            .RuleFor(c => c.CommentId, f => f.Random.Guid())
            .RuleFor(c => c.Content, f => f.Lorem.Paragraph())
            .RuleFor(c => c.PostId, f => f.Random.Guid())
            .RuleFor(c => c.UserId, f => f.Random.Guid().ToString())
            .RuleFor(c => c.PublishDate, f => f.Date.Past())
            .RuleFor(c => c.ParentCommentId, f => f.Random.Guid());
    }

    #region CreateComment

    [Fact]
    public async void CreateComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        CreateCommentRequest createComment = new(comment.Content, comment.PostId, comment.UserId, comment.ParentCommentId);

        _mediator.Send(Arg.Any<CreateCommentCommand>())
            .ReturnsForAnyArgs(comment);

        //Act
        var response = (await _controller.CreateComment(createComment, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CommentResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CommentResponse>();
        result.Should().BeEquivalentTo(comment, opts =>
            opts.Excluding(c => c.CommentId)
        );
    }

    #endregion

    #region GetComments

    [Fact]
    public async void GetComments_WhenCalled_ReturnOk()
    {
        //Arrange
        var comments = _commentFaker.Generate(10);

        _mediator.Send(Arg.Any<GetCommentsQuery>())
            .ReturnsForAnyArgs(comments);

        //Act
        var response = (await _controller.GetComments(CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as List<CommentResponse>;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<List<CommentResponse>>();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(comments);
    }

    [Fact]
    public async void GetComments_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var comments = _commentFaker.Generate(0);

        _mediator.Send(Arg.Any<GetCommentsQuery>())
            .ReturnsForAnyArgs(comments);

        //Act
        var response = await _controller.GetComments(CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region GetComment

    [Fact]
    public async void GetComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();

        _mediator.Send(Arg.Any<GetCommentByIdQuery>())
            .ReturnsForAnyArgs(comment);

        //Act
        var response =
            (await _controller.GetCommentById(comment.CommentId, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CommentResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CommentResponse>();
        result.Should().BeEquivalentTo(comment);
    }

    [Fact]
    public async void GetComment_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Send(Arg.Any<GetCommentByIdQuery>())
            .ReturnsNullForAnyArgs();

        //Act
        var response = await _controller.GetCommentById(Guid.NewGuid(), CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region UpdateComment

    [Fact]
    public async void UpdateComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        UpdateCommentRequest updateComment = new(comment.CommentId, comment.Content, comment.PostId, comment.UserId,
            comment.PublishDate, comment.ParentCommentId);

        _mediator.Send(Arg.Any<UpdateCommentCommand>())
            .ReturnsForAnyArgs(comment);

        //Act
        var response = (await _controller.UpdateComment(updateComment, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CommentResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CommentResponse>();
        result.Should().BeEquivalentTo(comment);
    }

    [Fact]
    public async void UpdateComment_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        UpdateCommentRequest updateComment = new(comment.CommentId, comment.Content, comment.PostId, comment.UserId,
            comment.PublishDate, comment.ParentCommentId);

        _mediator.Send(Arg.Any<UpdateCommentCommand>())
            .ReturnsNullForAnyArgs();

        //Act
        var response = await _controller.UpdateComment(updateComment, CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region DeleteComment

    [Fact]
    public async void DeleteComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();

        _mediator.Send(Arg.Any<DeleteCommentCommand>())
            .ReturnsForAnyArgs(comment);

        //Act
        var response =
            (await _controller.DeleteComment(comment.CommentId, CancellationToken.None) as OkObjectResult)!;
        var result = response.Value as CommentResponse;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<CommentResponse>();
    }

    [Fact]
    public async void DeleteComment_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Send(Arg.Any<DeleteCommentCommand>())
            .ReturnsNullForAnyArgs();

        //Act
        var response = await _controller.DeleteComment(Guid.NewGuid(), CancellationToken.None) as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion
}