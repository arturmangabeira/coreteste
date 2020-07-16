using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Api.Entidades;

namespace Core.Api.Entidades.SolicitaLogonRetorno
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Message : EntidadeBase
    {

        private MessageIdType messageIdField;

        private MessageMessageBody messageBodyField;

        private SignatureType signatureField;

        /// <remarks/>
        public MessageIdType MessageId
        {
            get
            {
                return this.messageIdField;
            }
            set
            {
                this.messageIdField = value;
            }
        }

        /// <remarks/>
        public MessageMessageBody MessageBody
        {
            get
            {
                return this.messageBodyField;
            }
            set
            {
                this.messageBodyField = value;
            }
        }

        public SignatureType Signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
            }
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MessageMessageBody
    {

        private RespostaType respostaField;
        
        /// <remarks/>
        public RespostaType Resposta
        {
            get
            {
                return this.respostaField;
            }
            set
            {
                this.respostaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RespostaType
    {

        private MensagemRespostaType mensagemrespostaField;

        private string desafio;

        /// <remarks/>
        public string Desafio
        {
            get { return desafio; }
            set { desafio = value; }
        }

        
        /// <remarks/>
        public MensagemRespostaType Mensagem
        {
            get
            {
                return this.mensagemrespostaField;
            }
            set
            {
                this.mensagemrespostaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MensagemRespostaType
    {

        private string codigo;
        private string descricao;

        /// <remarks/>
        public string Codigo
        {
            get
            {
                return this.codigo;
            }
            set
            {
                this.codigo = value;
            }
        }
        /// <remarks/>
        public string Descricao
        {
            get
            {
                return this.descricao;
            }
            set
            {
                this.descricao = value;
            }
        }
    }
}