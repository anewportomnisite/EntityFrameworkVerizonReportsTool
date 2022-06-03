namespace TestApp.Domain;

public class VerizonData
{
    public string? Mdn { get; set; }
    public string? Meid { get; set; }
    public string? LastDateActivated { get; set; }
    public string? AccountName { get; set; }

    public override string ToString()
    {
        return Mdn + "," + Meid + "," + LastDateActivated + "," + AccountName;
    }
}