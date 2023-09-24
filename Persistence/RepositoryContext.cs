using Entities.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class RepositoryContext : DbContext
    {
        private AppConfiguration _cadena;
        public RepositoryContext()
        {
            _cadena = new AppConfiguration();
        }

        public RepositoryContext(DbContextOptions options) : base(options)
        {
            _cadena = new AppConfiguration();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            _ = optionsBuilder.UseMySql(_cadena.ConnectionString, new MySqlServerVersion(new System.Version(8, 0, 25)));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Tablero
            modelBuilder.Entity<Empresas>().HasNoKey();
            modelBuilder.Entity<GraficoGeneral>().HasNoKey();
            modelBuilder.Entity<ProcesosLogros>().HasNoKey();
            modelBuilder.Entity<PersonasResultado>().HasNoKey();
            modelBuilder.Entity<PerfilBrechas>().HasNoKey();
            modelBuilder.Entity<CriterioGrafico>().HasNoKey();
            modelBuilder.Entity<LogroInstrumentos>().HasNoKey();
            modelBuilder.Entity<FaenaPerfilCandidato>().HasNoKey();
            modelBuilder.Entity<ProcesoLogroCandidato>().HasNoKey();
            modelBuilder.Entity<BrechaPortal>().HasNoKey();

            //Portal
            modelBuilder.Entity<ProcesoInforme>().HasNoKey();
            modelBuilder.Entity<Proceso>().HasNoKey();
            modelBuilder.Entity<ProcesoReportabilidad>().HasNoKey();
            modelBuilder.Entity<Grafico_Brecha>().HasNoKey();
            modelBuilder.Entity<Tablero_brecha>().HasNoKey();

            //Exportar
            modelBuilder.Entity<Exportar>().HasNoKey();
            modelBuilder.Entity<PerfilCandidato>().HasNoKey();
            modelBuilder.Entity<BrechasCandidato>().HasNoKey();

            //
            modelBuilder.Entity<Candidato>().HasNoKey();
        }
        #region Otros
        public DbSet<Perfil> Perfiles { get; set; }

        public DbSet<PerfilCandidato> PerfilesCandidato { get; set; }
        public DbSet<Sector> Sectores { get; set; }
        public DbSet<Exportar> Exportar { get; set; }
        #endregion

        #region Comunes
        public DbSet<Faena> Faenas { get; set; }
        public DbSet<Resultado> Resultados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<EstadoProceso> EstadoProcesos { get; set; }

        #endregion

        #region Portal
        public DbSet<Log> Logs { get; set; }
        public DbSet<UsuarioPortal> UsuarioPortals { get; set; }
        public DbSet<Proceso> ProcesoPortal { get; set; }

        public DbSet<ProcesoInforme> ProcesoPortalInforme { get; set; }
        public DbSet<ProcesoReportabilidad> ProcesoReportabilidadPortal { get; set; }
        public DbSet<BrechaPortal> BrechaPortal { get; set; }
        public DbSet<ServiciosVinculados> ServiciosVinculados { get; set; }
        public DbSet<Grafico_Brecha> graficoBrecha { get; set; }
        public DbSet<Tablero_brecha> tablero_Brechas { get; set; }

        public DbSet<BrechasCandidato> brechasCandidato { get; set; }



        #endregion

        #region Secomlab
        public DbSet<UsuarioSecomLab> usuarioSecomLabs { get; set; }
        public DbSet<NivelUsuario> nivelUsuarios { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
        public DbSet<EvaluacionPCT> EvaluacionesPCT { get; set; }
        public DbSet<EvaluacionPROT> EvaluacionesPROT { get; set; }
        public DbSet<EvaluacionEI> EvaluacionesEI { get; set; }
        public DbSet<EvaluacionPROT2> EvaluacionesPROT2 { get; set; }
        public DbSet<EvaluacionEJD> EvaluacionesEJD { get; set; }
        public DbSet<DocumentoBrecha> DocumentoBrechas { get; set; }

        #endregion

        #region TableroRegion
        public DbSet<Empresas> dataEmpresas { get; set; }
        public DbSet<GraficoGeneral> dataGraficos { get; set; }
        public DbSet<ProcesosLogros> dataProcesosLogros { get; set; }
        public DbSet<PersonasResultado> dataPersonasResultados { get; set; }
        public DbSet<PerfilBrechas> dataPerfilBrechas { get; set; }
        public DbSet<CriterioGrafico> dataCriterioGraficos { get; set; }
        public DbSet<LogroInstrumentos> dataLogroInstrumentos { get; set; }
        public DbSet<FaenaPerfilCandidato> dataFaenaPerfilCandidatos { get; set; }
        public DbSet<ProcesoLogroCandidato> dataProcesoLogroCandidatos { get; set; }
        #endregion
    }
}
