namespace Catalog.UseCases.Products.DeleteById;

public sealed record DeleteProductByIdCommand(Guid Id) : ICommand;
