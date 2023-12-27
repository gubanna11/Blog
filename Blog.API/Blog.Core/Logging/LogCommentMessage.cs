using System;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Logging;

public static partial class LogCommentMessage
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Comment object with id {GetByIdCommentId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWithIdDoesNotExist(this ILogger logger, Guid getByIdCommentId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Comment with id {CreatedCommentId} was created",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasCreated(this ILogger logger, Guid createdCommentId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Comment wasn't created",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasNotCreated(this ILogger logger);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Comment object with id {DeleteCommentId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasNotDeleted(this ILogger logger, Guid deleteCommentId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Comment object with id {DeletedCommentId} was deleted",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasDeleted(this ILogger logger, Guid deletedCommentId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Comment with id {FailedUpdateCommentId} was not updated",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasNotUpdated(this ILogger logger, Guid failedUpdateCommentId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Comment with id {UpdatedCommentId} was updated",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasUpdated(this ILogger logger, Guid updatedCommentId);
}