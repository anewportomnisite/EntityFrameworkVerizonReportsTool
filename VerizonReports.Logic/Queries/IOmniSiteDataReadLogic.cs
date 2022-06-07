using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Logic.Queries;

public interface IOmniSiteDataReadLogic
{
    Task<List<IOmniSiteData>> GetOmniSiteDataAsync();
}