using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Repository.Models;

public class Radio : IRadio {
    public int RadioId { get; set; }
    public int RadioModelId { get; set; }
    public int RadioProtocolId { get; set; }
    public string ProtocolName { get; set; }
}