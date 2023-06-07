using Contracts.Generic;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPermisosServices: IRepositoryBase<Permisos>
    {
        public List<Permisos> GetAllPermisos();

        public Permisos GetPermisosByid(int id);

    }
}
