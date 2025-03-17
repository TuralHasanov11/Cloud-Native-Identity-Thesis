using Catalog.Core.CatalogAggregate;
using Catalog.Core.CatalogAggregate.Specifications;

namespace Catalog.UseCases.Products.DeleteById;

public sealed class DeleteByIdCommandHandler : ICommandHandler<DeleteProductByIdCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteByIdCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(new ProductId(request.Id)),
            cancellationToken);

        if (product == null)
        {
            return Result.NotFound($"Item with id {request.Id} not found.");
        }

        _productRepository.Delete(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
