using Core.Api.Models;
using Core.Api.Objects;
using System.Collections.Generic;
using System.ServiceModel;

namespace Core.Api.Integracao
{
    [ServiceContract]
    public interface IIntegracaoService
    {
        [OperationContract]
        [XmlSerializerFormat(SupportFaults = false)]
        string AutenticarESAJ();

        [OperationContract]
        [XmlSerializerFormat(SupportFaults = false)]
        string GetTiposDocDigitalXML(string codigo);

        [OperationContract]
        List<TipoOperacao> ObterTipoOperacaoBD();
        [OperationContract]
        public consultarProcessoResponse consultarProcesso(ConsultarProcesso consultarProcesso);
    }
}
