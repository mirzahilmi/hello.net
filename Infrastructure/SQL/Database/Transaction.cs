using Microsoft.EntityFrameworkCore;

namespace Hello.NET.Infrastructure.SQL.Database;

public sealed class Transaction(ApplicationDbContext context)
{
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        var strategy = context.Database.CreateExecutionStrategy();

        var result = await strategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                await context.Database.BeginTransactionAsync();

            try
            {
                var result = await action();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        });

        return result;
    }
}
