using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Posts;

public sealed record UpdatePostCommand(UpdatePostRequest Post) : IRequest<Post?>;