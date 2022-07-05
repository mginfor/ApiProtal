using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class UsuariosNivel
    {
        public UsuarioSecomLab user { get; set; }
        public NivelUsuario nivel { get; set; }
    }
}
