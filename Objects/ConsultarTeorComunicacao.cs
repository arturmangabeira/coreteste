using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegradorIdea.Objects
{

    public partial class consultarTeorComunicacaoRequest
    {


        public string idConsultante { get; set; }


        public string senhaConsultante { get; set; }


        public string numeroProcesso { get; set; }


        public string identificadorAviso { get; set; }

        public consultarTeorComunicacaoRequest()
        {
        }

        public consultarTeorComunicacaoRequest(string idConsultante, string senhaConsultante, string numeroProcesso, string identificadorAviso)
        {
            this.idConsultante = idConsultante;
            this.senhaConsultante = senhaConsultante;
            this.numeroProcesso = numeroProcesso;
            this.identificadorAviso = identificadorAviso;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "consultarTeorComunicacaoResposta", WrapperNamespace = "", IsWrapped = true)]
    public partial class consultarTeorComunicacaoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 0)]
        public bool sucesso;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 1)]
        public string mensagem;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("comunicacao", Namespace = "", IsNullable = true)]
        public tipoComunicacaoProcessual[] comunicacao;

        public consultarTeorComunicacaoResponse()
        {
        }

        public consultarTeorComunicacaoResponse(bool sucesso, string mensagem, tipoComunicacaoProcessual[] comunicacao)
        {
            this.sucesso = sucesso;
            this.mensagem = mensagem;
            this.comunicacao = comunicacao;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoComunicacaoProcessual : object, System.ComponentModel.INotifyPropertyChanged
    {

        private tipoParte destinatarioField;

        private string processoField;

        private string teorField;

        private tipoDocumento[] documentoField;

        private string[] parametroField;

        private System.Xml.XmlElement anyField;

        private string idField;

        private string tipoComunicacaoField;

        private tipoPrazo tipoPrazoField;

        private bool tipoPrazoFieldSpecified;

        private string dataReferenciaField;

        private int prazoField;

        private bool prazoFieldSpecified;

        private int nivelSigiloField;

        private bool nivelSigiloFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public tipoParte destinatario
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
        public string processo
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
        public string teor
        {
            get
            {
                return this.teorField;
            }
            set
            {
                this.teorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true, Order = 3)]
        public tipoDocumento[] documento
        {
            get
            {
                return this.documentoField;
            }
            set
            {
                this.documentoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("parametro", IsNullable = true, Order = 4)]
        public string[] parametro
        {
            get
            {
                return this.parametroField;
            }
            set
            {
                this.parametroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 5)]
        public System.Xml.XmlElement Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public tipoPrazo tipoPrazo
        {
            get
            {
                return this.tipoPrazoField;
            }
            set
            {
                this.tipoPrazoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool tipoPrazoSpecified
        {
            get
            {
                return this.tipoPrazoFieldSpecified;
            }
            set
            {
                this.tipoPrazoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataReferencia
        {
            get
            {
                return this.dataReferenciaField;
            }
            set
            {
                this.dataReferenciaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int prazo
        {
            get
            {
                return this.prazoField;
            }
            set
            {
                this.prazoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prazoSpecified
        {
            get
            {
                return this.prazoFieldSpecified;
            }
            set
            {
                this.prazoFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int nivelSigilo
        {
            get
            {
                return this.nivelSigiloField;
            }
            set
            {
                this.nivelSigiloField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool nivelSigiloSpecified
        {
            get
            {
                return this.nivelSigiloFieldSpecified;
            }
            set
            {
                this.nivelSigiloFieldSpecified = value;
            }
        }

        #pragma warning disable 067
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum tipoPrazo
    {

        /// <remarks/>
        HOR,

        /// <remarks/>
        DIA,

        /// <remarks/>
        MES,

        /// <remarks/>
        ANO,

        /// <remarks/>
        DATA_CERTA,

        /// <remarks/>
        SEMPRAZO,
    }

}
