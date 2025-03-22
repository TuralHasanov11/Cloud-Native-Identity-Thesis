using Audit;
using Outbox;

namespace Webhooks.Infrastructure.Data;

public sealed class WebhooksDbContext(DbContextOptions<WebhooksDbContext> options) : DbContext(options)
{
    public DbSet<WebhookSubscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("webhooks");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WebhooksDbContext).Assembly);

        modelBuilder.UseOutbox();

        modelBuilder.UseAudit();
    }
}
