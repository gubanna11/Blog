﻿using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Posts;

public sealed record UpdatePostCommand(UpdatePostRequest Post) : IRequest<Post?>;