using System.ComponentModel;

namespace Catalog.Api.Features.Products;

public static class DeleteById
{
    public static async Task<Results<NoContent, NotFound, ForbidHttpResult>> Handle(
        IProductRepository productRepository,
        [Description("The id of the catalog item to delete")] Guid id,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.SingleOrDefaultAsync(
            new GetProductByIdSpecification(new ProductId(id)),
            cancellationToken);

        if (product == null)
        {
            return TypedResults.NotFound();
        }

        productRepository.Delete(product);
        await productRepository.SaveChangesAsync(cancellationToken);

        return TypedResults.NoContent();
    }
}
