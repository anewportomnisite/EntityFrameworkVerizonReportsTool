using Microsoft.EntityFrameworkCore;
using VerizonReports.Logic.Interfaces;
using VerizonReports.Logic.OutboundPorts;
using VerizonReports.Repository.Entities;
using VerizonReports.Repository.Models;

namespace VerizonReports.Repository.Read;

public class SimReadRepo : ISimReadRepo
{
    private readonly DbContextOptions<OmniSiteDbContext> _dbContextOptions;

    public SimReadRepo(DbContextOptions<OmniSiteDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }
    public async Task<List<ISim>> ReadSimsAsync(List<IUnit> units)
    {
        await using var context = new OmniSiteDbContext(_dbContextOptions);
        var stationList = new List<SimDb>();

        foreach (var unit in units)
        {
            stationList = await context.Sim.AsNoTracking()
                .Where(sims => sims.SimId == sims.Assembly.SimId)
                .ToListAsync();
        }

        return Map(stationList);
    }
    private static List<ISim> Map(List<SimDb> dbModel)
    {
        return dbModel.Select(simDb => new Sim 
        { 
            SimId = simDb.SimId,
            SimNumber = simDb.SimNumber,
            ActiveFlag = simDb.ActiveFlag,
            ActiveWithProviderFlag = simDb.ActiveWithProviderFlag,
            Msisdn = simDb.Msisdn,
            ServiceId = simDb.ServiceId
        }).Cast<ISim>().ToList();
    }
}