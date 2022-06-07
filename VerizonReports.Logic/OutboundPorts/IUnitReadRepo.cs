using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Logic.OutboundPorts;

public interface IUnitReadRepo
{
    Task<List<IUnit>> ReadUnitsAsync();
}