using System.Configuration;
using Microsoft.EntityFrameworkCore;
using VerizonReports.Logic.Interfaces;
using VerizonReports.Logic.Queries;
using VerizonReports.Models;
using VerizonReports.Repository;
using VerizonReports.Repository.Read;

namespace VerizonReports;

public static class ConsoleApp
{
    private static async Task Main()
    {
        await GenerateReports();
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

    private static List<OmniSiteMatchData> GetOmniSiteMatchData(List<IOmniSiteData> omniSiteDataList, List<VerizonData> verizonDataList)
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

    private static List<VerizonMatchData> GetVerizonMatchData(List<VerizonData> verizonDataList, List<IOmniSiteData> omniSiteDataList)
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

    private static bool MatchCheck(IOmniSiteData baseDataPoint, List<VerizonData> comparisonList)
    {
        return comparisonList.Any(verizonDataPoint => verizonDataPoint.Mdn == baseDataPoint.Msisdn);
    }
    private static bool MatchCheck(VerizonData baseDataPoint, List<IOmniSiteData> comparisonList)
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
        var omniSiteDataList = await new OmniSiteDataReadRepo(new DbContextOptions<OmniSiteDbContext>()).ReadOmniSiteDataAsync();
        var verizonDataList = await new VerizonApiAdapter().PrintDictData();

        //I will move these file paths to app settings later
        WriteData(omniSiteDataList, @".\OmniSiteData.txt");
        WriteData(verizonDataList, @".\VerizonData.txt");
        WriteData(GetOmniSiteMatchData(omniSiteDataList, verizonDataList), @".\OmniSiteMatchData.txt");
        WriteData(GetVerizonMatchData(verizonDataList, omniSiteDataList), @".\VerizonMatchData.txt");
    }
}