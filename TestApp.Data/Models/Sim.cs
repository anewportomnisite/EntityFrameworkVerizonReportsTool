using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Repository.Models;

public class Sim : ISim
{
    public int SimId { get; set; }
    public string SimNumber { get; set; }
    public bool ActiveFlag { get; set; }
    public bool ActiveWithProviderFlag { get; set; }
    public string? Msisdn { get; set; }
    public int ServiceId { get; set; }
}