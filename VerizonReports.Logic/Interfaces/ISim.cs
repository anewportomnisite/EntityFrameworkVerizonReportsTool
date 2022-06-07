namespace VerizonReports.Logic.Interfaces;

public interface ISim
{
    public int SimId { get; set; }
    public string SimNumber { get; set; }
    public bool ActiveFlag { get; set; }
    public bool ActiveWithProviderFlag { get; set; }
    string? Msisdn { get; set; }
    int ServiceId { get; set; }
}