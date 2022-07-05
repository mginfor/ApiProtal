using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tg_sector")]
    public class Sector
    {
        [Key]
        [Column("CRR_SECTOR")]
        public int id { get; set; }
        [Column("DESC_SECTOR")]
        public string descripcionSector { get; set; }
        [Column("FLG_VIGENCIA")]
        public int vigencia { get; set; }
    }
}
