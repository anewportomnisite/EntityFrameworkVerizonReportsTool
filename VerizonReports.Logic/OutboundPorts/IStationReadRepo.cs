using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Logic.OutboundPorts;

public interface IStationReadRepo
{
    Task<List<IStation>> ReadStationsAsync(List<IUnit> units);
}