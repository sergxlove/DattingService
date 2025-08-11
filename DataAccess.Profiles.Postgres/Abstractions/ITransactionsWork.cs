namespace DataAccess.Profiles.Postgres.Abstractions
{
    public interface ITransactionsWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
