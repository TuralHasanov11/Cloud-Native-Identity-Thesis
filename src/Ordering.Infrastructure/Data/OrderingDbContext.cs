using Audit;
using Outbox;

namespace Ordering.Infrastructure.Data;

public class OrderingDbContext(DbContextOptions<OrderingDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<CardType> CardTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderingDbContext).Assembly);

        modelBuilder.UseOutbox();

        modelBuilder.UseAudit();
    }
}
