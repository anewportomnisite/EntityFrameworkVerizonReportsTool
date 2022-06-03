using System.ComponentModel.DataAnnotations;

namespace TestApp.Domain;

public class OmniSiteData
{
    public string? StationId { get; set; }
    public string? UnitId { get; set; }
    [Key]
    public string? SimNumber { get; set; }
    public string? Msisdn { get; set; }
    public string? Protocol { get; set; }
    public string? Provider { get; set; }
    public bool? ProviderActive { get; set; }
    public bool? ActiveFlag { get; set; }

    public override string ToString()
    {
        if (StationId is null or "")
        {
            StationId = "No Station";
        }
        return StationId + "," + UnitId + "," + SimNumber + "," + Msisdn + "," + Protocol + "," + Provider + "," + ProviderActive + "," + ActiveFlag;
    }
}