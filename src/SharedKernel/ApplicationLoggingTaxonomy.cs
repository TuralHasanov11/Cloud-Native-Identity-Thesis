using Microsoft.Extensions.Compliance.Classification;

namespace SharedKernel;

public static class ApplicationLoggingTaxonomy
{
    public static DataClassification EUIIDataClassification => new("EUIIDataTaxonomy", "EUIIData");

    public static DataClassification EUPDataClassification => new("EUPDataTaxonomy", "EUPData");
}
