using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class ConsultarProcessoResponse
    {
        public bool sucesso { get; set; }

        public string mensagem { get; set; }

        public tipoProcessoJudicial processo { get; set; }

        public ConsultarProcessoResponse()
        {
        }

        public ConsultarProcessoResponse(bool sucesso, string mensagem, tipoProcessoJudicial processo)
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
    [DataContract(Name = "movimento")]
    [XmlType(TypeName = "movimento")]
    public partial class tipoMovimentoProcessual
    {

        private string complementoField;

        private tipoMovimentoNacional movimentoNacionalField;

        private string dataHoraField;

        public string complemento
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
    [XmlType(TypeName = "TipoDocumento")]
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

        public tipoParametro[] outroParametros
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
        
        public tipoPoloProcessual[] polos
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


        public tipoAssuntoProcessual[] assuntos
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

        public tipoParametro[] outrosParametros
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
    [DataContract(Name = "outroParametro")]
    [XmlType(TypeName = "outroParametro")]
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

    [DebuggerDisplay("{polo}")]
    [Serializable]
    [DataContract(Name = "polo")]
    [XmlType(TypeName = "polo")]
    public partial class tipoPoloProcessual
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private tipoParte[] parteField;

        private modalidadePoloProcessual poloField;

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private bool poloFieldSpecified;

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

        [DataMember(Name = "polo")]
        [XmlAttribute(AttributeName = "polo")]
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

    public enum modalidadePoloProcessual
    {
        AT,
        PA,
        TC,
        FL,
        TJ,
        AD,
        VI,
    }

    [Serializable]
    [DataContract(Name = "pessoas")]
    [XmlType(TypeName = "pessoas")]
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

    public enum modalidadeRepresentanteProcessual
    {
        A,
        E,
        M,
        D,
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
        
        public tipoDocumentoIdentificacao[] documentos
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

        [DataMember(Name = "sexo")]
        [XmlAttribute(AttributeName = "sexo")]
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

        [DataMember(Name = "tipoPessoa")]
        [XmlAttribute(AttributeName = "tipoPessoa")]
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

        [DataMember(Name = "tipoPessoa")]
        [XmlAttribute(AttributeName = "tipoPessoa")]
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
    [DataContract(Name = "documento")]
    [XmlType(TypeName = "documento")]
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

    public enum modalidadeDocumentoIdentificador
    {
        CI,
        CNH,
        TE,
        CN,
        CC,
        PAS,
        CT,
        RIC,
        CMF,
        PIS_PASEP,
        CEI,
        NIT,
        CP,
        IF,
        OAB,
        RJC,
        RGE
    }

    [Serializable]
    [DataContract(Name = "tipoRelacionamentoPessoal")]
    [XmlType(TypeName = "tipoRelacionamentoPessoal")]
    public partial class tipoRelacionamentoPessoal
    {

        private tipoPessoa pessoaField;

        private modalidadesRelacionamentoPessoal modalidadeRelacionamentoField;

        private bool modalidadeRelacionamentoFieldSpecified;

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

    public enum modalidadesRelacionamentoPessoal
    {
        P,
        AP,
        SP,
        T,
        C
    }

    public enum modalidadeGeneroPessoa
    {
        M,
        F,
        D,
    }

    public enum tipoQualificacaoPessoa
    {
        fisica,
        juridica,
        autoridade,
        orgaorepresentacao
    }

    public enum modalidadeRelacionamentoProcessual
    {
        CP,
        RP,
        TF,
        AT,
        AS
    }

    [Serializable]
    [DataContract(Name = "assunto")]
    [XmlType(TypeName = "assunto")]
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
