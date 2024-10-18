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
using Entities.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using AspNetCoreRateLimit;
using api.Extensions;

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
            services.AddHttpContextAccessor();
            //.AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null); 
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
            services.AddScoped<IPermisosServices, PermisosServices>();
            services.AddScoped<IRolService, RolServices>();

            services.AddScoped<ICriterioService, CriterioServices>();


            //Funciones realizadas por Syscomp
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<ISectorService, SectorService>();
            services.AddScoped<IFaenaService, FaenaService>();
            services.AddScoped<IExportarService, ExportarService>();
            services.AddScoped<IEvaluacionExportarService, EvaluacionExportarService>();

            //
            services.AddScoped<ICandidatoService, CandidatoService>();
            services.AddScoped<ILogTableroService,  LogTableroServices>();

            // Configuración de autorización
            services.AddAuthorization(options =>
            {
                options.AddPolicy("GestionInformesPolicy", policy =>
                    policy.Requirements.Add(new PermissionRequirement("GestionInformes")));
                options.AddPolicy("TableroGestionPolicy", policy =>
                    policy.Requirements.Add(new PermissionRequirement("TableroGestion")));
                options.AddPolicy("TratamientoBrechaPolicy", policy =>
                    policy.Requirements.Add(new PermissionRequirement("TratamientoBrecha")));
                options.AddPolicy("DescargasPolicy", policy =>
                    policy.Requirements.Add(new PermissionRequirement("Descargas")));
            });

            // Registrar manejadores de autorización
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton<IAuthorizationHandler, RoleHandler>();
            services.ConfigureRateLimitiong();
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
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.Use(async (context, next) =>
            {
                // Eliminar encabezados específicos
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Remove("X-Powered-By");
                    context.Response.Headers.Remove("X-AspNet-Version");
                    // Puedes agregar más encabezados a eliminar según sea necesario
                    return Task.CompletedTask;
                });

                // Agregar encabezados de seguridad
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                context.Response.Headers["X-Frame-Options"] = "DENY";
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
                context.Response.Headers["Referrer-Policy"] = "no-referrer-when-downgrade";

                await next();
            });

            app.UseCors(builder => builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()

           );

          


            app.UseIpRateLimiting();

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
