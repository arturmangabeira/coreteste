using System.Collections.Generic;
using System.ServiceModel;
using Core.Api.Model;

namespace Core.Api.Models
{
    [ServiceContract]
    public interface ISampleService
    {
      [OperationContract]
      string TextoRetorno(string s);
      [OperationContract]
      void XmlMethod(System.Xml.Linq.XElement xml);
      [OperationContract]
      List<Events> TestCustomModel(Events inputModel);

      //List<Evento> EventoModel(Evento inputModel);
      [OperationContract]
      List<Evento> EventoModel(Evento inputModel);
    }
}