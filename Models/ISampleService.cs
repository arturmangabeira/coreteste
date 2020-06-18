using System.ServiceModel;

namespace Core.Api.Models
{
    [ServiceContract]
    public interface ISampleService
    {
      [OperationContract]
      string Test(string s);
      [OperationContract]
      void XmlMethod(System.Xml.Linq.XElement xml);
      [OperationContract]
      Evento TestCustomModel(Evento inputModel);
    }
}