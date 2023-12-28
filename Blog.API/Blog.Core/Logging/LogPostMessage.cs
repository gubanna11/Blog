using System;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Logging;

public static partial class LogPostMessage
{
    [LoggerMessage(EventId = LogEvents.PostWasNotGottenId, EventName = LogEvents.PostWasNotGottenName,
        Level = LogLevel.Error,
        Message = "Post object with id {GetByIdPostId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogPostWithIdDoesNotExist(this ILogger logger, Guid getByIdPostId);

    [LoggerMessage(EventId = LogEvents.PostWasCreatedId, EventName = LogEvents.PostWasCreatedName,
        Level = LogLevel.Information,
        Message = "Post with id {CreatedPostId} was created",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasCreated(this ILogger logger, Guid createdPostId);

    [LoggerMessage(EventId = LogEvents.PostWasNotCreatedId, EventName = LogEvents.PostWasNotCreatedName,
        Level = LogLevel.Error,
        Message = "Post wasn't created",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasNotCreated(this ILogger logger);

    [LoggerMessage(EventId = LogEvents.PostWasNotDeletedId, EventName = LogEvents.PostWasNotDeletedName,
        Level = LogLevel.Error,
        Message = "Post object with id {DeletePostId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasNotDeleted(this ILogger logger, Guid deletePostId);

    [LoggerMessage(EventId = LogEvents.PostWasDeletedId, EventName = LogEvents.PostWasDeletedName,
        Level = LogLevel.Information,
        Message = "Post object with id {DeletedPostId} was deleted",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasDeleted(this ILogger logger, Guid deletedPostId);

    [LoggerMessage(EventId = LogEvents.PostWasNotUpdatedId, EventName = LogEvents.PostWasNotUpdatedName,
        Level = LogLevel.Error,
        Message = "Post with id {FailedUpdatePostId} was not updated",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasNotUpdated(this ILogger logger, Guid failedUpdatePostId);

    [LoggerMessage(EventId = LogEvents.PostWasUpdatedId, EventName = LogEvents.PostWasUpdatedName,
        Level = LogLevel.Information,
        Message = "Post with id {UpdatedPostId} was updated",
        SkipEnabledCheck = true)]
    public static partial void LogPostWasUpdated(this ILogger logger, Guid updatedPostId);
}