using Microsoft.Extensions.Compliance.Classification;

namespace SharedKernel;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class EUIIDataAttribute : DataClassificationAttribute
{
    public EUIIDataAttribute()
        : base(ApplicationLoggingTaxonomy.EUIIDataClassification) { }
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class EUPIDataAttribute : DataClassificationAttribute
{
    public EUPIDataAttribute()
        : base(ApplicationLoggingTaxonomy.EUPIDataClassification) { }
}
