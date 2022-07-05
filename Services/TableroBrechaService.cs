using Contracts;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;


namespace Services
{
    public class  TableroBrechaService: RepositoryBase<Tablero_brecha>, ITablero_brecha
    {
        private RepositoryContext db;


        public TableroBrechaService(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            db = new RepositoryContext();

        }

        public List<Tablero_brecha> getCantidadTableroBrecha(int idCliente)
        { 
            var query = "SELECT " +
                "pp.GLS_BRECHA AS brecha, " +
                "COUNT(DISTINCT(vpb.RUN_OPERADOR)) AS Q_OP " +
                "FROM v_pbi_brechas vpb, tcnf_pool_preguntas pp " +
                "WHERE vpb.CRR_IDPOOLPREGUNTA = pp.CRR_IDPOOLPREGUNTA " +
                "AND vpb.FLG_BRECHA <> 0 " +
                "AND vpb.crr_cliente = {0} " +
                "GROUP BY pp.GLS_BRECHA " +
                "ORDER BY 2 desc " +
                "limit 10; "; 

            return db.tablero_Brechas
                .FromSqlRaw(query, idCliente)
                .ToList();
        }

    }
}
