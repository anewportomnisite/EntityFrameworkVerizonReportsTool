using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Data.Entities
{
    [Table("ServiceProvider", Schema = "Purchasing")]
    public class ServiceProviderDb
    {
        [Key]
        [Column("ServiceID")]
        public int ServiceProviderId { get; set; }
        [Column("Company")]
        public string Provider { get; set; }

        public ICollection<SimDb> Sims { get; set; }
    }
}