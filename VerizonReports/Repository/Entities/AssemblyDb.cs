using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerizonReports.Repository.Entities;

[Table("Assembly", Schema = "Production")]
public class AssemblyDb
{
    [Key]
    [Column("AssemblyID")]
    public int AssemblyId { get; set; }
    [Column("RadioID")]
    public int RadioId { get; set; }
    [Column("SimID")]
    public int SimId { get; set; }

    public UnitDb Unit { get; set; }
    public RadioDb Radio { get; set; }
    public SimDb Sim { get; set; }
}