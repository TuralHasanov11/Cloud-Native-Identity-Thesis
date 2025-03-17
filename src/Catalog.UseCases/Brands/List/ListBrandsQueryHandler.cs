using Catalog.Core.CatalogAggregate;
using Catalog.Core.CatalogAggregate.Specifications;

namespace Catalog.UseCases.Brands.List;

public sealed class ListBrandsQueryHandler : IQueryHandler<ListBrandsQuery, IEnumerable<BrandDto>>
{
    private readonly IBrandRepository _brandRepository;

    public ListBrandsQueryHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<Result<IEnumerable<BrandDto>>> Handle(
        ListBrandsQuery request,
        CancellationToken cancellationToken)
    {
        var brands = await _brandRepository.ListAsync(
            new GetBrandsSpecification(),
            BrandMapper.ToBrandDto,
            cancellationToken);

        return Result.Success(brands);
    }
}
