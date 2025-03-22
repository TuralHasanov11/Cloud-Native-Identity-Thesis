namespace Catalog.UseCases.Brands.List;

public sealed class ListBrandsQueryHandler(IBrandRepository brandRepository)
    : IQueryHandler<ListBrandsQuery, IEnumerable<BrandDto>>
{
    public async Task<Result<IEnumerable<BrandDto>>> Handle(
        ListBrandsQuery request,
        CancellationToken cancellationToken)
    {
        var brands = await brandRepository.ListAsync(
            new GetBrandsSpecification(),
            p => p.ToBrandDto(),
            cancellationToken);

        return Result.Success(brands);
    }
}
