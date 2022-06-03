﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Data.Entities
{
    [Table("Radio", Schema = "Production")]
    public class RadioDb
    {
        [Key]
        [Column("RadioID")]
        public int RadioId { get; set; }
        [Column("RadioModelID")]
        public int RadioModelId { get; set; }

        public AssemblyDb Assembly { get; set; }
        public SimDb Sim { get; set; }
        public RadioModelDb RadioModel { get; set; }
    }
}
