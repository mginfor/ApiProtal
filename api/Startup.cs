using Contracts;
using Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Generic;
using Services.Generic;
using Persistence;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.IO;
using api.Helpers;
using Entities.EPModels;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddHealthChecks();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            var connectionString = Configuration["mysqlconnection:connectionString"];
            services.AddDbContext<RepositoryContext>(o => o.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 25))));

            services.AddSingleton<ILoggerManager, LoggerManager>();

            //Dependency Injection
            services.AddScoped<ITableroService, TableroService>();
            services.AddScoped<INivelUsuarioService, NivelUsuarioService>();
            services.AddScoped<IUsuarioSecomLabService, UsuarioSecomLabService>();

            services.AddScoped<IUsuarioPortalService, UsuarioPortalService>();
            services.AddScoped<IProcesoService, ProcesoService>();
            services.AddScoped<IEstadoProcesoService, EstadoProcesoService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IServicioVinculadoService, ServicioVinculadoService>();

            services.AddScoped<IEvaluacionService, EvaluacionService>();
            services.AddScoped<IEvaluacionPCTService, EvaluacionPCTService>();
            services.AddScoped<IEvaluacionPROTService, EvaluacionPROTService>();
            services.AddScoped<IEvaluacionEIService, EvaluacionEIService>();
            services.AddScoped<IEvaluacionPROT2Service, EvaluacionPROT2Service>();
            services.AddScoped<IEvaluacionEJDService, EvaluacionEJDService>();
            services.AddScoped<IDocumentoBrechaService, DocumentoBrechaService>();
            services.AddScoped<IGraficoBrechaService, GraficoBrechaService>();
            services.AddScoped<ITablero_brecha, TableroBrechaService>();

            //Funciones realizadas por Syscomp
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<ISectorService, SectorService>();
            services.AddScoped<IFaenaService, FaenaService>();
            services.AddScoped<IExportarService, ExportarService>();
            services.AddScoped<IEvaluacionExportarService, EvaluacionExportarService>();

            //
            services.AddScoped<ICandidatoService, CandidatoService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
            }

            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()

           );



            app.UseMiddleware<JwtMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
