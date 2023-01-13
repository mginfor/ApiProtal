using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
 
        [Table("tges_contrato")]
        public class Contrato
        {
            [Key]
            [Column("CRR_IDCONTRATO")]
            public int id { get; set; }  

            [Column("CRR_CLIENTE")]
            public int idCliente { get; set; }

            [Column("CRR_FAENA")]
             public int idFaena { get; set; }

         
        }
    }

