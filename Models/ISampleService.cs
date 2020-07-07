using System.Collections.Generic;
using System.Reflection.Metadata;
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

      [OperationContract]
      [XmlSerializerFormat(SupportFaults = false)]
      string GetTiposDocDigital(string codigo);

      [OperationContract]
      [XmlSerializerFormat(SupportFaults = false)]
      string AutenticarESAJ();
      
      [OperationContract]
      string ObterCertificado(string nome);

      [OperationContract]   
      string ConectarESAJ();

      [OperationContract]
      List<DocumentoDigital> ObterDocumentoDigitais();  
    }
}