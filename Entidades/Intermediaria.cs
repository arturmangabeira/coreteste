using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegradorIdea.Entidades.Intermediaria
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Message : EntidadeBase
    {

        private MessageIdType messageIdField;

        private MessageMessageBody messageBodyField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class MessageIdType
    {

        private ServicePetIntermediarioIdType serviceIdField;

        private VersionType versionField;

        private string msgDescField;

        private string codeField;

        private string fromAddressField;

        private string toAddressField;

        private string dateField;

        /// <remarks/>
        public ServicePetIntermediarioIdType ServiceId
        {
            get
            {
                return this.serviceIdField;
            }
            set
            {
                this.serviceIdField = value;
            }
        }

        /// <remarks/>
        public VersionType Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        public string MsgDesc
        {
            get
            {
                return this.msgDescField;
            }
            set
            {
                this.msgDescField = value;
            }
        }

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string FromAddress
        {
            get
            {
                return this.fromAddressField;
            }
            set
            {
                this.fromAddressField = value;
            }
        }

        /// <remarks/>
        public string ToAddress
        {
            get
            {
                return this.toAddressField;
            }
            set
            {
                this.toAddressField = value;
            }
        }

        /// <remarks/>
        public string Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum ServicePetIntermediarioIdType
    {

        /// <remarks/>
        PetIntermediario,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    public enum VersionType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1.0")]
        Item10,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DocumentoType
    {

        private string tipoField;

        private string nomeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ParteType
    {

        private string tipoField;

        private string nomeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Tipo
        {
            get
            {
                return this.tipoField;
            }
            set
            {
                this.tipoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PeticaoType
    {

        private string classeField;

        private string assuntoPrincipalField;

        private string[] outrosAssuntosField;

        private string processoField;

        private string foroField;

        private string nomePeticaoField;

        /// <remarks/>
        public string Classe
        {
            get
            {
                return this.classeField;
            }
            set
            {
                this.classeField = value;
            }
        }

        /// <remarks/>
        public string AssuntoPrincipal
        {
            get
            {
                return this.assuntoPrincipalField;
            }
            set
            {
                this.assuntoPrincipalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Assunto", IsNullable = false)]
        public string[] OutrosAssuntos
        {
            get
            {
                return this.outrosAssuntosField;
            }
            set
            {
                this.outrosAssuntosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Processo
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Foro
        {
            get
            {
                return this.foroField;
            }
            set
            {
                this.foroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string NomePeticao
        {
            get
            {
                return this.nomePeticaoField;
            }
            set
            {
                this.nomePeticaoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class MessageMessageBody
    {

        private PeticaoType peticaoField;

        private ParteType[] partesField;

        private DocumentoType[] documentosField;

        /// <remarks/>
        public PeticaoType Peticao
        {
            get
            {
                return this.peticaoField;
            }
            set
            {
                this.peticaoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Parte", IsNullable = false)]
        public ParteType[] Partes
        {
            get
            {
                return this.partesField;
            }
            set
            {
                this.partesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Documento", IsNullable = false)]
        public DocumentoType[] Documentos
        {
            get
            {
                return this.documentosField;
            }
            set
            {
                this.documentosField = value;
            }
        }
    }
}
