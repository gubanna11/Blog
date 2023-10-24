using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Posts;

public sealed record CreatePostCommand(CreatePostRequest Post) : IRequest<Post>;