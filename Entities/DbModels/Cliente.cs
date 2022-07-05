using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tg_clientes")]    
    public class Cliente
    {
        [Key]
        [Column("CRR_IDCLIENTE")]
        public int id { get; set; }

        [Column("RUN_CLIENTE")]
        public int? run { get; set; }
        
        [Column("DIG_CLIENTE")]
        public string digito { get; set; }
        
        [Column("NOMBRE_CLIENTE")]
        public string nombre { get; set; }
        
        [Column("NOMBRE_FANTASIA")]
        public string? nombreFantasia { get; set; }

        [Column("NIVEL_CLIENTE")]
        public int? nivel { get; set; }


        //FKs
        [ForeignKey("id")]
        public ServiciosVinculados? serviciosVinculados { get; set; }

  
    }
}
