using System.Xml.Linq;
using Core.Api.Models;

namespace Core.Api
{
    public class SampleService : ISampleService
    {
        string ISampleService.Test(string s)
        {
            return "Test Method Executed!";            
        }

        Evento ISampleService.TestCustomModel(Evento inputModel)
        {
            return new Evento(){
                    DataEvento = "12/02/2020",
                    EventoId = 1,
                    Local = "testse",
                    Lote = "1",
                    QtdPessoas = 3,
                    Tema = "Show" 
                };
        }

        void ISampleService.XmlMethod(XElement xml)
        {
            throw new System.NotImplementedException();
        }
    }
}