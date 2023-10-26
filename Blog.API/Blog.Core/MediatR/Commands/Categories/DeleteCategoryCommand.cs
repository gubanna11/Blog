﻿using System;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Categories;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest<Category?>;