using TestApp.Domain;
using System.Configuration;
using TestApp.Data;

namespace ConsoleApp;

public static class ConsoleApp
{
    private static async Task Main()
    {
        try
        {
            await GenerateReports();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: ");
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Console.WriteLine("The program has finished");
        }
    }

    private static List<OmniSiteMatchData> GetOmniSiteMatchData(List<OmniSiteData> omniSiteDataList, List<VerizonData> verizonDataList)
    {
        return omniSiteDataList.Select(omniSiteDataPoint => new OmniSiteMatchData
        {
            StationId = omniSiteDataPoint.StationId,
            UnitId = omniSiteDataPoint.UnitId,
            SimNumber = omniSiteDataPoint.SimNumber,
            Msisdn = omniSiteDataPoint.Msisdn,
            HasMatch = MatchCheck(omniSiteDataPoint, verizonDataList)
        }).ToList();
    }

    private static List<VerizonMatchData> GetVerizonMatchData(List<VerizonData> verizonDataList, List<OmniSiteData> omniSiteDataList)
    {
        return verizonDataList.Select(verizonDataPoint => new VerizonMatchData()
        {
            Mdn = verizonDataPoint.Mdn,
            Meid = verizonDataPoint.Meid,
            LastDateActivated = verizonDataPoint.LastDateActivated,
            AccountName = verizonDataPoint.AccountName,
            HasMatch = MatchCheck(verizonDataPoint, omniSiteDataList)
        }).ToList();
    }

    private static bool MatchCheck(OmniSiteData baseDataPoint, List<VerizonData> comparisonList)
    {
        return comparisonList.Any(verizonDataPoint => verizonDataPoint.Mdn == baseDataPoint.Msisdn);
    }
    private static bool MatchCheck(VerizonData baseDataPoint, List<OmniSiteData> comparisonList)
    {
        return comparisonList.Any(omniSiteDataPoint => omniSiteDataPoint.Msisdn == baseDataPoint.Mdn);
    }

    private static void WriteData<T>(List<T> dataList, string filePath)
    {
        using var fileWriter = File.CreateText(filePath);
        foreach (var row in dataList)
        {
            if (row != null) fileWriter.WriteLine(row.ToString());
        }
    }

    private static async Task GenerateReports()
    {
        var verizonApiAdapter = new VerizonApiAdapter();
        var verizonDataList = await verizonApiAdapter.PrintDictData();

        var connectionString = ConfigurationManager.ConnectionStrings["OmniSiteDB"].ConnectionString;

        var omniSiteDbAccessor = new OmniSiteDbAccessor(connectionString);
        var omniSiteDataList = await omniSiteDbAccessor.GetDataAsync();

        WriteData(omniSiteDataList, ConfigurationManager.AppSettings.Get("omniSiteDataPath")
                                    ?? throw new ArgumentNullException(connectionString,
                                        "No filepath for OmniSite Data"));

        WriteData(verizonDataList, ConfigurationManager.AppSettings.Get("verizonDataPath")
                                   ?? throw new ArgumentNullException(connectionString,
                                       "No filepath for Verizon Data"));

        WriteData(GetOmniSiteMatchData(omniSiteDataList, verizonDataList), ConfigurationManager.AppSettings.Get("omniSiteMatchDataPath")
                                                                           ?? throw new ArgumentNullException(connectionString,
                                                                               "No filepath for Match Data"));

        WriteData(GetVerizonMatchData(verizonDataList, omniSiteDataList), ConfigurationManager.AppSettings.Get("verizonMatchDataPath")
                                                                          ?? throw new ArgumentNullException(connectionString,
                                                                              "No filepath for Match Data"));
    }
}