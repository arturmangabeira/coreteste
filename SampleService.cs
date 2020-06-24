using System.Xml.Linq;
using Core.Api.Data;
using Core.Api.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using Core.Api.Model;

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
            //List<Evento> retorno = new List<Evento>();
            return this.dataContext.Eventos.ToList();;
        }

        void ISampleService.XmlMethod(XElement xml)
        {
            throw new System.NotImplementedException();
        }

    }
}