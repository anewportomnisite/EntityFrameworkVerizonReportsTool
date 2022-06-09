using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Logic.OutboundPorts;

public interface IOmniSiteDataReadRepo
{
    Task<List<IOmniSiteData>> ReadOmniSiteDataAsync();
}