using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DizuAutomation.Repositorio.IOC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // Automapper
            //var mappingConfig = AutoMapperConfig.RegisterMappings();
            //IMapper mapper = mappingConfig.CreateMapper();
            //services.AddSingleton(mapper);

            // ASP.NET HttpContext dependency
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddScoped<IPlayerRepository, PlayerRepository>();
            //services.AddScoped<IPlayerTokenRepository, PlayerTokenRepository>();
            //services.AddScoped<IProfissaoRepository, ProfissaoRepository>();
        }
    }
}
