using System.Xml.Linq;
using Core.Api.Data;
using Core.Api.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using Core.Api.Model;
using System.ServiceModel;
using Core.Api.Integracao;
using System.Security.Cryptography.X509Certificates;

namespace Core.Api
{
    public class SampleService : ISampleService
    {      
        private readonly IConfiguration configuration;  
        public DataContext dataContext { get; }
        public System.IServiceProvider ServiceProvider { get; }

        public SampleService(IConfiguration config)
        {
            this.configuration = config;
            this.dataContext = new DataContext(this.configuration);       
        }
        /*public SampleService(DataContext db)
        {
            this.dataContext = db;
        }*/
 
        string ISampleService.TextoRetorno(string s)
        {
            return "Test Method Executed!";
        }

        List<Events> ISampleService.TestCustomModel(Events inputModel)
        {     
            var result = this.dataContext.Eventos.ToList();
            
            List<Events> retorno = new List<Events>();

            foreach (var item in result)
            {
               retorno.Add(new Events()
                {
                    DataEvento = item.DataEvento,
                    EventoId = item.EventoId,
                    Local = item.Local,
                    Lote = item.Lote,
                    QtdPessoas = item.QtdPessoas,
                    Tema = item.Tema
                });
            }

            return retorno;
            /*
            return new Events()
            {
                DataEvento = "12/02/2020",
                EventoId = 1,
                Local = "testse",
                Lote = "1",
                QtdPessoas = 3,
                Tema = "Show"
            };*/
        }

        List<Evento> ISampleService.EventoModel(Evento inputModel)
        {
            IntegracaoTJBA.ServicoPJ2PortTypeClient servicoPJ2 = new IntegracaoTJBA.ServicoPJ2PortTypeClient(
                new BasicHttpBinding() {
                    Name = "ServicoPJ2HttpBinding",
                    AllowCookies = true,                                
                    TextEncoding = System.Text.Encoding.UTF8,
                    TransferMode = System.ServiceModel.TransferMode.Buffered,
                    UseDefaultWebProxy = true,                               
                    MaxReceivedMessageSize = 2147483647,
                    MaxBufferSize = 2147483647                
            },
                new EndpointAddress(this.configuration.GetValue<string>("UrlTJ"))            
            );

            var res = servicoPJ2.SolicitacaoCitacaoAtoAsync("", "");
            //List<Evento> retorno = new List<Evento>();
            return this.dataContext.Eventos.ToList();;
        }

        void ISampleService.XmlMethod(XElement xml)
        {
            throw new System.NotImplementedException();
        }

        public void GetTiposDocDigital()
        {

            var valorConfig = this.configuration.GetValue<string>("ESAJ:UrlTJ");
            IntegracaoTJBA.ServicoPJ2PortTypeClient servicoPJ2 = new IntegracaoTJBA.ServicoPJ2PortTypeClient(
               new BasicHttpBinding()
               {
                   Name = "ServicoPJ2HttpBinding",
                   AllowCookies = true,
                   TextEncoding = System.Text.Encoding.UTF8,
                   TransferMode = System.ServiceModel.TransferMode.Buffered,
                   UseDefaultWebProxy = true,
                   MaxReceivedMessageSize = 2147483647,
                   MaxBufferSize = 2147483647
               },
               new EndpointAddress(this.configuration.GetValue<string>("ESAJ:UrlTJ"))
           );

           var res = servicoPJ2.getTiposDocDigitalAsync().Result;

           var correios = new Correios.AtendeClienteClient();

           var consulta = correios.consultaCEPAsync("40080241").Result;

           
        }

        public string ObterCertificado(string nome)
        {
            string retorno = "";
            try
            {
                X509Certificate2 certificadoX509 = Certificado.ObterCertificado(this.configuration.GetValue<string>("Certificado:RepositorioCertificado"), nome, this.configuration);
                if (certificadoX509 != null)
                {
                    retorno = "Certificado encontrado!";
                }
            }
            catch (System.Exception ex)
            {

                retorno = $"Erro ao encontrar certificado ! Erro: {ex.Message},trace : {ex.StackTrace} ";
            }

            return retorno;
        }

        public string ConectarESAJ()
        {
            var valorConfig = this.configuration.GetValue<string>("ESAJ:UrlTJ");
            IntegracaoTJBA.ServicoPJ2PortTypeClient servicoPJ2 = new IntegracaoTJBA.ServicoPJ2PortTypeClient(
               new BasicHttpBinding()
               {
                   Name = "ServicoPJ2HttpBinding",
                   AllowCookies = true,
                   TextEncoding = System.Text.Encoding.UTF8,
                   TransferMode = System.ServiceModel.TransferMode.Buffered,
                   UseDefaultWebProxy = true,
                   MaxReceivedMessageSize = 2147483647,
                   MaxBufferSize = 2147483647
               },
               new EndpointAddress(this.configuration.GetValue<string>("ESAJ:UrlTJ"))
           );

            var res = servicoPJ2.getTiposDocDigitalAsync().Result;

            return res;
        }
    }
}