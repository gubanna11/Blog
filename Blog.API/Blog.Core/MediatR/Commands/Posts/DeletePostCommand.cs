using Blog.Core.Contracts.Controllers.Posts;
using MediatR;
using System;

namespace Blog.Core.MediatR.Commands.Posts;

public sealed record DeletePostCommand(Guid Id) : IRequest<PostResponse?>;