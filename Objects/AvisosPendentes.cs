
namespace IntegradorIdea.Objects
{
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    //[System.ServiceModel.MessageContractAttribute(WrapperName = "consultarAvisosPendentesResposta", WrapperNamespace = "", IsWrapped = true)]
    public partial class consultarAvisosPendentesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 0)]
        public bool sucesso;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 1)]
        public string mensagem;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("aviso", Namespace = "", IsNullable = true)]
        public tipoAvisoComunicacaoPendente[] aviso;

        public consultarAvisosPendentesResponse()
        {
        }

        public consultarAvisosPendentesResponse(bool sucesso, string mensagem, tipoAvisoComunicacaoPendente[] aviso)
        {
            this.sucesso = sucesso;
            this.mensagem = mensagem;
            this.aviso = aviso;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoAvisoComunicacaoPendente
    {

        private tipoParteDestinatario destinatarioField;

        private tipoCabecalhoProcesso processoField;

        private string dataDisponibilizacaoField;

        private string idAvisoField;

        private string tipoComunicacaoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public tipoParteDestinatario destinatario
        {
            get
            {
                return this.destinatarioField;
            }
            set
            {
                this.destinatarioField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public tipoCabecalhoProcesso processo
        {
            get
            {
                return this.processoField;
            }
            set
            {
                this.processoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string dataDisponibilizacao
        {
            get
            {
                return this.dataDisponibilizacaoField;
            }
            set
            {
                this.dataDisponibilizacaoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string idAviso
        {
            get
            {
                return this.idAvisoField;
            }
            set
            {
                this.idAvisoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoComunicacao
        {
            get
            {
                return this.tipoComunicacaoField;
            }
            set
            {
                this.tipoComunicacaoField = value;         
            }
        }
    }
}
