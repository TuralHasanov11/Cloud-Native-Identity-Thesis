namespace Catalog.UseCases.ProductTypes.List;

public sealed class ListProductTypesQueryHandler(IProductTypeRepository productTypeRepository)
    : IQueryHandler<ListProductTypesQuery, IEnumerable<ProductTypeDto>>
{
    public async Task<Result<IEnumerable<ProductTypeDto>>> Handle(
        ListProductTypesQuery request,
        CancellationToken cancellationToken)
    {
        var productTypes = await productTypeRepository.ListAsync(
            new GetProductTypesSpecification(),
            p => p.ToProductTypeDto(),
            cancellationToken);

        return Result.Success(productTypes);
    }
}
