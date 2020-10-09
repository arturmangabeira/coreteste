using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegradorIdea.Objects
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "entregarManifestacaoProcessual", WrapperNamespace = "", IsWrapped = true)]
    public partial class entregarManifestacaoProcessualRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 0)]
        public string idManifestante;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 1)]
        public string senhaManifestante;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 2)]
        public string numeroProcesso;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 3)]
        public tipoCabecalhoProcesso dadosBasicos;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("documento", Namespace = "")]
        public Documento[] documento;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 5)]
        public string dataEnvio;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute("parametros", IsNullable = true)]
        public tipoParametro[] parametros;

        public entregarManifestacaoProcessualRequest()
        {
        }

        public entregarManifestacaoProcessualRequest(string idManifestante, string senhaManifestante, string numeroProcesso, tipoCabecalhoProcesso dadosBasicos, Documento[] documento, string dataEnvio, tipoParametro[] parametros)
        {
            this.idManifestante = idManifestante;
            this.senhaManifestante = senhaManifestante;
            this.numeroProcesso = numeroProcesso;
            this.dadosBasicos = dadosBasicos;
            this.documento = documento;
            this.dataEnvio = dataEnvio;
            this.parametros = parametros;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "entregarManifestacaoProcessualResposta", WrapperNamespace = "", IsWrapped = true)]
    public partial class entregarManifestacaoProcessualResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 0)]
        public bool sucesso;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 1)]
        public string mensagem;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 2)]
        public string protocoloRecebimento;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 3)]
        public string dataOperacao;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] recibo;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute("parametro", IsNullable = true)]
        public tipoParametro[] parametro;

        public entregarManifestacaoProcessualResponse()
        {
        }

        public entregarManifestacaoProcessualResponse(bool sucesso, string mensagem, string protocoloRecebimento, string dataOperacao, byte[] recibo, tipoParametro[] parametro)
        {
            this.sucesso = sucesso;
            this.mensagem = mensagem;
            this.protocoloRecebimento = protocoloRecebimento;
            this.dataOperacao = dataOperacao;
            this.recibo = recibo;
            this.parametro = parametro;
        }
    }
}
