namespace VerizonReports.Models;

public class OmniSiteMatchData
{
    public int? StationId { get; set; }
    public int UnitId { get; set; }
    public string? SimNumber { get; set; }
    public string? Msisdn { get; set; }
    public bool? HasMatch { get; set; }
    public override string ToString()
    {
        return StationId + "," + UnitId + "," + SimNumber + "," + Msisdn + "," + HasMatch;
    }
}