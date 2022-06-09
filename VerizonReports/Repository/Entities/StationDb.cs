using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerizonReports.Repository.Entities;

[Table("Station", Schema = "GuardDog")]
public class StationDb
{
    [Key]
    [Column("StationID")]
    public int StationId { get; set; }
    [Column("UnitID")]
    public int UnitId { get; set; }

    public UnitDb Unit { get; set; }
}