using Core.Api.Model;
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
        List<DocumentoDigital> ObterDocumentoDigitaisBD();
        [OperationContract]
        public Entidades.ConsultaProcessoResposta.Message ObterDadosProcesso(ConsultarProcesso consultarProcesso);
    }
}
