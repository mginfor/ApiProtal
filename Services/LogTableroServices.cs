using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Generic;
using System.Threading.Tasks;
using System;

namespace Services
{
    public class LogTableroServices : RepositoryBase<Log_Tablero>, ILogTableroService
    {

        private readonly ILogService _logService;
        private readonly RepositoryContext _repositoryContext;
        public LogTableroServices(RepositoryContext repositoryContext, ILogService logService)
        : base(repositoryContext)
        {
            _logService = logService;
            _repositoryContext = repositoryContext;
        }


      


        public async Task LogEvent(LogTableroResponse logModel)
        {
            var logTablero = ConvertToLogTablero(logModel);
            _repositoryContext.LogTablero.Add(logTablero);
            await _repositoryContext.SaveChangesAsync();
        }

        private Log_Tablero ConvertToLogTablero(LogTableroResponse response)
        {
            return new Log_Tablero
            {
                UsuarioId = response.usuarioId,
                NombreUsuario = response.nombreUsuario,
                IdCliente = response.idCliente,
                Cargo = response.cargo,
                Correo = response.correo,
                Zona = response.zona,
                UrlServicio = response.urlServicio,
                TipoNivelCliente = response.tipoNivelCliente,
                NombreCliente = response.nombreCliente,
                Fecha = DateTimeOffset.UtcNow
            };
        }

    }
}
