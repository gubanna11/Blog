using System;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Logging;

public static partial class LogPostMessage
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Post object with id {GetByIdPostId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogPostWithIdDoesNotExist(this ILogger logger, Guid getByIdPostId);

    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Post with id {CreatedPostId} was created",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasCreated(this ILogger logger, Guid createdPostId);

    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Post wasn't created",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasNotCreated(this ILogger logger);

    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Post object with id {DeletePostId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasNotDeleted(this ILogger logger, Guid deletePostId);

    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Post object with id {DeletedPostId} was deleted",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasDeleted(this ILogger logger, Guid deletedPostId);

    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Post with id {FailedUpdatePostId} was not updated",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasNotUpdated(this ILogger logger, Guid failedUpdatePostId);

    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Post with id {UpdatedPostId} was updated",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasUpdated(this ILogger logger, Guid updatedPostId);
}