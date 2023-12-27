using System;
using Microsoft.Extensions.Logging;

namespace Blog.Core.Logging;

public static partial class LogCategoryMessage
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Category object with id {GetByIdCategoryId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWithIdDoesNotExist(this ILogger logger, Guid getByIdCategoryId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Category with id {CreatedCategoryId} was created",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasCreated(this ILogger logger, Guid createdCategoryId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Category wasn't created",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasNotCreated(this ILogger logger);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Category object with id {DeleteCategoryId} doesn't exist",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasNotDeleted(this ILogger logger, Guid deleteCategoryId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Category object with id {DeletedCategoryId} was deleted",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasDeleted(this ILogger logger, Guid deletedCategoryId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Error,
        Message = "Category with id {FailedUpdateCategoryId} was not updated",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasNotUpdated(this ILogger logger, Guid failedUpdateCategoryId);
    
    [LoggerMessage(EventId = 0, Level = LogLevel.Information,
        Message = "Category with id {UpdatedCategoryId} was updated",
        SkipEnabledCheck = true)]
    public static partial void LogCategoryWasUpdated(this ILogger logger, Guid updatedCategoryId);
}