using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Repository.Models;

public class Assembly : IAssembly
{
    public int AssemblyId { get; set; }
    public int RadioId { get; set; }
    public int SimId { get; set; }
}