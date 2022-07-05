using Contracts.Generic;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITablero_brecha : IRepositoryBase<Tablero_brecha>
    {

        List<Tablero_brecha> getCantidadTableroBrecha(int idCliente);

    }
}
