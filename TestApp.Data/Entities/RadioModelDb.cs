using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Data.Entities
{
    [Table("RadioModel", Schema = "Production")]
    public class RadioModelDb
    {
        [Key]
        [Column("RadioModelID")]
        public int RadioModelId { get; set; }
        [Column("RadioProtocolID")]
        public int RadioProtocolId { get; set; }

        public ICollection<RadioDb> Radios { get; set; }
        public RadioProtocolDb RadioProtocol { get; set; }
    }
}