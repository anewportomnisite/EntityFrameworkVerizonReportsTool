using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Data.Entities
{
    [Table("SIM", Schema = "Production")]
    public class SimDb
    {
        [Key]
        [Column("SimID")]
        public int SimId { get; set; }
        [Column("Number", TypeName = "varchar(20)")]
        public string SimNumber { get; set; }
        public bool ActiveFlag { get; set; }
        public bool ActiveWithProviderFlag { get; set; }
        [Column("MSISDN", TypeName = "char(10)")]
        public string Msisdn { get; set; }
        [Column("ServiceID")]
        public int ServiceId { get; set; }

        public AssemblyDb Assembly { get; set; }
        public RadioDb Radio { get; set; }
        public ServiceProviderDb ServiceProvider { get; set; }
    }
}
