using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("nivel_usuarios")]
    public class NivelUsuario
    {
        [Key]
        [Column("id_nivel")]
        public int id { get; set; }

        [Column("nivel_usuario")]
        [Required(ErrorMessage = "Este Campo no puede estar vacio")]
        [StringLength(255, ErrorMessage = "Este Campo no puede superar los 255 caracteres")]
        public string descripcion { get; set; }
    }
}
