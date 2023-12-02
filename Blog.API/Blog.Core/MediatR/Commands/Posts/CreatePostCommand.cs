using Blog.Core.Contracts.Controllers.Posts;
using MediatR;

namespace Blog.Core.MediatR.Commands.Posts;

public sealed record CreatePostCommand(CreatePostRequest Post) : IRequest<PostResponse?>;