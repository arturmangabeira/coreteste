using System.ServiceModel;
using Core.Api.Data;
using Core.Api.Integracao;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SoapCore;

namespace Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {         
            services.AddDbContext<DataContext>(
                x => x.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"))
            );
                     
            services.AddScoped<IIntegracaoService, IntegracaoService>();
            services.AddSoapExceptionTransformer((ex) => ex.Message);
            services.AddControllers();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //REFERENCIA A INTERFACE DE "ServiceContract -> IIntegracaoService" PARA QUE SEJA REFLETIDA COMO SERVIÇOS EM SOAP.
            app.UseSoapEndpoint<IIntegracaoService>("/Integrador.asmx", new BasicHttpBinding(), SoapSerializer.XmlSerializer);

        }
    }
}
