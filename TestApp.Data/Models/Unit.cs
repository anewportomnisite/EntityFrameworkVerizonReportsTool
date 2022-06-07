using VerizonReports.Logic.Interfaces;

namespace VerizonReports.Repository.Models;

public class Unit : IUnit
{
    public int UnitId { get; set; }
    public int AssemblyId { get; set; }
}