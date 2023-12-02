using Blog.Core.Contracts.ResponseDtos;
using MediatR;
using System;

namespace Blog.Core.MediatR.Commands.Posts;

public sealed record DeletePostCommand(Guid Id) : IRequest<PostResponse?>;