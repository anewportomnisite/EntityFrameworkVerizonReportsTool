namespace VerizonReports.Models;

public class VerizonMatchData : VerizonData
{
    public bool? HasMatch { get; set; }
    public override string ToString()
    {
        return Mdn + "," + Meid + "," + LastDateActivated + "," + AccountName + "," + HasMatch;
    }
}