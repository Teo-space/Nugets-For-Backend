using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Infrastructure.EntityFrameworkCore.Interceptors;

public class NoLockInterceptor : DbCommandInterceptor
{
    private const string SET_READ_UNCOMMITED = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";

    private const string SET_READ_COMMITED = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        if (SkipNolock(command.CommandText))
        {
            return base.ReaderExecuting(command, eventData, result);
        }

        AddNolock(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, 
        InterceptionResult<DbDataReader> result, 
        CancellationToken cancellationToken = default)
    {
        if (SkipNolock(command.CommandText))
        {
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }

        AddNolock(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
    {
        if (SkipNolock(command.CommandText))
        {
            return base.ScalarExecuting(command, eventData, result);
        }

        AddNolock(command);
        return base.ScalarExecuting(command, eventData, result);
    }

    public override async ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, 
        InterceptionResult<object> result, 
        CancellationToken cancellationToken = default)
    {
        if (SkipNolock(command.CommandText))
        {
            return await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
        }

        AddNolock(command);
        return await base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }

    private static bool SkipNolock(string commandText)
    {
        if (!string.IsNullOrWhiteSpace(commandText))
        {
            if (    !commandText.Contains("INSERT INTO", StringComparison.OrdinalIgnoreCase) 
                &&  !commandText.Contains("UPDATE", StringComparison.OrdinalIgnoreCase))
            {
                return commandText.Contains("DELETE FROM", StringComparison.OrdinalIgnoreCase);
            }

            return true;
        }

        return false;
    }

    private static void AddNolock(DbCommand command)
    {
        string commandText = command.CommandText;
        command.CommandText = $"{"SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED"} {Environment.NewLine} {commandText} {Environment.NewLine} {"SET TRANSACTION ISOLATION LEVEL READ COMMITTED"}";
    }
}