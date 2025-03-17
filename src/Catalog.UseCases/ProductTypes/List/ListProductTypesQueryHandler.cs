using Catalog.Core.CatalogAggregate;
using Catalog.Core.CatalogAggregate.Specifications;

namespace Catalog.UseCases.ProductTypes.List;

public sealed class ListProductTypesQueryHandler : IQueryHandler<ListProductTypesQuery, IEnumerable<ProductTypeDto>>
{
    private readonly IProductTypeRepository _productTypeRepository;

    public ListProductTypesQueryHandler(IProductTypeRepository productRepository)
    {
        _productTypeRepository = productRepository;
    }

    public async Task<Result<IEnumerable<ProductTypeDto>>> Handle(
        ListProductTypesQuery request,
        CancellationToken cancellationToken)
    {
        var productTypes = await _productTypeRepository.ListAsync(
            new GetProductTypesSpecification(),
            ProductTypeMapper.ToProductTypeDto,
            cancellationToken);

        return Result.Success(productTypes);
    }
}
