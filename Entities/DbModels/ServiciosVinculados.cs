using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tges_serv_contratados")]
    public class ServiciosVinculados
    {
        [Key]
        [Column("CRR_IDSERVICIOSCONTRATADOS")]
        public int id { get; set; }
        
        [Column("CRR_CLIENTE")]
        public int idCliente { get; set; }
        [Column("CRR_SERVICIOS")]
        public int idServicios { get; set; }
        [Column("URL_SERVICIO")]
        public string urlServicio{ get; set; }
        [Column("FLG_ESTADO")]
        public bool estado { get; set; }

    }
}
