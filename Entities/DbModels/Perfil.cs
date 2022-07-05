using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities.DbModels
{
    [Table("tg_perfil")]
    public class Perfil
    {
        [Key]
        [Column("CRR_IDPERFIL")]
        public int id { get; set; }
        [Column("COD_PERFIL")]
        public string codigoPerfil { get; set; }
        [Column("DESC_PERFIL")]
        public string descripcionPerfil { get; set; }
        [Column("CRR_SECTOR")]
        public int? codigoSector { get; set; }
        [Column("CRR_SUBSECTOR")]
        public int? codigoSubSector { get; set; }
        [Column("FECHA_VIGENCIA")]
        public DateTime? fechaVigencia { get; set; }
        [Column("CRR_IDAGRUP_PERFIL")]
        public int? idGrupoPerfil { get; set; }
        [Column("FLG_VIGENCIA")]
        public int? flgVigencia { get; set; }
        [Column("FLG_CHILEVALORA")]
        public int? flgChileValora { get; set; }
        [Column("FLG_HOMOLOGADO")]
        public int? flgHomologado { get; set; }
    }
}
