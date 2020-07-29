using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Objects
{

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "consultarProcesso", WrapperNamespace = "", IsWrapped = true)]
    public partial class consultarProcessoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 0)]
        public string idConsultante;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 1)]
        public string senhaConsultante;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 2)]
        public string numeroProcesso;
                
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 3)]
        public bool movimentos;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 4)]
        public bool incluirCabecalho;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 5)]
        public bool incluirDocumentos;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true)]
        public string[] documento;

        public consultarProcessoRequest()
        {
        }

        public consultarProcessoRequest(string idConsultante, string senhaConsultante, string numeroProcesso, bool movimentos, bool incluirCabecalho, bool incluirDocumentos, string[] documento)
        {
            this.idConsultante = idConsultante;
            this.senhaConsultante = senhaConsultante;
            this.numeroProcesso = numeroProcesso;            
            this.movimentos = movimentos;
            this.incluirCabecalho = incluirCabecalho;
            this.incluirDocumentos = incluirDocumentos;
            this.documento = documento;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "consultarProcessoResposta", WrapperNamespace = "", IsWrapped = true)]
    public partial class consultarProcessoResponse
    {
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 0)]
        public bool sucesso;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 1)]
        public string mensagem;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 2)]
        public tipoProcessoJudicial processo;

        public consultarProcessoResponse()
        {
        }

        public consultarProcessoResponse(bool sucesso, string mensagem, tipoProcessoJudicial processo)
        {
            this.sucesso = sucesso;
            this.mensagem = mensagem;
            this.processo = processo;
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoProcessoJudicial
    {

        private tipoCabecalhoProcesso dadosBasicosField;

        private tipoMovimentoProcessual[] movimentoField;

        private tipoDocumento[] documentoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public tipoCabecalhoProcesso dadosBasicos
        {
            get
            {
                return this.dadosBasicosField;
            }
            set
            {
                this.dadosBasicosField = value;                
            }
        }

        /// <remarks/>   
        [System.Xml.Serialization.XmlElementAttribute("movimento", IsNullable = true, Order = 1)]
        public tipoMovimentoProcessual[] movimento
        {
            get
            {
                return this.movimentoField;
            }
            set
            {
                this.movimentoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true, Order = 2)]
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoMovimentoProcessual
    {

        private string[] complementoField;

        private tipoMovimentoNacional movimentoNacionalField;

        private string dataHoraField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("complemento", IsNullable = true, Order = 0)]
        public string[] complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;                
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public tipoMovimentoNacional movimentoNacional
        {
            get
            {
                return this.movimentoNacionalField;
            }
            set
            {
                this.movimentoNacionalField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataHora
        {
            get
            {
                return this.dataHoraField;
            }
            set
            {
                this.dataHoraField = value;
                
            }
        }     
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoDocumento
    {

        private byte[] conteudoField;

        private tipoAssinatura[] assinaturaField;

        private tipoParametro[] outroParametroField;

        private System.Xml.XmlElement anyField;

        private tipoDocumento[] documentoVinculadoField;

        private string idDocumentoField;

        private string idDocumentoVinculadoField;

        private string tipoDocumento1Field;

        private string dataHoraField;

        private string mimetypeField;

        private int nivelSigiloField;

        private bool nivelSigiloFieldSpecified;

        private int movimentoField;

        private bool movimentoFieldSpecified;

        private string hashField;

        private string descricaoField;

        private string tipoDocumentoLocalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 0)]
        public byte[] conteudo
        {
            get
            {
                return this.conteudoField;
            }
            set
            {
                this.conteudoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assinatura", IsNullable = true, Order = 1)]
        public tipoAssinatura[] assinatura
        {
            get
            {
                return this.assinaturaField;
            }
            set
            {
                this.assinaturaField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("outroParametro", IsNullable = true, Order = 2)]
        public tipoParametro[] outroParametro
        {
            get
            {
                return this.outroParametroField;
            }
            set
            {
                this.outroParametroField = value;             
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
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
        [System.Xml.Serialization.XmlElementAttribute("documentoVinculado", IsNullable = true, Order = 4)]
        public tipoDocumento[] documentoVinculado
        {
            get
            {
                return this.documentoVinculadoField;
            }
            set
            {
                this.documentoVinculadoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string idDocumento
        {
            get
            {
                return this.idDocumentoField;
            }
            set
            {
                this.idDocumentoField = value;             
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string idDocumentoVinculado
        {
            get
            {
                return this.idDocumentoVinculadoField;
            }
            set
            {
                this.idDocumentoVinculadoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("tipoDocumento")]
        public string tipoDocumento1
        {
            get
            {
                return this.tipoDocumento1Field;
            }
            set
            {
                this.tipoDocumento1Field = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataHora
        {
            get
            {
                return this.dataHoraField;
            }
            set
            {
                this.dataHoraField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mimetype
        {
            get
            {
                return this.mimetypeField;
            }
            set
            {
                this.mimetypeField = value;                
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int movimento
        {
            get
            {
                return this.movimentoField;
            }
            set
            {
                this.movimentoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool movimentoSpecified
        {
            get
            {
                return this.movimentoFieldSpecified;
            }
            set
            {
                this.movimentoFieldSpecified = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hash
        {
            get
            {
                return this.hashField;
            }
            set
            {
                this.hashField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string descricao
        {
            get
            {
                return this.descricaoField;
            }
            set
            {
                this.descricaoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoDocumentoLocal
        {
            get
            {
                return this.tipoDocumentoLocalField;
            }
            set
            {
                this.tipoDocumentoLocalField = value;         
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoAssinatura : object, System.ComponentModel.INotifyPropertyChanged
    {

        private tipoSignatarioSimples[] signatarioLoginField;

        private string assinaturaField;

        private string dataAssinaturaField;

        private string cadeiaCertificadoField;

        private string algoritmoHashField;

        private string codificacaoCertificadoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("signatarioLogin", IsNullable = true, Order = 0)]
        public tipoSignatarioSimples[] signatarioLogin
        {
            get
            {
                return this.signatarioLoginField;
            }
            set
            {
                this.signatarioLoginField = value;
                this.RaisePropertyChanged("signatarioLogin");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assinatura
        {
            get
            {
                return this.assinaturaField;
            }
            set
            {
                this.assinaturaField = value;
                this.RaisePropertyChanged("assinatura");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataAssinatura
        {
            get
            {
                return this.dataAssinaturaField;
            }
            set
            {
                this.dataAssinaturaField = value;
                this.RaisePropertyChanged("dataAssinatura");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cadeiaCertificado
        {
            get
            {
                return this.cadeiaCertificadoField;
            }
            set
            {
                this.cadeiaCertificadoField = value;
                this.RaisePropertyChanged("cadeiaCertificado");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string algoritmoHash
        {
            get
            {
                return this.algoritmoHashField;
            }
            set
            {
                this.algoritmoHashField = value;
                this.RaisePropertyChanged("algoritmoHash");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codificacaoCertificado
        {
            get
            {
                return this.codificacaoCertificadoField;
            }
            set
            {
                this.codificacaoCertificadoField = value;
                this.RaisePropertyChanged("codificacaoCertificado");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
       

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoSignatarioSimples : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string identificadorField;

        private string dataHoraField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string identificador
        {
            get
            {
                return this.identificadorField;
            }
            set
            {
                this.identificadorField = value;
                this.RaisePropertyChanged("identificador");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataHora
        {
            get
            {
                return this.dataHoraField;
            }
            set
            {
                this.dataHoraField = value;
                this.RaisePropertyChanged("dataHora");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoOrgaoJulgador
    {

        private string codigoOrgaoField;

        private string nomeOrgaoField;

        private string instanciaField;

        private int codigoMunicipioIBGEField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigoOrgao
        {
            get
            {
                return this.codigoOrgaoField;
            }
            set
            {
                this.codigoOrgaoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nomeOrgao
        {
            get
            {
                return this.nomeOrgaoField;
            }
            set
            {
                this.nomeOrgaoField = value;               
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string instancia
        {
            get
            {
                return this.instanciaField;
            }
            set
            {
                this.instanciaField = value;            
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int codigoMunicipioIBGE
        {
            get
            {
                return this.codigoMunicipioIBGEField;
            }
            set
            {
                this.codigoMunicipioIBGEField = value;         
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoMovimentoNacional
    {

        private string[] complementoField;

        //private int codigoNacionalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("complemento", IsNullable = true, Order = 0)]
        public string[] complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;                
            }
        }
        /*
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int codigoNacional
        {
            get
            {
                return this.codigoNacionalField;
            }
            set
            {
                this.codigoNacionalField = value;         
            }
        }*/
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoCabecalhoProcesso
    {

        private tipoPoloProcessual[] poloField;

        private tipoAssuntoProcessual[] assuntoField;

        private tipoParametro[] outroParametroField;

        private string[] magistradoAtuanteField;
        
        private string[] prioridadeField;

        private double valorCausaField;

        private bool valorCausaFieldSpecified;

        private tipoOrgaoJulgador orgaoJulgadorField;

        private string[] outrosnumerosField;

        private string numeroField;

        private int competenciaField;

        private bool competenciaFieldSpecified;

        private int classeProcessualField;

        private string codigoLocalidadeField;

        private int nivelSigiloField;

        private bool intervencaoMPField;

        private bool intervencaoMPFieldSpecified;

        private int tamanhoProcessoField;

        private bool tamanhoProcessoFieldSpecified;

        private string dataAjuizamentoField;

        [System.Xml.Serialization.XmlElementAttribute("polo", Order = 0)]
        public tipoPoloProcessual[] polo
        {
            get
            {
                return this.poloField;
            }
            set
            {
                this.poloField = value;                
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("assunto", Order = 1)]
        public tipoAssuntoProcessual[] assunto
        {
            get
            {
                return this.assuntoField;
            }
            set
            {
                this.assuntoField = value;                
            }
        }

        /// <remarks/>        
        [System.Xml.Serialization.XmlElementAttribute("magistradoAtuante", Order = 2)]
        public string[] magistradoAtuante
        {
            get
            {
                return this.magistradoAtuanteField;
            }
            set
            {
                this.magistradoAtuanteField = value;
             
            }
        }

        /// <remarks/>        


        /// <remarks/>        
        [System.Xml.Serialization.XmlElementAttribute("prioridade", Order = 3)]
        public string[] prioridade
        {
            get
            {
                return this.prioridadeField;
            }
            set
            {
                this.prioridadeField = value;
                
            }
        }

       
        [System.Xml.Serialization.XmlElementAttribute("outroParametro", IsNullable = true, Order = 4)]
        public tipoParametro[] outroParametro
        {
            get
            {
                return this.outroParametroField;
            }
            set
            {
                this.outroParametroField = value;                
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public double valorCausa
        {
            get
            {
                return this.valorCausaField;
            }
            set
            {
                this.valorCausaField = value;                
            }
        }


        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valorCausaSpecified
        {
            get
            {
                return this.valorCausaFieldSpecified;
            }
            set
            {
                this.valorCausaFieldSpecified = value;
                
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public tipoOrgaoJulgador orgaoJulgador
        {
            get
            {
                return this.orgaoJulgadorField;
            }
            set
            {
                this.orgaoJulgadorField = value;             
            }
        }

        /// <remarks/>        
        [System.Xml.Serialization.XmlElementAttribute("outrosnumeros", IsNullable = true, Order = 7)]
        /// <remarks/>        
        public string[] outrosnumeros
        {
            get
            {
                return this.outrosnumerosField;
            }
            set
            {
                this.outrosnumerosField = value;
               
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
        public string numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
                
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
        public int competencia
        {
            get
            {
                return this.competenciaField;
            }
            set
            {
                this.competenciaField = value;               
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        /// <remarks/>        
        public bool competenciaSpecified
        {
            get
            {
                return this.competenciaFieldSpecified;
            }
            set
            {
                this.competenciaFieldSpecified = value;               
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int classeProcessual
        {
            get
            {
                return this.classeProcessualField;
            }
            set
            {
                this.classeProcessualField = value;               
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
        public string codigoLocalidade
        {
            get
            {
                return this.codigoLocalidadeField;
            }
            set
            {
                this.codigoLocalidadeField = value;
               
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
        public bool intervencaoMP
        {
            get
            {
                return this.intervencaoMPField;
            }
            set
            {
                this.intervencaoMPField = value;
               
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        /// <remarks/>        
        public bool intervencaoMPSpecified
        {
            get
            {
                return this.intervencaoMPFieldSpecified;
            }
            set
            {
                this.intervencaoMPFieldSpecified = value;
              
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
        public int tamanhoProcesso
        {
            get
            {
                return this.tamanhoProcessoField;
            }
            set
            {
                this.tamanhoProcessoField = value;
              
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        /// <remarks/>        
        public bool tamanhoProcessoSpecified
        {
            get
            {
                return this.tamanhoProcessoFieldSpecified;
            }
            set
            {
                this.tamanhoProcessoFieldSpecified = value;
            
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
        public string dataAjuizamento
        {
            get
            {
                return this.dataAjuizamentoField;
            }
            set
            {
                this.dataAjuizamentoField = value;
         
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoParametro
    {

        private string nomeField;

        private string valorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nome
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;         
            }
        }       
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoPoloProcessual
    {

        private tipoParte[] parteField;

        private modalidadePoloProcessual poloField;

        private bool poloFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("parte", Order = 0)]
        public tipoParte[] parte
        {
            get
            {
                return this.parteField;
            }
            set
            {
                this.parteField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public modalidadePoloProcessual polo
        {
            get
            {
                return this.poloField;
            }
            set
            {
                this.poloField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool poloSpecified
        {
            get
            {
                return this.poloFieldSpecified;
            }
            set
            {
                this.poloFieldSpecified = value;         
            }
        }       
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum modalidadePoloProcessual
    {

        /// <remarks/>
        AT,

        /// <remarks/>
        PA,

        /// <remarks/>
        TC,

        /// <remarks/>
        FL,

        /// <remarks/>
        TJ,

        /// <remarks/>
        AD,

        /// <remarks/>
        VI,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoParte
    {

        private tipoPessoa pessoaField;

        private string interessePublicoField;

        private tipoRepresentanteProcessual[] advogadoField;

        private tipoParte[] pessoaProcessualRelacionadaField;

        private bool assistenciaJudiciariaField;

        private bool assistenciaJudiciariaFieldSpecified;

        private int intimacaoPendenteField;

        private bool intimacaoPendenteFieldSpecified;

        private modalidadeRelacionamentoProcessual relacionamentoProcessualField;

        private bool relacionamentoProcessualFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public tipoPessoa pessoa
        {
            get
            {
                return this.pessoaField;
            }
            set
            {
                this.pessoaField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string interessePublico
        {
            get
            {
                return this.interessePublicoField;
            }
            set
            {
                this.interessePublicoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("advogado", IsNullable = true, Order = 2)]
        public tipoRepresentanteProcessual[] advogado
        {
            get
            {
                return this.advogadoField;
            }
            set
            {
                this.advogadoField = value;               
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pessoaProcessualRelacionada", IsNullable = true, Order = 3)]
        public tipoParte[] pessoaProcessualRelacionada
        {
            get
            {
                return this.pessoaProcessualRelacionadaField;
            }
            set
            {
                this.pessoaProcessualRelacionadaField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool assistenciaJudiciaria
        {
            get
            {
                return this.assistenciaJudiciariaField;
            }
            set
            {
                this.assistenciaJudiciariaField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assistenciaJudiciariaSpecified
        {
            get
            {
                return this.assistenciaJudiciariaFieldSpecified;
            }
            set
            {
                this.assistenciaJudiciariaFieldSpecified = value;               
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int intimacaoPendente
        {
            get
            {
                return this.intimacaoPendenteField;
            }
            set
            {
                this.intimacaoPendenteField = value;              
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool intimacaoPendenteSpecified
        {
            get
            {
                return this.intimacaoPendenteFieldSpecified;
            }
            set
            {
                this.intimacaoPendenteFieldSpecified = value;             
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public modalidadeRelacionamentoProcessual relacionamentoProcessual
        {
            get
            {
                return this.relacionamentoProcessualField;
            }
            set
            {
                this.relacionamentoProcessualField = value;              
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool relacionamentoProcessualSpecified
        {
            get
            {
                return this.relacionamentoProcessualFieldSpecified;
            }
            set
            {
                this.relacionamentoProcessualFieldSpecified = value;         
            }
        }               
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
    public partial class tipoRepresentanteProcessual
    {

       

        private string nomeField;

        private string inscricaoField;

        private string numeroDocumentoPrincipalField;

        private bool intimacaoField;

        private modalidadeRepresentanteProcessual tipoRepresentanteField;

        /// <remarks/>       

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nome
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inscricao
        {
            get
            {
                return this.inscricaoField;
            }
            set
            {
                this.inscricaoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numeroDocumentoPrincipal
        {
            get
            {
                return this.numeroDocumentoPrincipalField;
            }
            set
            {
                this.numeroDocumentoPrincipalField = value;               
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool intimacao
        {
            get
            {
                return this.intimacaoField;
            }
            set
            {
                this.intimacaoField = value;               
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public modalidadeRepresentanteProcessual tipoRepresentante
        {
            get
            {
                return this.tipoRepresentanteField;
            }
            set
            {
                this.tipoRepresentanteField = value;         
            }
        }                
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
    public enum modalidadeRepresentanteProcessual
    {

        /// <remarks/>
        A,

        /// <remarks/>
        E,

        /// <remarks/>
        M,

        /// <remarks/>
        D,

        /// <remarks/>
        P,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoPessoa
    {

        private string[] outroNomeField;

        private tipoDocumentoIdentificacao[] documentoField;
        
        private tipoRelacionamentoPessoal[] pessoaRelacionadaField;

        private tipoPessoa pessoaVinculadaField;

        private string nomeField;

        private modalidadeGeneroPessoa sexoField;

        private string nomeGenitorField;

        private string nomeGenitoraField;

        private string dataNascimentoField;

        private string dataObitoField;

        private string numeroDocumentoPrincipalField;

        private tipoQualificacaoPessoa tipoPessoa1Field;

        private string cidadeNaturalField;

        private string estadoNaturalField;

        private string nacionalidadeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("outroNome", IsNullable = true, Order = 0)]
        public string[] outroNome
        {
            get
            {
                return this.outroNomeField;
            }
            set
            {
                this.outroNomeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true, Order = 1)]
        public tipoDocumentoIdentificacao[] documento
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
        [System.Xml.Serialization.XmlElementAttribute("pessoaRelacionada", IsNullable = true, Order = 2)]
        public tipoRelacionamentoPessoal[] pessoaRelacionada
        {
            get
            {
                return this.pessoaRelacionadaField;
            }
            set
            {
                this.pessoaRelacionadaField = value;          
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public tipoPessoa pessoaVinculada
        {
            get
            {
                return this.pessoaVinculadaField;
            }
            set
            {
                this.pessoaVinculadaField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nome
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public modalidadeGeneroPessoa sexo
        {
            get
            {
                return this.sexoField;
            }
            set
            {
                this.sexoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nomeGenitor
        {
            get
            {
                return this.nomeGenitorField;
            }
            set
            {
                this.nomeGenitorField = value;             
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nomeGenitora
        {
            get
            {
                return this.nomeGenitoraField;
            }
            set
            {
                this.nomeGenitoraField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataNascimento
        {
            get
            {
                return this.dataNascimentoField;
            }
            set
            {
                this.dataNascimentoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataObito
        {
            get
            {
                return this.dataObitoField;
            }
            set
            {
                this.dataObitoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string numeroDocumentoPrincipal
        {
            get
            {
                return this.numeroDocumentoPrincipalField;
            }
            set
            {
                this.numeroDocumentoPrincipalField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("tipoPessoa")]
        public tipoQualificacaoPessoa tipoPessoa1
        {
            get
            {
                return this.tipoPessoa1Field;
            }
            set
            {
                this.tipoPessoa1Field = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string cidadeNatural
        {
            get
            {
                return this.cidadeNaturalField;
            }
            set
            {
                this.cidadeNaturalField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string estadoNatural
        {
            get
            {
                return this.estadoNaturalField;
            }
            set
            {
                this.estadoNaturalField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nacionalidade
        {
            get
            {
                return this.nacionalidadeField;
            }
            set
            {
                this.nacionalidadeField = value;                
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoDocumentoIdentificacao
    {

        private string codigoDocumentoField;

        private string emissorDocumentoField;

        private string tipoDocumentoField;

        private string nomeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codigoDocumento
        {
            get
            {
                return this.codigoDocumentoField;
            }
            set
            {
                this.codigoDocumentoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string emissorDocumento
        {
            get
            {
                return this.emissorDocumentoField;
            }
            set
            {
                this.emissorDocumentoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tipoDocumento
        {
            get
            {
                return this.tipoDocumentoField;
            }
            set
            {
                this.tipoDocumentoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nome
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum modalidadeDocumentoIdentificador
    {

        /// <remarks/>
        CI,

        /// <remarks/>
        CNH,

        /// <remarks/>
        TE,

        /// <remarks/>
        CN,

        /// <remarks/>
        CC,

        /// <remarks/>
        PAS,

        /// <remarks/>
        CT,

        /// <remarks/>
        RIC,

        /// <remarks/>
        CMF,

        /// <remarks/>
        PIS_PASEP,

        /// <remarks/>
        CEI,

        /// <remarks/>
        NIT,

        /// <remarks/>
        CP,

        /// <remarks/>
        IF,

        /// <remarks/>
        OAB,

        /// <remarks/>
        RJC,

        /// <remarks/>
        RGE,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoRelacionamentoPessoal
    {

        private tipoPessoa pessoaField;

        private modalidadesRelacionamentoPessoal modalidadeRelacionamentoField;

        private bool modalidadeRelacionamentoFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public tipoPessoa pessoa
        {
            get
            {
                return this.pessoaField;
            }
            set
            {
                this.pessoaField = value;               
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public modalidadesRelacionamentoPessoal modalidadeRelacionamento
        {
            get
            {
                return this.modalidadeRelacionamentoField;
            }
            set
            {
                this.modalidadeRelacionamentoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool modalidadeRelacionamentoSpecified
        {
            get
            {
                return this.modalidadeRelacionamentoFieldSpecified;
            }
            set
            {
                this.modalidadeRelacionamentoFieldSpecified = value;                
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum modalidadesRelacionamentoPessoal
    {

        /// <remarks/>
        P,

        /// <remarks/>
        AP,

        /// <remarks/>
        SP,

        /// <remarks/>
        T,

        /// <remarks/>
        C,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum modalidadeGeneroPessoa
    {

        /// <remarks/>
        M,

        /// <remarks/>
        F,

        /// <remarks/>
        D,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum tipoQualificacaoPessoa
    {

        /// <remarks/>
        fisica,

        /// <remarks/>
        juridica,

        /// <remarks/>
        autoridade,

        /// <remarks/>
        orgaorepresentacao,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public enum modalidadeRelacionamentoProcessual
    {

        /// <remarks/>
        CP,

        /// <remarks/>
        RP,

        /// <remarks/>
        TF,

        /// <remarks/>
        AT,

        /// <remarks/>
        AS,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoAssuntoProcessual
    {

        private int codigoNacionalField;

        private bool codigoNacionalFieldSpecified;

        private tipoAssuntoLocal assuntoLocalField;

        private bool principalField;

        private bool principalFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int codigoNacional
        {
            get
            {
                return this.codigoNacionalField;
            }
            set
            {
                this.codigoNacionalField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool codigoNacionalSpecified
        {
            get
            {
                return this.codigoNacionalFieldSpecified;
            }
            set
            {
                this.codigoNacionalFieldSpecified = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public tipoAssuntoLocal assuntoLocal
        {
            get
            {
                return this.assuntoLocalField;
            }
            set
            {
                this.assuntoLocalField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool principal
        {
            get
            {
                return this.principalField;
            }
            set
            {
                this.principalField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool principalSpecified
        {
            get
            {
                return this.principalFieldSpecified;
            }
            set
            {
                this.principalFieldSpecified = value;
            }
        }

    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "")]
    public partial class tipoAssuntoLocal
    {       

        private int codigoAssuntoField;

        private string descricaoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int codigoAssunto
        {
            get
            {
                return this.codigoAssuntoField;
            }
            set
            {
                this.codigoAssuntoField = value;                
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string descricao
        {
            get
            {
                return this.descricaoField;
            }
            set
            {
                this.descricaoField = value;                
            }
        }
    }

}
