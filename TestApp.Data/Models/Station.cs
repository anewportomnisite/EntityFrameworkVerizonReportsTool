using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Repository.Models;

public class Station : IStation
{
    public int StationId { get; set; }
    public int UnitId { get; set; }
}