using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Audit;

public class AuditTrailConsumer(DbContext dbContext) : IConsumer<AuditTrailMessage>
{
    private readonly DbContext _dbContext = dbContext;

    public async Task Consume(ConsumeContext<AuditTrailMessage> context)
    {
        _dbContext.Set<AuditEntry>().AddRange(context.Message.Entries);
        await _dbContext.SaveChangesAsync();
    }
}
