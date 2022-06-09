using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerizonReports.Repository.Entities;

[Table("RadioProtocol", Schema = "Production")]
public class RadioProtocolDb
{
    [Key]
    [Column("RadioProtocolID")]
    public int RadioProtocolId { get; set; }
    [Column("Name")]
    public string RadioProtocolName { get; set; }

    public ICollection<RadioModelDb> RadioModels { get; set; }
}