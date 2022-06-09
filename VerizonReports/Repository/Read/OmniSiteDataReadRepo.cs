using Microsoft.EntityFrameworkCore;
using VerizonReports.Logic.Interfaces;
using VerizonReports.Logic.OutboundPorts;
using VerizonReports.Repository.Models;

namespace VerizonReports.Repository.Read;

public class OmniSiteDataReadRepo : IOmniSiteDataReadRepo
{
    private readonly DbContextOptions<OmniSiteDbContext> _dbContextOptions;

    public OmniSiteDataReadRepo(DbContextOptions<OmniSiteDbContext> dbContextOptions) => _dbContextOptions = dbContextOptions;

    public async Task<List<IOmniSiteData>> ReadOmniSiteDataAsync()
    {
        await using var context = new OmniSiteDbContext(_dbContextOptions);

        var omniSiteDataResult = from unit in context.Unit.AsNoTracking()
            orderby unit.UnitId
            select new OmniSiteData
            {
                StationId = unit.Station.StationId,
                UnitId = unit.UnitId,
                SimNumber = unit.Assembly.Sim.SimNumber,
                ActiveFlag = unit.Assembly.Sim.ActiveFlag,
                ProviderActiveFlag = unit.Assembly.Sim.ActiveWithProviderFlag,
                Msisdn = unit.Assembly.Sim.Msisdn,
                Protocol = unit.Assembly.Radio.RadioModel.RadioProtocol.RadioProtocolName,
                Provider = unit.Assembly.Sim.ServiceProvider.ProviderName
            };
        return new List<IOmniSiteData>(omniSiteDataResult);
    }
}