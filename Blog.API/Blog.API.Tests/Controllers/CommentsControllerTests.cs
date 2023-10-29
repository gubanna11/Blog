using Blog.API.Controllers;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Core.MediatR.Queries.Comments;
using Bogus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Tests.Controllers;

public sealed class CommentsControllerTests
{
    private readonly Faker<Comment> _commentFaker;
    private readonly CommentsController _controller;
    private readonly Mock<IMediator> _mediator;

    public CommentsControllerTests()
    {
        _mediator = new Mock<IMediator>();
        _controller = new CommentsController(_mediator.Object);
        _commentFaker = new Faker<Comment>()
            .RuleFor(c => c.CommentId, f => f.Random.Guid())
            .RuleFor(c => c.Content, f => f.Lorem.Paragraph())
            .RuleFor(c => c.PostId, f => f.Random.Guid())
            .RuleFor(c => c.UserId, f => f.Random.Guid().ToString())
            .RuleFor(c => c.PublishDate, f => f.Date.Past())
            .RuleFor(c => c.ParentCommentId, f => f.Random.Guid());
    }

    #region CreateComment

    [Fact]
    public void CreateComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        CreateCommentRequest createComment = new(comment.Content, comment.PostId, comment.UserId, comment.PublishDate,
            comment.ParentCommentId);

        _mediator.Setup(m => m.Send(It.IsAny<CreateCommentCommand>(), default))
            .ReturnsAsync(comment);

        //Act
        var response = (_controller.CreateComment(createComment, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Comment;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Comment>();
        result.Should().BeEquivalentTo(comment, opts =>
            opts.Excluding(c => c.CommentId)
        );
    }

    #endregion

    #region GetComments

    [Fact]
    public void GetComments_WhenCalled_ReturnOk()
    {
        //Arrange
        var comments = _commentFaker.Generate(10);

        _mediator.Setup(m => m.Send(It.IsAny<GetCommentsQuery>(), default))
            .ReturnsAsync(comments);

        //Act
        var response = (_controller.GetComments(CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as List<Comment>;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<List<Comment>>();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(comments);
    }

    [Fact]
    public void GetComments_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var comments = _commentFaker.Generate(0);

        _mediator.Setup(m => m.Send(It.IsAny<GetCommentsQuery>(), default))
            .ReturnsAsync(comments);

        //Act
        var response = _controller.GetComments(CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region GetComment

    [Fact]
    public void GetComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();

        _mediator.Setup(m => m.Send(It.IsAny<GetCommentByIdQuery>(), default))
            .ReturnsAsync(comment);

        //Act
        var response =
            (_controller.GetCommentById(comment.CommentId, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Comment;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Comment>();
        result.Should().BeEquivalentTo(comment);
    }

    [Fact]
    public void GetComment_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Setup(m => m.Send(It.IsAny<GetCommentByIdQuery>(), default))
            .ReturnsAsync((Comment)null!);

        //Act
        var response = _controller.GetCommentById(Guid.NewGuid(), CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region UpdateComment

    [Fact]
    public void UpdateComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        UpdateCommentRequest updateComment = new(comment.CommentId, comment.Content, comment.PostId, comment.UserId,
            comment.PublishDate, comment.ParentCommentId);

        _mediator.Setup(m => m.Send(It.IsAny<UpdateCommentCommand>(), default))
            .ReturnsAsync(comment);

        //Act
        var response = (_controller.UpdateComment(updateComment, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Comment;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Comment>();
        result.Should().BeEquivalentTo(comment);
    }

    [Fact]
    public void UpdateComment_WhenCalled_ReturnNotFound()
    {
        //Arrange
        var comment = _commentFaker.Generate();
        UpdateCommentRequest updateComment = new(comment.CommentId, comment.Content, comment.PostId, comment.UserId,
            comment.PublishDate, comment.ParentCommentId);

        _mediator.Setup(m => m.Send(It.IsAny<UpdateCommentCommand>(), default))
            .ReturnsAsync((Comment)null!);

        //Act
        var response = _controller.UpdateComment(updateComment, CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion

    #region DeleteComment

    [Fact]
    public void DeleteComment_WhenCalled_ReturnOk()
    {
        //Arrange
        var comment = _commentFaker.Generate();

        _mediator.Setup(m => m.Send(It.IsAny<DeleteCommentCommand>(), default))
            .ReturnsAsync(comment);

        //Act
        var response =
            (_controller.DeleteComment(comment.CommentId, CancellationToken.None).Result as OkObjectResult)!;
        var result = response.Value as Comment;

        //Assert
        response.Should().BeOfType<OkObjectResult>();
        result.Should().BeOfType<Comment>();
    }

    [Fact]
    public void DeleteComment_WhenCalled_ReturnNotFound()
    {
        //Arrange
        _mediator.Setup(m => m.Send(It.IsAny<DeleteCommentCommand>(), default))
            .ReturnsAsync((Comment)null!);

        //Act
        var response = _controller.DeleteComment(Guid.NewGuid(), CancellationToken.None).Result as NotFoundResult;

        //Assert
        response.Should().BeOfType<NotFoundResult>();
    }

    #endregion
}