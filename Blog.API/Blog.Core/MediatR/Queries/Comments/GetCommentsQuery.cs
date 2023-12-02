﻿using Blog.Core.Contracts.ResponseDtos;
using MediatR;
using System.Collections.Generic;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetCommentsQuery : IRequest<IEnumerable<CommentResponse>>;