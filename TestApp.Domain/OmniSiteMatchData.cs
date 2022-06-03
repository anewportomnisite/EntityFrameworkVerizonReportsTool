namespace TestApp.Domain
{
    public class OmniSiteMatchData
    {
        public string? StationId { get; set; }
        public string? UnitId { get; set; }
        public string? SimNumber { get; set; }
        public string? Msisdn { get; set; }
        public bool? HasMatch { get; set; }
        public override string ToString()
        {
            return StationId + "," + UnitId + "," + SimNumber + "," + Msisdn + "," + HasMatch;
        }
    }
}
