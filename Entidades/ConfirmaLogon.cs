using Core.Api.Entidades;

namespace coreteste.Entidades.ConfirmaLogon
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

        private string desafioassinado;
        /// <remarks/>
        public string DesafioAssinado
        {
            get { return desafioassinado; }
            set { desafioassinado = value; }
        }
                
    }
}