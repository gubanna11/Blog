using System;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Posts;

public sealed record DeletePostCommand(Guid Id) : IRequest<Post?>;