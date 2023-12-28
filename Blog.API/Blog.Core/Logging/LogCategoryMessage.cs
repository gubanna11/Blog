using System;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Logging;

public static partial class LogCategoryMessage
{
    [LoggerMessage(EventId = LogEvents.CategoryWasNotGottenId, EventName = LogEvents.CategoryWasNotGottenName,
        Level = LogLevel.Error,
        Message = "Category object with id {GetByIdCategoryId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWithIdDoesNotExist(this ILogger logger, Guid getByIdCategoryId);

    [LoggerMessage(EventId = LogEvents.CategoryWasCreatedId, EventName = LogEvents.CategoryWasCreatedName,
        Level = LogLevel.Information,
        Message = "Category with id {CreatedCategoryId} was created",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasCreated(this ILogger logger, Guid createdCategoryId);

    [LoggerMessage(EventId = LogEvents.CategoryWasNotCreatedId, EventName = LogEvents.CategoryWasNotCreatedName,
        Level = LogLevel.Error,
        Message = "Category wasn't created",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasNotCreated(this ILogger logger);

    [LoggerMessage(EventId = LogEvents.CategoryWasNotDeletedId, EventName = LogEvents.CategoryWasNotDeletedName,
        Level = LogLevel.Error,
        Message = "Category object with id {DeleteCategoryId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasNotDeleted(this ILogger logger, Guid deleteCategoryId);

    [LoggerMessage(EventId = LogEvents.CategoryWasDeletedId, EventName = LogEvents.CategoryWasDeletedName,
        Level = LogLevel.Information,
        Message = "Category object with id {DeletedCategoryId} was deleted",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasDeleted(this ILogger logger, Guid deletedCategoryId);

    [LoggerMessage(EventId = LogEvents.CategoryWasNotUpdatedId, EventName = LogEvents.CategoryWasNotUpdatedName,
        Level = LogLevel.Error,
        Message = "Category with id {FailedUpdateCategoryId} was not updated",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasNotUpdated(this ILogger logger, Guid failedUpdateCategoryId);

    [LoggerMessage(EventId = LogEvents.CategoryWasUpdatedId, EventName = LogEvents.CategoryWasUpdatedName,
        Level = LogLevel.Information,
        Message = "Category with id {UpdatedCategoryId} was updated",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasUpdated(this ILogger logger, Guid updatedCategoryId);
}