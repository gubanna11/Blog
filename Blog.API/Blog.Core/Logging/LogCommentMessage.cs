using System;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Logging;

public static partial class LogCommentMessage
{
    [LoggerMessage(EventId = LogEvents.CommentWasNotGottenId, EventName = LogEvents.CommentWasNotGottenName,
        Level = LogLevel.Error,
        Message = "Comment object with id {GetByIdCommentId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWithIdDoesNotExist(this ILogger logger, Guid getByIdCommentId);

    [LoggerMessage(EventId = LogEvents.CommentWasCreatedId, EventName = LogEvents.CommentWasCreatedName,
        Level = LogLevel.Information,
        Message = "Comment with id {CreatedCommentId} was created",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasCreated(this ILogger logger, Guid createdCommentId);

    [LoggerMessage(EventId = LogEvents.CommentWasNotCreatedId, EventName = LogEvents.CommentWasNotCreatedName,
        Level = LogLevel.Error,
        Message = "Comment was not created",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasNotCreated(this ILogger logger);

    [LoggerMessage(EventId = LogEvents.CommentWasNotDeletedId, EventName = LogEvents.CommentWasNotDeletedName,
        Level = LogLevel.Error,
        Message = "Comment object with id {DeleteCommentId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasNotDeleted(this ILogger logger, Guid deleteCommentId);

    [LoggerMessage(EventId = LogEvents.CommentWasDeletedId, EventName = LogEvents.CommentWasDeletedName,
        Level = LogLevel.Information,
        Message = "Comment object with id {DeletedCommentId} was deleted",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasDeleted(this ILogger logger, Guid deletedCommentId);

    [LoggerMessage(EventId = LogEvents.CommentWasNotUpdatedId, EventName = LogEvents.CommentWasNotUpdatedName,
        Level = LogLevel.Error,
        Message = "Comment with id {FailedUpdateCommentId} was not updated",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasNotUpdated(this ILogger logger, Guid failedUpdateCommentId);

    [LoggerMessage(EventId = LogEvents.CommentWasUpdatedId, EventName = LogEvents.CommentWasUpdatedName,
        Level = LogLevel.Information,
        Message = "Comment with id {UpdatedCommentId} was updated",
        SkipEnabledCheck = true)]
    public static partial void LogCommentWasUpdated(this ILogger logger, Guid updatedCommentId);
}