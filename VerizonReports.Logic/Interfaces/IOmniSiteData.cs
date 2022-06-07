namespace VerizonReports.Logic.Interfaces;

public interface IOmniSiteData
{
    int? StationId { get; set; }
    int UnitId { get; set; }
    string? SimNumber { get; set; }
    string? Msisdn { get; set; }
    string? Protocol { get; set; }
    string? Provider { get; set; }
    bool? ProviderActiveFlag { get; set; }
    bool? ActiveFlag { get; set; }
}