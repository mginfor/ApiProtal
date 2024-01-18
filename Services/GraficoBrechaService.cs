using Contracts;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class GraficoBrechaService : RepositoryBase<Grafico_Brecha>, IGraficoBrechaService
    {

        private RepositoryContext db;

        public GraficoBrechaService(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            db = new RepositoryContext();
        }

        public List<Grafico_Brecha> getCantidadOperadoresBrechasPerfil(int idCliente)
        {
            var query = "SELECT " +
                        "Desc_Perfil as PERFIL, " +
                        "COUNT(DISTINCT(run_operador)) operadores_brechas, " +
                        "crr_cliente " +
                        "FROM v_pbi_brechas vpb " +
                        "JOIN tg_perfil p ON vpb.crr_perfil = p.crr_idperfil " +
                        "WHERE vpb.crr_perfil = p.crr_idperfil " +
                        "and vpb.crr_cliente = {0} " +
                        "GROUP BY crr_cliente, Desc_Perfil " +
                        "order by 2 " +
                        "desc limit 10; ";


            return db.graficoBrecha
                .FromSqlRaw(query, idCliente)
                .ToList();
        }


      
    }
}
