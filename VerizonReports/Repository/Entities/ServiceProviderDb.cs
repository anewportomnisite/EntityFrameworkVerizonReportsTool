using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VerizonReports.Repository.Entities;

[Table("ServiceProvider", Schema = "Purchasing")]
public class ServiceProviderDb
{
    [Key]
    [Column("ServiceID")]
    public int ServiceProviderId { get; set; }
    [Column("Company")]
    public string ProviderName { get; set; }

    public ICollection<SimDb> Sims { get; set; }
}