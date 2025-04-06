namespace Catalog.Core.CatalogAggregate;

public class CatalogException : Exception
{
    public CatalogException()
    { }

    public CatalogException(string message)
        : base(message)
    { }

    public CatalogException(string message, Exception innerException)
        : base(message, innerException)
    { }
}

public class BrandNameEmptyException : CatalogException
{
    public BrandNameEmptyException()
        : base("The brand name cannot be empty.") { }
}

public class BrandNameTooLongException(int actualLength, int allowedLength) 
    : CatalogException($"The brand name is too long. Actual length: {actualLength}, allowed length: {allowedLength}.");
