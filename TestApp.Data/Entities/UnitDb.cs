using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Data.Entities
{
    [Table("Unit", Schema = "Sales")]
    public class UnitDb
    {
        [Key]
        [Column("UnitID")]
        public int UnitId { get; set; }
        [Column("AssemblyID")]
        public int AssemblyId { get; set; }

        public StationDb Station { get; set; }
        public AssemblyDb Assembly { get; set; }
    }
}