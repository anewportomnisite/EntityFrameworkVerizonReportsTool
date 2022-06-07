using VerizonReports.Logic.Interfaces;
using VerizonReports.Logic.OutboundPorts;

namespace VerizonReports.Logic.Queries;

//Not used yet
public class OmniSiteDataReadLogic : IOmniSiteDataReadLogic
{
    private readonly IOmniSiteDataReadRepo _repo;

    public OmniSiteDataReadLogic(IOmniSiteDataReadRepo repo)
    {
        _repo = repo;
    }

    public async Task<List<IOmniSiteData>> GetOmniSiteDataAsync()
    {
        return await _repo.ReadOmniSiteDataAsync();
    }
}