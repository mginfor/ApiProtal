using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tg_candidato")]
    public class Candidato
    {
        [Key]
        [Column("CRR_IDCANDIDATO")]
        public int id { get; set; }

        [Column("RUN_CANDIDATO")]
        public int run { get; set; }

        [Column("DIG_CANDIDATO")]
        public string dv { get; set; }

        [Column("EMAIL_CANDIDATO")]
        public string? correo { get; set; }

        [Column("PASS_BRECHA")]
        public string passBrecha { get; set; }

    }
}
