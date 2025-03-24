namespace Catalog.UseCases.Products.DeleteById;

public sealed class DeleteByIdCommandHandler(
    IProductRepository productRepository)
    : ICommandHandler<DeleteProductByIdCommand>
{
    public async Task<Result> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(new ProductId(request.Id)),
            cancellationToken);

        if (product == null)
        {
            return Result.NotFound($"Item with id {request.Id} not found.");
        }

        productRepository.Delete(product);

        await productRepository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
