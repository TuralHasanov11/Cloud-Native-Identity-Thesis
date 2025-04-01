using System.Data;
using Audit;
using Microsoft.EntityFrameworkCore.Storage;
using Outbox;

namespace Ordering.Infrastructure.Data;

public class OrderingDbContext(DbContextOptions<OrderingDbContext> options) : DbContext(options)
{
    private IDbContextTransaction _currentTransaction;

    public bool HasActiveTransaction => _currentTransaction != null;

    public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<CardType> CardTypes { get; set; }


    //public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    //{
    //    // Dispatch Domain Events collection.
    //    // Choices:
    //    // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including
    //    // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
    //    // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions.
    //    // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers.
    //    await _mediator.DispatchDomainEventsAsync(this);

    //    // After executing this line all the changes (from the Command Handler and Domain Event Handlers)
    //    // performed through the DbContext will be committed
    //    _ = await base.SaveChangesAsync(cancellationToken);

    //    return true;
    //}

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            return null;
        }

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        if (transaction != _currentTransaction)
        {
            throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");
        }

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingDbContext).Assembly);

        modelBuilder.UseOutbox();

        modelBuilder.UseAudit();
    }
}
