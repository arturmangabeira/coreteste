using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Objects
{
    /// <remarks/>
        public partial class tipoProcessoJudicial 
    {

        private tipoCabecalhoProcesso dadosBasicosField;

        private tipoMovimentoProcessual[] movimentoField;

        private tipoDocumento[] documentoField;

        /// <remarks/>
    
        public tipoCabecalhoProcesso dadosBasicos
        {
            get
            {
                return this.dadosBasicosField;
            }
            set
            {
                this.dadosBasicosField = value;
                this.RaisePropertyChanged("dadosBasicos");
            }
        }

        /// <remarks/>        
        public tipoMovimentoProcessual[] movimento
        {
            get
            {
                return this.movimentoField;
            }
            set
            {
                this.movimentoField = value;
                this.RaisePropertyChanged("movimento");
            }
        }

        /// <remarks/>        
        public tipoDocumento[] documento
        {
            get
            {
                return this.documentoField;
            }
            set
            {
                this.documentoField = value;
                this.RaisePropertyChanged("documento");
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
        
    public partial class consultarProcessoResponse
    {

        
        public bool sucesso;

        
        public string mensagem;

        
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

    /// <remarks/>
    
    public partial class tipoParametro 
    {

        private string nomeField;

        private string valorField;

        /// <remarks/>        
        public string nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
                this.RaisePropertyChanged("nome");
            }
        }

        /// <remarks/>        
        public string valor
        {
            get
            {
                return this.valorField;
            }
            set
            {
                this.valorField = value;
                this.RaisePropertyChanged("valor");
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

    /// <remarks/>
    
    public partial class tipoCabecalhoProcesso
    {

        private tipoPoloProcessual[] poloField;

        private tipoAssuntoProcessual[] assuntoField;

        private string[] magistradoAtuanteField;

        private tipoVinculacaoProcessual[] processoVinculadoField;

        private string[] prioridadeField;

        private tipoParametro[] outroParametroField;

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

        /// <remarks/>        
        public tipoPoloProcessual[] polo
        {
            get
            {
                return this.poloField;
            }
            set
            {
                this.poloField = value;
                this.RaisePropertyChanged("polo");
            }
        }

        /// <remarks/>       
        public tipoAssuntoProcessual[] assunto
        {
            get
            {
                return this.assuntoField;
            }
            set
            {
                this.assuntoField = value;
                this.RaisePropertyChanged("assunto");
            }
        }

        /// <remarks/>        
        public string[] magistradoAtuante
        {
            get
            {
                return this.magistradoAtuanteField;
            }
            set
            {
                this.magistradoAtuanteField = value;
                this.RaisePropertyChanged("magistradoAtuante");
            }
        }

        /// <remarks/>        
        public tipoVinculacaoProcessual[] processoVinculado
        {
            get
            {
                return this.processoVinculadoField;
            }
            set
            {
                this.processoVinculadoField = value;
                this.RaisePropertyChanged("processoVinculado");
            }
        }

        /// <remarks/>        
        public string[] prioridade
        {
            get
            {
                return this.prioridadeField;
            }
            set
            {
                this.prioridadeField = value;
                this.RaisePropertyChanged("prioridade");
            }
        }

        /// <remarks/>       
        public tipoParametro[] outroParametro
        {
            get
            {
                return this.outroParametroField;
            }
            set
            {
                this.outroParametroField = value;
                this.RaisePropertyChanged("outroParametro");
            }
        }

        /// <remarks/>        
        public double valorCausa
        {
            get
            {
                return this.valorCausaField;
            }
            set
            {
                this.valorCausaField = value;
                this.RaisePropertyChanged("valorCausa");
            }
        }

        /// <remarks/>        
        public bool valorCausaSpecified
        {
            get
            {
                return this.valorCausaFieldSpecified;
            }
            set
            {
                this.valorCausaFieldSpecified = value;
                this.RaisePropertyChanged("valorCausaSpecified");
            }
        }

        /// <remarks/>        
        public tipoOrgaoJulgador orgaoJulgador
        {
            get
            {
                return this.orgaoJulgadorField;
            }
            set
            {
                this.orgaoJulgadorField = value;
                this.RaisePropertyChanged("orgaoJulgador");
            }
        }

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
                this.RaisePropertyChanged("outrosnumeros");
            }
        }

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
                this.RaisePropertyChanged("numero");
            }
        }

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
                this.RaisePropertyChanged("competencia");
            }
        }

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
                this.RaisePropertyChanged("competenciaSpecified");
            }
        }

        /// <remarks/>
        
        public int classeProcessual
        {
            get
            {
                return this.classeProcessualField;
            }
            set
            {
                this.classeProcessualField = value;
                this.RaisePropertyChanged("classeProcessual");
            }
        }

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
                this.RaisePropertyChanged("codigoLocalidade");
            }
        }

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
                this.RaisePropertyChanged("nivelSigilo");
            }
        }

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
                this.RaisePropertyChanged("intervencaoMP");
            }
        }

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
                this.RaisePropertyChanged("intervencaoMPSpecified");
            }
        }

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
                this.RaisePropertyChanged("tamanhoProcesso");
            }
        }

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
                this.RaisePropertyChanged("tamanhoProcessoSpecified");
            }
        }

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
                this.RaisePropertyChanged("dataAjuizamento");
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

    /// <remarks/>
    
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

    /// <remarks/>
    
    
    public partial class tipoDocumentoIdentificacao
    {

        private string codigoDocumentoField;

        private string emissorDocumentoField;

        private modalidadeDocumentoIdentificador tipoDocumentoField;

        private string nomeField;

        /// <remarks/>        
        public string codigoDocumento
        {
            get
            {
                return this.codigoDocumentoField;
            }
            set
            {
                this.codigoDocumentoField = value;
                this.RaisePropertyChanged("codigoDocumento");
            }
        }

        /// <remarks/>        
        public string emissorDocumento
        {
            get
            {
                return this.emissorDocumentoField;
            }
            set
            {
                this.emissorDocumentoField = value;
                this.RaisePropertyChanged("emissorDocumento");
            }
        }

        /// <remarks/>        
        public modalidadeDocumentoIdentificador tipoDocumento
        {
            get
            {
                return this.tipoDocumentoField;
            }
            set
            {
                this.tipoDocumentoField = value;
                this.RaisePropertyChanged("tipoDocumento");
            }
        }

        /// <remarks/>
        
        public string nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
                this.RaisePropertyChanged("nome");
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

    /// <remarks/>
    
    
    public partial class tipoPessoa
    {

        private string[] outroNomeField;

        private tipoDocumentoIdentificacao[] documentoField;

        private tipoEndereco[] enderecoField;

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
        public string[] outroNome
        {
            get
            {
                return this.outroNomeField;
            }
            set
            {
                this.outroNomeField = value;
                this.RaisePropertyChanged("outroNome");
            }
        }

        /// <remarks/>        
        public tipoDocumentoIdentificacao[] documento
        {
            get
            {
                return this.documentoField;
            }
            set
            {
                this.documentoField = value;
                this.RaisePropertyChanged("documento");
            }
        }

        /// <remarks/>        
        public tipoEndereco[] endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
                this.RaisePropertyChanged("endereco");
            }
        }

        /// <remarks/>        
        public tipoRelacionamentoPessoal[] pessoaRelacionada
        {
            get
            {
                return this.pessoaRelacionadaField;
            }
            set
            {
                this.pessoaRelacionadaField = value;
                this.RaisePropertyChanged("pessoaRelacionada");
            }
        }

        /// <remarks/>        
        public tipoPessoa pessoaVinculada
        {
            get
            {
                return this.pessoaVinculadaField;
            }
            set
            {
                this.pessoaVinculadaField = value;
                this.RaisePropertyChanged("pessoaVinculada");
            }
        }

        /// <remarks/>
        
        public string nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
                this.RaisePropertyChanged("nome");
            }
        }

        /// <remarks/>
        
        public modalidadeGeneroPessoa sexo
        {
            get
            {
                return this.sexoField;
            }
            set
            {
                this.sexoField = value;
                this.RaisePropertyChanged("sexo");
            }
        }

        /// <remarks/>
        
        public string nomeGenitor
        {
            get
            {
                return this.nomeGenitorField;
            }
            set
            {
                this.nomeGenitorField = value;
                this.RaisePropertyChanged("nomeGenitor");
            }
        }

        /// <remarks/>
        
        public string nomeGenitora
        {
            get
            {
                return this.nomeGenitoraField;
            }
            set
            {
                this.nomeGenitoraField = value;
                this.RaisePropertyChanged("nomeGenitora");
            }
        }

        /// <remarks/>
        
        public string dataNascimento
        {
            get
            {
                return this.dataNascimentoField;
            }
            set
            {
                this.dataNascimentoField = value;
                this.RaisePropertyChanged("dataNascimento");
            }
        }

        /// <remarks/>
        
        public string dataObito
        {
            get
            {
                return this.dataObitoField;
            }
            set
            {
                this.dataObitoField = value;
                this.RaisePropertyChanged("dataObito");
            }
        }

        /// <remarks/>
        
        public string numeroDocumentoPrincipal
        {
            get
            {
                return this.numeroDocumentoPrincipalField;
            }
            set
            {
                this.numeroDocumentoPrincipalField = value;
                this.RaisePropertyChanged("numeroDocumentoPrincipal");
            }
        }

        /// <remarks/>        
        public tipoQualificacaoPessoa tipoPessoa1
        {
            get
            {
                return this.tipoPessoa1Field;
            }
            set
            {
                this.tipoPessoa1Field = value;
                this.RaisePropertyChanged("tipoPessoa1");
            }
        }

        /// <remarks/>
        
        public string cidadeNatural
        {
            get
            {
                return this.cidadeNaturalField;
            }
            set
            {
                this.cidadeNaturalField = value;
                this.RaisePropertyChanged("cidadeNatural");
            }
        }

        /// <remarks/>
        
        public string estadoNatural
        {
            get
            {
                return this.estadoNaturalField;
            }
            set
            {
                this.estadoNaturalField = value;
                this.RaisePropertyChanged("estadoNatural");
            }
        }

        /// <remarks/>
        
        public string nacionalidade
        {
            get
            {
                return this.nacionalidadeField;
            }
            set
            {
                this.nacionalidadeField = value;
                this.RaisePropertyChanged("nacionalidade");
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

    /// <remarks/>
        
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
        
        public tipoPessoa pessoa
        {
            get
            {
                return this.pessoaField;
            }
            set
            {
                this.pessoaField = value;
                this.RaisePropertyChanged("pessoa");
            }
        }

        /// <remarks/>
        
        public string interessePublico
        {
            get
            {
                return this.interessePublicoField;
            }
            set
            {
                this.interessePublicoField = value;
                this.RaisePropertyChanged("interessePublico");
            }
        }

        /// <remarks/>
        
        public tipoRepresentanteProcessual[] advogado
        {
            get
            {
                return this.advogadoField;
            }
            set
            {
                this.advogadoField = value;
                this.RaisePropertyChanged("advogado");
            }
        }

        /// <remarks/>
        
        public tipoParte[] pessoaProcessualRelacionada
        {
            get
            {
                return this.pessoaProcessualRelacionadaField;
            }
            set
            {
                this.pessoaProcessualRelacionadaField = value;
                this.RaisePropertyChanged("pessoaProcessualRelacionada");
            }
        }

        /// <remarks/>
        
        public bool assistenciaJudiciaria
        {
            get
            {
                return this.assistenciaJudiciariaField;
            }
            set
            {
                this.assistenciaJudiciariaField = value;
                this.RaisePropertyChanged("assistenciaJudiciaria");
            }
        }

        /// <remarks/>        
        public bool assistenciaJudiciariaSpecified
        {
            get
            {
                return this.assistenciaJudiciariaFieldSpecified;
            }
            set
            {
                this.assistenciaJudiciariaFieldSpecified = value;
                this.RaisePropertyChanged("assistenciaJudiciariaSpecified");
            }
        }

        /// <remarks/>
        
        public int intimacaoPendente
        {
            get
            {
                return this.intimacaoPendenteField;
            }
            set
            {
                this.intimacaoPendenteField = value;
                this.RaisePropertyChanged("intimacaoPendente");
            }
        }

        /// <remarks/>
        
        public bool intimacaoPendenteSpecified
        {
            get
            {
                return this.intimacaoPendenteFieldSpecified;
            }
            set
            {
                this.intimacaoPendenteFieldSpecified = value;
                this.RaisePropertyChanged("intimacaoPendenteSpecified");
            }
        }

        /// <remarks/>
        
        public modalidadeRelacionamentoProcessual relacionamentoProcessual
        {
            get
            {
                return this.relacionamentoProcessualField;
            }
            set
            {
                this.relacionamentoProcessualField = value;
                this.RaisePropertyChanged("relacionamentoProcessual");
            }
        }

        /// <remarks/>
        
        public bool relacionamentoProcessualSpecified
        {
            get
            {
                return this.relacionamentoProcessualFieldSpecified;
            }
            set
            {
                this.relacionamentoProcessualFieldSpecified = value;
                this.RaisePropertyChanged("relacionamentoProcessualSpecified");
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

    /// <remarks/>
   
    
    public partial class tipoPoloProcessual
    {

        private tipoParte[] parteField;

        private modalidadePoloProcessual poloField;

        private bool poloFieldSpecified;

        /// <remarks/>
        
        public tipoParte[] parte
        {
            get
            {
                return this.parteField;
            }
            set
            {
                this.parteField = value;
                this.RaisePropertyChanged("parte");
            }
        }

        /// <remarks/>
        
        public modalidadePoloProcessual polo
        {
            get
            {
                return this.poloField;
            }
            set
            {
                this.poloField = value;
                this.RaisePropertyChanged("polo");
            }
        }

        /// <remarks/>
        
        public bool poloSpecified
        {
            get
            {
                return this.poloFieldSpecified;
            }
            set
            {
                this.poloFieldSpecified = value;
                this.RaisePropertyChanged("poloSpecified");
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

    /// <remarks/>
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

    /// <remarks/>
    
    public partial class tipoAssuntoProcessual
    {

        private int codigoNacionalField;

        private bool codigoNacionalFieldSpecified;

        private tipoAssuntoLocal assuntoLocalField;

        private bool principalField;

        private bool principalFieldSpecified;

        /// <remarks/>
        
        public int codigoNacional
        {
            get
            {
                return this.codigoNacionalField;
            }
            set
            {
                this.codigoNacionalField = value;
                this.RaisePropertyChanged("codigoNacional");
            }
        }

        /// <remarks/>
        
        public bool codigoNacionalSpecified
        {
            get
            {
                return this.codigoNacionalFieldSpecified;
            }
            set
            {
                this.codigoNacionalFieldSpecified = value;
                this.RaisePropertyChanged("codigoNacionalSpecified");
            }
        }

        /// <remarks/>        
        public tipoAssuntoLocal assuntoLocal
        {
            get
            {
                return this.assuntoLocalField;
            }
            set
            {
                this.assuntoLocalField = value;
                this.RaisePropertyChanged("assuntoLocal");
            }
        }

        /// <remarks/>
        
        public bool principal
        {
            get
            {
                return this.principalField;
            }
            set
            {
                this.principalField = value;
                this.RaisePropertyChanged("principal");
            }
        }

        /// <remarks/>
        
        public bool principalSpecified
        {
            get
            {
                return this.principalFieldSpecified;
            }
            set
            {
                this.principalFieldSpecified = value;
                this.RaisePropertyChanged("principalSpecified");
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

    /// <remarks/>

    public partial class tipoAssuntoLocal
    {

        private tipoAssuntoLocal assuntoLocalPaiField;

        private int codigoAssuntoField;

        private int codigoPaiNacionalField;

        private string descricaoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public tipoAssuntoLocal assuntoLocalPai
        {
            get
            {
                return this.assuntoLocalPaiField;
            }
            set
            {
                this.assuntoLocalPaiField = value;
                this.RaisePropertyChanged("assuntoLocalPai");
            }
        }

        /// <remarks/>
        
        public int codigoAssunto
        {
            get
            {
                return this.codigoAssuntoField;
            }
            set
            {
                this.codigoAssuntoField = value;
                this.RaisePropertyChanged("codigoAssunto");
            }
        }

        /// <remarks/>
        
        public int codigoPaiNacional
        {
            get
            {
                return this.codigoPaiNacionalField;
            }
            set
            {
                this.codigoPaiNacionalField = value;
                this.RaisePropertyChanged("codigoPaiNacional");
            }
        }

        /// <remarks/>
        
        public string descricao
        {
            get
            {
                return this.descricaoField;
            }
            set
            {
                this.descricaoField = value;
                this.RaisePropertyChanged("descricao");
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

    /// <remarks/>
      
    public partial class tipoVinculacaoProcessual
    {

        private string numeroProcessoField;

        private modalidadeVinculacaoProcesso vinculoField;

        /// <remarks/>
        
        public string numeroProcesso
        {
            get
            {
                return this.numeroProcessoField;
            }
            set
            {
                this.numeroProcessoField = value;
                this.RaisePropertyChanged("numeroProcesso");
            }
        }

        /// <remarks/>
        
        public modalidadeVinculacaoProcesso vinculo
        {
            get
            {
                return this.vinculoField;
            }
            set
            {
                this.vinculoField = value;
                this.RaisePropertyChanged("vinculo");
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

    /// <remarks/>
    public enum modalidadeVinculacaoProcesso
    {

        /// <remarks/>
        CX,

        /// <remarks/>
        CT,

        /// <remarks/>
        DP,

        /// <remarks/>
        AR,

        /// <remarks/>
        CD,

        /// <remarks/>
        OR,

        /// <remarks/>
        RR,

        /// <remarks/>
        RG,
    }

    /// <remarks/>
    
    public partial class tipoOrgaoJulgador
    {

        private string codigoOrgaoField;

        private string nomeOrgaoField;

        private string instanciaField;

        private int codigoMunicipioIBGEField;

        /// <remarks/>
        
        public string codigoOrgao
        {
            get
            {
                return this.codigoOrgaoField;
            }
            set
            {
                this.codigoOrgaoField = value;
                this.RaisePropertyChanged("codigoOrgao");
            }
        }

        /// <remarks/>
        
        public string nomeOrgao
        {
            get
            {
                return this.nomeOrgaoField;
            }
            set
            {
                this.nomeOrgaoField = value;
                this.RaisePropertyChanged("nomeOrgao");
            }
        }

        /// <remarks/>
        
        public string instancia
        {
            get
            {
                return this.instanciaField;
            }
            set
            {
                this.instanciaField = value;
                this.RaisePropertyChanged("instancia");
            }
        }

        /// <remarks/>
        
        public int codigoMunicipioIBGE
        {
            get
            {
                return this.codigoMunicipioIBGEField;
            }
            set
            {
                this.codigoMunicipioIBGEField = value;
                this.RaisePropertyChanged("codigoMunicipioIBGE");
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

    /// <remarks/>
    public partial class tipoRepresentanteProcessual
    {

        private tipoEndereco[] enderecoField;

        private string nomeField;

        private string inscricaoField;

        private string numeroDocumentoPrincipalField;

        private bool intimacaoField;

        private modalidadeRepresentanteProcessual tipoRepresentanteField;

        /// <remarks/>        
        public tipoEndereco[] endereco
        {
            get
            {
                return this.enderecoField;
            }
            set
            {
                this.enderecoField = value;
                this.RaisePropertyChanged("endereco");
            }
        }

        /// <remarks/>
        
        public string nome
        {
            get
            {
                return this.nomeField;
            }
            set
            {
                this.nomeField = value;
                this.RaisePropertyChanged("nome");
            }
        }

        /// <remarks/>
        
        public string inscricao
        {
            get
            {
                return this.inscricaoField;
            }
            set
            {
                this.inscricaoField = value;
                this.RaisePropertyChanged("inscricao");
            }
        }

        /// <remarks/>
        
        public string numeroDocumentoPrincipal
        {
            get
            {
                return this.numeroDocumentoPrincipalField;
            }
            set
            {
                this.numeroDocumentoPrincipalField = value;
                this.RaisePropertyChanged("numeroDocumentoPrincipal");
            }
        }

        /// <remarks/>
        
        public bool intimacao
        {
            get
            {
                return this.intimacaoField;
            }
            set
            {
                this.intimacaoField = value;
                this.RaisePropertyChanged("intimacao");
            }
        }

        /// <remarks/>
        
        public modalidadeRepresentanteProcessual tipoRepresentante
        {
            get
            {
                return this.tipoRepresentanteField;
            }
            set
            {
                this.tipoRepresentanteField = value;
                this.RaisePropertyChanged("tipoRepresentante");
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

    /// <remarks/>
     public partial class tipoEndereco
    {

        private string logradouroField;

        private string numeroField;

        private string complementoField;

        private string bairroField;

        private string cidadeField;

        private string estadoField;

        private string paisField;

        private string cepField;

        /// <remarks/>
        
        public string logradouro
        {
            get
            {
                return this.logradouroField;
            }
            set
            {
                this.logradouroField = value;
                this.RaisePropertyChanged("logradouro");
            }
        }

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
                this.RaisePropertyChanged("numero");
            }
        }

        /// <remarks/>
        
        public string complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;
                this.RaisePropertyChanged("complemento");
            }
        }

        /// <remarks/>
        
        public string bairro
        {
            get
            {
                return this.bairroField;
            }
            set
            {
                this.bairroField = value;
                this.RaisePropertyChanged("bairro");
            }
        }

        /// <remarks/>
        
        public string cidade
        {
            get
            {
                return this.cidadeField;
            }
            set
            {
                this.cidadeField = value;
                this.RaisePropertyChanged("cidade");
            }
        }

        /// <remarks/>
        
        public string estado
        {
            get
            {
                return this.estadoField;
            }
            set
            {
                this.estadoField = value;
                this.RaisePropertyChanged("estado");
            }
        }

        /// <remarks/>
        
        public string pais
        {
            get
            {
                return this.paisField;
            }
            set
            {
                this.paisField = value;
                this.RaisePropertyChanged("pais");
            }
        }

        /// <remarks/>
        
        public string cep
        {
            get
            {
                return this.cepField;
            }
            set
            {
                this.cepField = value;
                this.RaisePropertyChanged("cep");
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

    /// <remarks/>
   
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

    /// <remarks/>

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
                this.RaisePropertyChanged("pessoa");
            }
        }

        /// <remarks/>
        
        public modalidadesRelacionamentoPessoal modalidadeRelacionamento
        {
            get
            {
                return this.modalidadeRelacionamentoField;
            }
            set
            {
                this.modalidadeRelacionamentoField = value;
                this.RaisePropertyChanged("modalidadeRelacionamento");
            }
        }

        /// <remarks/>
        
        public bool modalidadeRelacionamentoSpecified
        {
            get
            {
                return this.modalidadeRelacionamentoFieldSpecified;
            }
            set
            {
                this.modalidadeRelacionamentoFieldSpecified = value;
                this.RaisePropertyChanged("modalidadeRelacionamentoSpecified");
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

    /// <remarks/>
      
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

    /// <remarks/>

    public partial class tipoMovimentoNacional
    {

        private string[] complementoField;

        private int codigoNacionalField;

        /// <remarks/>
        
        public string[] complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;
                this.RaisePropertyChanged("complemento");
            }
        }

        /// <remarks/>
        
        public int codigoNacional
        {
            get
            {
                return this.codigoNacionalField;
            }
            set
            {
                this.codigoNacionalField = value;
                this.RaisePropertyChanged("codigoNacional");
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

    /// <remarks/>

    public partial class tipoMovimentoLocal
    {

        private tipoMovimentoLocal movimentoLocalPaiField;

        private int codigoMovimentoField;

        private int codigoPaiNacionalField;

        private string descricaoField;

        /// <remarks/>
        
        public tipoMovimentoLocal movimentoLocalPai
        {
            get
            {
                return this.movimentoLocalPaiField;
            }
            set
            {
                this.movimentoLocalPaiField = value;
                this.RaisePropertyChanged("movimentoLocalPai");
            }
        }

        /// <remarks/>
        
        public int codigoMovimento
        {
            get
            {
                return this.codigoMovimentoField;
            }
            set
            {
                this.codigoMovimentoField = value;
                this.RaisePropertyChanged("codigoMovimento");
            }
        }

        /// <remarks/>
        
        public int codigoPaiNacional
        {
            get
            {
                return this.codigoPaiNacionalField;
            }
            set
            {
                this.codigoPaiNacionalField = value;
                this.RaisePropertyChanged("codigoPaiNacional");
            }
        }

        /// <remarks/>
        
        public string descricao
        {
            get
            {
                return this.descricaoField;
            }
            set
            {
                this.descricaoField = value;
                this.RaisePropertyChanged("descricao");
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

    /// <remarks/>

    public partial class tipoMovimentoProcessual
    {

        private string[] complementoField;

        private string dataHoraField;

        /// <remarks/>
        
        public string[] complemento
        {
            get
            {
                return this.complementoField;
            }
            set
            {
                this.complementoField = value;
                this.RaisePropertyChanged("complemento");
            }
        }

        
        /// <remarks/>
        
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

    /// <remarks/>

    public partial class tipoAssinatura
    {

        

        private string assinaturaField;

        private string dataAssinaturaField;

        private string cadeiaCertificadoField;

        private string algoritmoHashField;

        private string codificacaoCertificadoField;

        /// <remarks/>
           
        /// <remarks/>
        
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

    /// <remarks/>

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
        
        public byte[] conteudo
        {
            get
            {
                return this.conteudoField;
            }
            set
            {
                this.conteudoField = value;
                this.RaisePropertyChanged("conteudo");
            }
        }

        /// <remarks/>
        
        public tipoAssinatura[] assinatura
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
        
        public tipoParametro[] outroParametro
        {
            get
            {
                return this.outroParametroField;
            }
            set
            {
                this.outroParametroField = value;
                this.RaisePropertyChanged("outroParametro");
            }
        }

        /// <remarks/>
        
        public System.Xml.XmlElement Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        
        public tipoDocumento[] documentoVinculado
        {
            get
            {
                return this.documentoVinculadoField;
            }
            set
            {
                this.documentoVinculadoField = value;
                this.RaisePropertyChanged("documentoVinculado");
            }
        }

        /// <remarks/>
        
        public string idDocumento
        {
            get
            {
                return this.idDocumentoField;
            }
            set
            {
                this.idDocumentoField = value;
                this.RaisePropertyChanged("idDocumento");
            }
        }

        /// <remarks/>
        
        public string idDocumentoVinculado
        {
            get
            {
                return this.idDocumentoVinculadoField;
            }
            set
            {
                this.idDocumentoVinculadoField = value;
                this.RaisePropertyChanged("idDocumentoVinculado");
            }
        }

        /// <remarks/>
        
        public string tipoDocumento1
        {
            get
            {
                return this.tipoDocumento1Field;
            }
            set
            {
                this.tipoDocumento1Field = value;
                this.RaisePropertyChanged("tipoDocumento1");
            }
        }

        /// <remarks/>
        
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

        /// <remarks/>
        
        public string mimetype
        {
            get
            {
                return this.mimetypeField;
            }
            set
            {
                this.mimetypeField = value;
                this.RaisePropertyChanged("mimetype");
            }
        }

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
                this.RaisePropertyChanged("nivelSigilo");
            }
        }

        /// <remarks/>
        
        public bool nivelSigiloSpecified
        {
            get
            {
                return this.nivelSigiloFieldSpecified;
            }
            set
            {
                this.nivelSigiloFieldSpecified = value;
                this.RaisePropertyChanged("nivelSigiloSpecified");
            }
        }

        /// <remarks/>
        
        public int movimento
        {
            get
            {
                return this.movimentoField;
            }
            set
            {
                this.movimentoField = value;
                this.RaisePropertyChanged("movimento");
            }
        }

        /// <remarks/>
        
        public bool movimentoSpecified
        {
            get
            {
                return this.movimentoFieldSpecified;
            }
            set
            {
                this.movimentoFieldSpecified = value;
                this.RaisePropertyChanged("movimentoSpecified");
            }
        }

        /// <remarks/>
        
        public string hash
        {
            get
            {
                return this.hashField;
            }
            set
            {
                this.hashField = value;
                this.RaisePropertyChanged("hash");
            }
        }

        /// <remarks/>
        
        public string descricao
        {
            get
            {
                return this.descricaoField;
            }
            set
            {
                this.descricaoField = value;
                this.RaisePropertyChanged("descricao");
            }
        }

        /// <remarks/>
        
        public string tipoDocumentoLocal
        {
            get
            {
                return this.tipoDocumentoLocalField;
            }
            set
            {
                this.tipoDocumentoLocalField = value;
                this.RaisePropertyChanged("tipoDocumentoLocal");
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
}
