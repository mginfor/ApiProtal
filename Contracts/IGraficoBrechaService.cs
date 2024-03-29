﻿using Contracts.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DbModels;

namespace Contracts
{
    public interface IGraficoBrechaService : IRepositoryBase<Grafico_Brecha>
    {

        List<Grafico_Brecha> getCantidadOperadoresBrechasPerfil(int idCliente);
    }
}
