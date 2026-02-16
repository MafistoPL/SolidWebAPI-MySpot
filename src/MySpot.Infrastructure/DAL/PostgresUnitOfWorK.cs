namespace MySpot.Infrastructure.DAL;

internal sealed class PostgresUnitOfWorK(MySpotDbContext dbContext) : IUnitOfWork
{
    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}