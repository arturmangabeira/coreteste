using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects
{
    [DebuggerDisplay("{nivelSigilo}, {hash}, {descricao}, {tipoDocumento}, {mimetype}, {idDocumento}, {dataHora}, {movimento}")]
    [Serializable]
    [DataContract(Name = "documento")]
    [XmlType(TypeName = "documento")]
    public partial class Documento
    {
        [DataMember(Name = "nivelSigilo")]
        [XmlAttribute(AttributeName = "nivelSigilo")]
        public int nivelSigilo { get; set; }

        [DataMember(Name = "hash")]
        [XmlAttribute(AttributeName = "hash")]
        public string hash { get; set; }

        [DataMember(Name = "descricao")]
        [XmlAttribute(AttributeName = "descricao")]
        public string descricao { get; set; }

        [DataMember(Name = "tipoDocumento")]
        [XmlAttribute(AttributeName = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "mimetype")]
        [XmlAttribute(AttributeName = "mimetype")]
        public string mimetype { get; set; }

        [DataMember(Name = "idDocumento")]
        [XmlAttribute(AttributeName = "idDocumento")]
        public string idDocumento { get; set; }

        [DataMember(Name = "dataHora")]
        [XmlAttribute(AttributeName = "dataHora")]
        public string dataHora { get; set; }

        [DataMember(Name = "movimento")]
        [XmlAttribute(AttributeName = "movimento")]
        public int movimento { get; set; }

        public byte[] conteudo { get; set; }

        public DocumentoVinculado[] documentoVinculado { get; set; }
    }

    [DebuggerDisplay("{nivelSigilo}, {hash}, {descricao}, {tipoDocumento}, {mimetype}, {idDocumento}, {dataHora}, {movimento}")]
    [Serializable]
    [DataContract(Name = "documentoVinculado")]
    [XmlType(TypeName = "documentoVinculado")]
    public partial class DocumentoVinculado
    {
        [DataMember(Name = "nivelSigilo")]
        [XmlAttribute(AttributeName = "nivelSigilo")]
        public int nivelSigilo { get; set; }

        [DataMember(Name = "hash")]
        [XmlAttribute(AttributeName = "hash")]
        public string hash { get; set; }

        [DataMember(Name = "descricao")]
        [XmlAttribute(AttributeName = "descricao")]
        public string descricao { get; set; }

        [DataMember(Name = "tipoDocumento")]
        [XmlAttribute(AttributeName = "tipoDocumento")]
        public string tipoDocumento { get; set; }

        [DataMember(Name = "mimetype")]
        [XmlAttribute(AttributeName = "mimetype")]
        public string mimetype { get; set; }

        [DataMember(Name = "idDocumento")]
        [XmlAttribute(AttributeName = "idDocumento")]
        public string idDocumento { get; set; }

        [DataMember(Name = "dataHora")]
        [XmlAttribute(AttributeName = "dataHora")]
        public string dataHora { get; set; }

        [DataMember(Name = "movimento")]
        [XmlAttribute(AttributeName = "movimento")]
        public int movimento { get; set; }

        public byte[] conteudo { get; set; }      
    }


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "consultarProcesso", WrapperNamespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", IsWrapped = true)]
    public partial class consultarProcessoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
        public string idConsultante;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
        public string senhaConsultante;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
        public string numeroProcesso;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
        public string dataReferencia;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
        public bool movimentos;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
        public bool incluirCabecalho;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
        public bool incluirDocumentos;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute("documento", Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", IsNullable = true)]
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

    //[System.Diagnostics.DebuggerStepThroughAttribute()]
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    //[System.ServiceModel.MessageContractAttribute(WrapperName = "consultarProcessoResponse", WrapperNamespace = "http://www.cnj.jus.br/servico-intercomunicacao-2.2.2/", IsWrapped = true)]
    public partial class consultarProcessoResponse
    {
        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 0)]
        public bool sucesso { get; set; }

        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 1)]
        public string mensagem { get; set; }

        //[System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2", Order = 2)]
        public tipoProcessoJudicial processo { get; set; }

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

    [Serializable]
    [DataContract(Name = "tipoProcessoJudicial")]
    [XmlType(TypeName = "tipoProcessoJudicial")]
    public partial class tipoProcessoJudicial
    {

        private tipoCabecalhoProcesso dadosBasicosField;

        private tipoMovimentoProcessual[] movimentoField;

        private tipoDocumento[] documentoField;

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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
        //[System.Xml.Serialization.XmlElementAttribute("movimento", IsNullable = true, Order = 1)]
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
        //[System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true, Order = 2)]
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

    [Serializable]
    [DataContract(Name = "tipoMovimentoProcessual")]
    [XmlType(TypeName = "tipoMovimentoProcessual")]
    public partial class tipoMovimentoProcessual
    {

        private string[] complementoField;

        private tipoMovimentoNacional movimentoNacionalField;

        private string dataHoraField;

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("complemento", IsNullable = true, Order = 0)]
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

        //[System.Xml.Serialization.XmlElementAttribute(Order = 1)]
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

        [DataMember(Name = "dataHora")]
        [XmlAttribute(AttributeName = "dataHora")]
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

    [Serializable]
    [DataContract(Name = "tipoDocumento")]
    [XmlType(TypeName = "tipoDocumento")]
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
        //[System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 0)]
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
        //[System.Xml.Serialization.XmlElementAttribute("assinatura", IsNullable = true, Order = 1)]
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
        //[System.Xml.Serialization.XmlElementAttribute("outroParametro", IsNullable = true, Order = 2)]
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
        //[System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
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
        //[System.Xml.Serialization.XmlElementAttribute("documentoVinculado", IsNullable = true, Order = 4)]
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

        [DataMember(Name = "idDocumento")]
        [XmlAttribute(AttributeName = "idDocumento")]
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

        [DataMember(Name = "idDocumentoVinculado")]
        [XmlAttribute(AttributeName = "idDocumentoVinculado")]
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

        [DataMember(Name = "tipoDocumento")]
        [XmlAttribute(AttributeName = "tipoDocumento")]
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

        [DataMember(Name = "dataHora")]
        [XmlAttribute(AttributeName = "dataHora")]
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

        [DataMember(Name = "mimetype")]
        [XmlAttribute(AttributeName = "mimetype")]
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

        [DataMember(Name = "nivelSigilo")]
        [XmlAttribute(AttributeName = "nivelSigilo")]
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

        [DataMember(Name = "movimento")]
        [XmlAttribute(AttributeName = "movimento")]
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

        [DataMember(Name = "hash")]
        [XmlAttribute(AttributeName = "hash")]
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

        [DataMember(Name = "descricao")]
        [XmlAttribute(AttributeName = "descricao")]
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

        [DataMember(Name = "tipoDocumentoLocal")]
        [XmlAttribute(AttributeName = "tipoDocumentoLocal")]
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

    [Serializable]
    [DataContract(Name = "tipoAssinatura")]
    [XmlType(TypeName = "tipoAssinatura")]
    public partial class tipoAssinatura
    {

        private tipoSignatarioSimples[] signatarioLoginField;

        private string assinaturaField;

        private string dataAssinaturaField;

        private string cadeiaCertificadoField;

        private string algoritmoHashField;

        private string codificacaoCertificadoField;

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("signatarioLogin", IsNullable = true, Order = 0)]
        public tipoSignatarioSimples[] signatarioLogin
        {
            get
            {
                return this.signatarioLoginField;
            }
            set
            {
                this.signatarioLoginField = value;                
            }
        }

        [DataMember(Name = "assinatura")]
        [XmlAttribute(AttributeName = "assinatura")]
        public string assinatura
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

        [DataMember(Name = "dataAssinatura")]
        [XmlAttribute(AttributeName = "dataAssinatura")]
        public string dataAssinatura
        {
            get
            {
                return this.dataAssinaturaField;
            }
            set
            {
                this.dataAssinaturaField = value;                
            }
        }

        [DataMember(Name = "cadeiaCertificado")]
        [XmlAttribute(AttributeName = "cadeiaCertificado")]
        public string cadeiaCertificado
        {
            get
            {
                return this.cadeiaCertificadoField;
            }
            set
            {
                this.cadeiaCertificadoField = value;                
            }
        }

        [DataMember(Name = "algoritmoHash")]
        [XmlAttribute(AttributeName = "algoritmoHash")]
        public string algoritmoHash
        {
            get
            {
                return this.algoritmoHashField;
            }
            set
            {
                this.algoritmoHashField = value;                
            }
        }

        [DataMember(Name = "codificacaoCertificado")]
        [XmlAttribute(AttributeName = "codificacaoCertificado")]
        public string codificacaoCertificado
        {
            get
            {
                return this.codificacaoCertificadoField;
            }
            set
            {
                this.codificacaoCertificadoField = value;                
            }
        }
    }


    [Serializable]
    [DataContract(Name = "tipoSignatarioSimples")]
    [XmlType(TypeName = "tipoSignatarioSimples")]
    public partial class tipoSignatarioSimples
    {

        private string identificadorField;

        private string dataHoraField;

        [DataMember(Name = "identificador")]
        [XmlAttribute(AttributeName = "identificador")]
        public string identificador
        {
            get
            {
                return this.identificadorField;
            }
            set
            {
                this.identificadorField = value;                
            }
        }

        [DataMember(Name = "dataHora")]
        [XmlAttribute(AttributeName = "dataHora")]
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

    [Serializable]
    [DataContract(Name = "tipoOrgaoJulgador")]
    [XmlType(TypeName = "tipoOrgaoJulgador")]
    public partial class tipoOrgaoJulgador
    {

        private string codigoOrgaoField;

        private string nomeOrgaoField;

        private string instanciaField;

        private int codigoMunicipioIBGEField;

        [DataMember(Name = "codigoOrgao")]
        [XmlAttribute(AttributeName = "codigoOrgao")]
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

        [DataMember(Name = "nomeOrgao")]
        [XmlAttribute(AttributeName = "nomeOrgao")]
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

        [DataMember(Name = "instancia")]
        [XmlAttribute(AttributeName = "instancia")]
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

        [DataMember(Name = "codigoMunicipioIBGE")]
        [XmlAttribute(AttributeName = "codigoMunicipioIBGE")]
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

    [Serializable]
    [DataContract(Name = "tipoMovimentoNacional")]
    [XmlType(TypeName = "tipoMovimentoNacional")]
    public partial class tipoMovimentoNacional
    {

        private string[] complementoField;

        //private int codigoNacionalField;

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("complemento", IsNullable = true, Order = 0)]
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
        //[System.Xml.Serialization.XmlAttributeAttribute()]
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

    [DebuggerDisplay("{magistradoAtuante},{valorCausa}")]
    [Serializable]
    [DataContract(Name = "tipoCabecalhoProcesso")]
    [XmlType(TypeName = "tipoCabecalhoProcesso")]
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

        //[System.Xml.Serialization.XmlElementAttribute("polo", Order = 0)]
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

        //[System.Xml.Serialization.XmlElementAttribute("assunto", Order = 1)]
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
        //[System.Xml.Serialization.XmlElementAttribute("magistradoAtuante", Order = 2)]
        
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
        //[System.Xml.Serialization.XmlElementAttribute("prioridade", Order = 3)]
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


        //[System.Xml.Serialization.XmlElementAttribute("outroParametro", IsNullable = true, Order = 4)]
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

        //[System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        [DataMember(Name = "valorCausa")]
        [XmlAttribute(AttributeName = "valorCausa")]
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

        //[System.Xml.Serialization.XmlElementAttribute(Order = 6)]
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
        //[System.Xml.Serialization.XmlElementAttribute("outrosnumeros", IsNullable = true, Order = 7)]
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

        ////[System.Xml.Serialization.XmlAttributeAttribute()]
        /// <remarks/>        
        [DataMember(Name = "numero")]
        [XmlAttribute(AttributeName = "numero")]
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

        [DataMember(Name = "competencia")]
        [XmlAttribute(AttributeName = "competencia")]
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

        [DataMember(Name = "classeProcessual")]
        [XmlAttribute(AttributeName = "classeProcessual")]
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

        [DataMember(Name = "codigoLocalidade")]
        [XmlAttribute(AttributeName = "codigoLocalidade")]
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

        [DataMember(Name = "nivelSigilo")]
        [XmlAttribute(AttributeName = "nivelSigilo")]
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

        [DataMember(Name = "intervencaoMP")]
        [XmlAttribute(AttributeName = "intervencaoMP")]
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

        [DataMember(Name = "tamanhoProcesso")]
        [XmlAttribute(AttributeName = "tamanhoProcesso")]
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

        [DataMember(Name = "dataAjuizamento")]
        [XmlAttribute(AttributeName = "dataAjuizamento")]
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

    [Serializable]
    [DataContract(Name = "tipoParametro")]
    [XmlType(TypeName = "tipoParametro")]
    public partial class tipoParametro
    {

        private string nomeField;

        private string valorField;

        [DataMember(Name = "nome")]
        [XmlAttribute(AttributeName = "nome")]
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

        [DataMember(Name = "valor")]
        [XmlAttribute(AttributeName = "valor")]
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

    [Serializable]
    [DataContract(Name = "tipoPoloProcessual")]
    [XmlType(TypeName = "tipoPoloProcessual")]
    public partial class tipoPoloProcessual
    {

        private tipoParte[] parteField;

        private modalidadePoloProcessual poloField;

        private bool poloFieldSpecified;

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("parte", Order = 0)]
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
        //[System.Xml.Serialization.XmlAttributeAttribute()]
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

    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
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

    [Serializable]
    [DataContract(Name = "tipoParte")]
    [XmlType(TypeName = "tipoParte")]
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
        //[System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [DataMember(Name = "interessePublico")]
        [XmlAttribute(AttributeName = "interessePublico")]
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
        //[System.Xml.Serialization.XmlElementAttribute("advogado", IsNullable = true, Order = 2)]
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
        //[System.Xml.Serialization.XmlElementAttribute("pessoaProcessualRelacionada", IsNullable = true, Order = 3)]
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

        [DataMember(Name = "assistenciaJudiciaria")]
        [XmlAttribute(AttributeName = "assistenciaJudiciaria")]
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

        [DataMember(Name = "intimacaoPendente")]
        [XmlAttribute(AttributeName = "intimacaoPendente")]
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
        //[System.Xml.Serialization.XmlAttributeAttribute()]
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


    [Serializable]
    [DataContract(Name = "tipoParteDestinatario")]
    [XmlType(TypeName = "tipoParteDestinatario")]
    public partial class tipoParteDestinatario
    {

        private tipoPessoaDestinatario pessoaField;

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
        //[System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public tipoPessoaDestinatario pessoa
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

        [DataMember(Name = "interessePublico")]
        [XmlAttribute(AttributeName = "interessePublico")]
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
        //[System.Xml.Serialization.XmlElementAttribute("advogado", IsNullable = true, Order = 2)]
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
        //[System.Xml.Serialization.XmlElementAttribute("pessoaProcessualRelacionada", IsNullable = true, Order = 3)]
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

        [DataMember(Name = "assistenciaJudiciaria")]
        [XmlAttribute(AttributeName = "assistenciaJudiciaria")]
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

        [DataMember(Name = "intimacaoPendente")]
        [XmlAttribute(AttributeName = "intimacaoPendente")]
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
        //[System.Xml.Serialization.XmlAttributeAttribute()]
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

    [Serializable]
    [DataContract(Name = "tipoRepresentanteProcessual")]
    [XmlType(TypeName = "tipoRepresentanteProcessual")]
    public partial class tipoRepresentanteProcessual
    {
        private string nomeField;
        private string inscricaoField;
        private string numeroDocumentoPrincipalField;
        private bool intimacaoField;
        private modalidadeRepresentanteProcessual tipoRepresentanteField;
        /// <remarks/>       

        [DataMember(Name = "nome")]
        [XmlAttribute(AttributeName = "nome")]
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

        [DataMember(Name = "inscricao")]
        [XmlAttribute(AttributeName = "inscricao")]
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

        [DataMember(Name = "numeroDocumentoPrincipal")]
        [XmlAttribute(AttributeName = "numeroDocumentoPrincipal")]
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

        [DataMember(Name = "intimacao")]
        [XmlAttribute(AttributeName = "intimacao")]
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
        //[System.Xml.Serialization.XmlAttributeAttribute()]
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

    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
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

    [Serializable]
    [DataContract(Name = "tipoPessoa")]
    [XmlType(TypeName = "tipoPessoa")]
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
        //[System.Xml.Serialization.XmlElementAttribute("outroNome", IsNullable = true, Order = 0)]
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
        //[System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true, Order = 1)]
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
        //[System.Xml.Serialization.XmlElementAttribute("pessoaRelacionada", IsNullable = true, Order = 2)]
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
        //[System.Xml.Serialization.XmlElementAttribute(Order = 3)]
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

        [DataMember(Name = "nome")]
        [XmlAttribute(AttributeName = "nome")]
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
        //[System.Xml.Serialization.XmlAttributeAttribute()]
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

        [DataMember(Name = "nomeGenitor")]
        [XmlAttribute(AttributeName = "nomeGenitor")]
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

        [DataMember(Name = "nomeGenitora")]
        [XmlAttribute(AttributeName = "nomeGenitora")]
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

        [DataMember(Name = "dataNascimento")]
        [XmlAttribute(AttributeName = "dataNascimento")]
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

        [DataMember(Name = "dataObito")]
        [XmlAttribute(AttributeName = "dataObito")]
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

        [DataMember(Name = "numeroDocumentoPrincipal")]
        [XmlAttribute(AttributeName = "numeroDocumentoPrincipal")]
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
        //[System.Xml.Serialization.XmlAttributeAttribute("tipoPessoa")]
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

        [DataMember(Name = "cidadeNatural")]
        [XmlAttribute(AttributeName = "cidadeNatural")]
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

        [DataMember(Name = "estadoNatural")]
        [XmlAttribute(AttributeName = "estadoNatural")]
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

        [DataMember(Name = "nacionalidade")]
        [XmlAttribute(AttributeName = "nacionalidade")]
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

    [Serializable]
    [DataContract(Name = "tipoPessoaDestinatario")]
    [XmlType(TypeName = "tipoPessoaDestinatario")]
    public partial class tipoPessoaDestinatario
    {
        private tipoDocumentoIdentificacao[] documentoField;

        private string nomeField;

        private string numeroDocumentoPrincipalField;

        private tipoQualificacaoPessoa tipoPessoa1Field;

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true, Order = 0)]
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

        [DataMember(Name = "nome")]
        [XmlAttribute(AttributeName = "nome")]
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

        [DataMember(Name = "numeroDocumentoPrincipal")]
        [XmlAttribute(AttributeName = "numeroDocumentoPrincipal")]
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
        //[System.Xml.Serialization.XmlAttributeAttribute("tipoPessoa")]
        public tipoQualificacaoPessoa tipoPessoa
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
    }

    [Serializable]
    [DataContract(Name = "tipoDocumentoIdentificacao")]
    [XmlType(TypeName = "tipoDocumentoIdentificacao")]
    public partial class tipoDocumentoIdentificacao
    {

        private string codigoDocumentoField;

        private string emissorDocumentoField;

        private string tipoDocumentoField;

        private string nomeField;

        [DataMember(Name = "codigoDocumento")]
        [XmlAttribute(AttributeName = "codigoDocumento")]
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

        [DataMember(Name = "emissorDocumento")]
        [XmlAttribute(AttributeName = "emissorDocumento")]
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

        [DataMember(Name = "tipoDocumento")]
        [XmlAttribute(AttributeName = "tipoDocumento")]
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

        [DataMember(Name = "nome")]
        [XmlAttribute(AttributeName = "nome")]
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

    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
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

    [Serializable]
    [DataContract(Name = "tipoRelacionamentoPessoal")]
    [XmlType(TypeName = "tipoRelacionamentoPessoal")]
    public partial class tipoRelacionamentoPessoal
    {

        private tipoPessoa pessoaField;

        private modalidadesRelacionamentoPessoal modalidadeRelacionamentoField;

        private bool modalidadeRelacionamentoFieldSpecified;

        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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
        //[System.Xml.Serialization.XmlAttributeAttribute()]
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
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
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
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
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
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
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
    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.5494")]
    //[System.SerializableAttribute()]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
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

    [Serializable]
    [DataContract(Name = "tipoAssuntoProcessual")]
    [XmlType(TypeName = "tipoAssuntoProcessual")]
    public partial class tipoAssuntoProcessual
    {

        private int codigoNacionalField;

        private bool codigoNacionalFieldSpecified;

        private tipoAssuntoLocal assuntoLocalField;

        private bool principalField;

        private bool principalFieldSpecified;

        [DataMember(Name = "codigoNacional")]
        [XmlAttribute(AttributeName = "codigoNacional")]
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
        //[System.Xml.Serialization.XmlElementAttribute(Order = 1)]
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

        [DataMember(Name = "principal")]
        [XmlAttribute(AttributeName = "principal")]
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

    [Serializable]
    [DataContract(Name = "tipoAssuntoLocal")]
    [XmlType(TypeName = "tipoAssuntoLocal")]
    public partial class tipoAssuntoLocal
    {

        private int codigoAssuntoField;

        private string descricaoField;

        [DataMember(Name = "codigoAssunto")]
        [XmlAttribute(AttributeName = "codigoAssunto")]
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

        [DataMember(Name = "descricao")]
        [XmlAttribute(AttributeName = "descricao")]
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
