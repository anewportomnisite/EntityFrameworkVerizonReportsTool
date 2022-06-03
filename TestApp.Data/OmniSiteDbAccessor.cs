using TestApp.Domain;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace TestApp.Data;

public class OmniSiteDbAccessor
{
    private readonly string _connectionString;

    public OmniSiteDbAccessor(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<OmniSiteData>> GetDataAsync()
    {
        await using var conn = new SqlConnection(_connectionString);
        conn.Open();
        var query = new SqlCommand(GetQuery());
        query.Connection = conn;
        await using var reader = await query.ExecuteReaderAsync();

        var omniSiteData = new List<OmniSiteData>();
        while (reader.Read())
        {
            omniSiteData.Add(new OmniSiteData
            {
                StationId = reader[0].ToString(),
                UnitId = reader[1].ToString(),
                SimNumber = reader[2].ToString(),
                Msisdn = reader[3].ToString(),
                Protocol = reader[4].ToString(),
                Provider = reader[5].ToString(),
                ProviderActive = (bool)reader[6],
                ActiveFlag = (bool)reader[7]
            });
        }
        conn.Close();

        return omniSiteData;
    }

    private static string GetQuery()
    {
        return
            "SELECT GuardDog.Station.StationID, Sales.Unit.UnitID, Production.SIM.Number, Production.SIM.MSISDN, Production.RadioProtocol.Name, " +
            "Purchasing.ServiceProvider.Company, Production.SIM.ActiveWithProviderFlag, Production.SIM.ActiveFlag FROM Sales.Unit " +
            "LEFT JOIN GuardDog.Station ON GuardDog.Station.UnitID = Sales.Unit.UnitID " +
            "LEFT JOIN Production.Assembly ON Production.Assembly.AssemblyID = Sales.Unit.AssemblyID " +
            "LEFT JOIN Production.Radio ON Production.Radio.RadioID = Production.Assembly.RadioID " +
            "LEFT JOIN Production.RadioModel ON Production.RadioModel.RadioModelID = Production.Radio.RadioModelID " +
            "LEFT JOIN Production.RadioProtocol ON Production.RadioModel.RadioProtocolId = Production.RadioProtocol.RadioProtocolId " +
            "LEFT JOIN Production.SIM ON Production.SIM.SimID = Production.Assembly.SimID " +
            "LEFT JOIN Purchasing.ServiceProvider ON Purchasing.ServiceProvider.ServiceID = Production.SIM.ServiceID " +
            "WHERE Production.SIM.ServiceID = 5 AND MSISDN NOT IN('DEMO1', 'DEMO2', 'DEMO3', 'DEMO4', 'DEMO5')";
    }
}