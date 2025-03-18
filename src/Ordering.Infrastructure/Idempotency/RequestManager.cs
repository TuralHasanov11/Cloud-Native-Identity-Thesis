using Ordering.Core.Exceptions;

namespace Ordering.Infrastructure.Idempotency;

public class RequestManager : IRequestManager
{
    private readonly OrderingDbContext _dbContext;

    public RequestManager(OrderingDbContext context)
    {
        _dbContext = context ?? throw new ArgumentNullException(nameof(context));
    }


    public async Task<bool> ExistAsync(Guid id)
    {
        var request = await _dbContext.
            FindAsync<ClientRequest>(id);

        return request != null;
    }

    public async Task CreateRequestForCommandAsync<T>(Guid id)
    {
        var exists = await ExistAsync(id);

        var request = exists ?
            throw new OrderingDomainException($"Request with {id} already exists") :
            new ClientRequest()
            {
                Id = id,
                Name = typeof(T).Name,
                Time = DateTime.UtcNow
            };

        _dbContext.Add(request);

        await _dbContext.SaveChangesAsync();
    }
}
