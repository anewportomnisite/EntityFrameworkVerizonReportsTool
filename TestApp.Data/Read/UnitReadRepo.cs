using Microsoft.EntityFrameworkCore;

namespace TestApp.Data.Read
{
    public class UnitReadRepo : IUnitReadRepo
    {
        private readonly DbContextOptions<OmniSiteDbContext> _dbContextOptions;

        public UnitReadRepo(DbContextOptions<OmniSiteDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<IUnit> GetUnitAsync(string identifier)
        {
            await using var context = new OmniSiteDbContext(_dbContextOptions);

            var unit = await context.Unit.AsNoTracking().FirstOrDefaultAsync(x => x.Identifier == identifier);

            return Map(unit);
        }

        public async Task<List<IUnit>> GetUnitsForCacheAsync()
        {
            await using var context = new OmniSiteDbContext(_dbContextOptions);

            var units = await context.Unit.AsNoTracking().ToListAsync();

            return units.Select(Map).ToList();
        }

        private IUnit Map(UnitDb dbModel)
        {
            if (dbModel == null)
            {
                return null;
            }

            return new Unit
            {
                Identifier = dbModel.Identifier,
                UnitId = dbModel.UnitId
            };
        }
    }
}