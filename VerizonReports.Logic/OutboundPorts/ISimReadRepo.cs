using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Logic.OutboundPorts;

public interface ISimReadRepo
{
    Task<List<ISim>> ReadSimsAsync(List<IUnit> units);
}