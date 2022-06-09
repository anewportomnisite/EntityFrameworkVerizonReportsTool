using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Logic.Queries;

//not used yet
public interface IOmniSiteDataReadLogic
{
    Task<List<IOmniSiteData>> GetOmniSiteDataAsync();
}