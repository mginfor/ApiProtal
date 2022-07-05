using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("usuarios")]
    public class UsuarioSecomLab
    {

        [Key]
        [Column("id_usuario")]
        public int id { get; set; }

        [Required(ErrorMessage = "Nombre de Persona Requerido")]
        [Column("nombre_usuario")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Nombre de Usuario Requerido")]
        [Column("usuario")]
        public string user { get; set; }

        [Required(ErrorMessage = "Contraseña de Usuario Requerido")]
        [Column("pass")]
        public string pass { get; set; }

        [Column("email")]
        public string correo { get; set; }

        [Required(ErrorMessage = "Nivel de seguridad es Requerido")]
        [Column("nivel_seguridad")]
        public int nivelSeguridad { get; set; }

        [Column("flg_revisor")]
        public bool esRevisor { get; set; }

        [Column("flg_asigna_auto")]
        public bool asignaAuto { get; set; }

        [Column("flg_carga_trabajo")]
        public bool cargaTrabajo { get; set; }

        [Column("flg_despedido")]
        public bool esDespedido { get; set; }

        [Column("flg_recoverypass")]
        public bool recoveryPass { get; set; }

        //FKs
        [ForeignKey("nivelSeguridad")]
        public  NivelUsuario nivelUsuario { get; set; }
    }
}
