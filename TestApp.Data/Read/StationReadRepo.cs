using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using VerizonReports.Logic.Interfaces;
using VerizonReports.Logic.OutboundPorts;
using VerizonReports.Repository.Entities;
using VerizonReports.Repository.Models;

namespace VerizonReports.Repository.Read;

public class StationReadRepo : IStationReadRepo
{
    private readonly DbContextOptions<OmniSiteDbContext> _dbContextOptions;

    public StationReadRepo(DbContextOptions<OmniSiteDbContext> dbContextOptions)
    {
        _dbContextOptions = dbContextOptions;
    }

    public async Task<List<IStation>> ReadStationsAsync(List<IUnit> units)
    {
        await using var context = new OmniSiteDbContext(_dbContextOptions);
        var stationList = new List<StationDb>();

        foreach (var unit in units)
        {
            stationList = await context.Station.AsNoTracking()
                .Where(x => x.UnitId == unit.UnitId)
                .ToListAsync();
        }

        return Map(stationList);
    }
    private static List<IStation> Map(List<StationDb> dbModel)
    {
        return dbModel.Select(stationDb => new Station
        {
            StationId = stationDb.StationId,
            UnitId = stationDb.UnitId
        }).Cast<IStation>().ToList();
    }
}