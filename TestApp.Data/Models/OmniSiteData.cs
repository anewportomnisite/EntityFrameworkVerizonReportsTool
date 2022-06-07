using System.ComponentModel.DataAnnotations;
using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Repository.Models;

public class OmniSiteData : IOmniSiteData
{
    public int? StationId { get; set; }
    [Key]
    public int UnitId { get; set; }
    public string? SimNumber { get; set; }
    public string? Msisdn { get; set; }

    public string? Protocol { get; set; }
    public string? Provider { get; set; }
    public bool? ProviderActiveFlag { get; set; }
    public bool? ActiveFlag { get; set; }

    public override string ToString()
    {
        return StationId is null
            ? "No Station," + UnitId + "," + SimNumber + "," + Msisdn + "," + Protocol + "," + Provider + "," +
              ProviderActiveFlag + "," + ActiveFlag


            : StationId + "," + UnitId + "," + SimNumber + "," + Msisdn + "," + Protocol + "," + Provider + "," +
              ProviderActiveFlag + "," + ActiveFlag;
    }
}