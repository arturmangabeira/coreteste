using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Entidades
{
    /// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class Message : EntidadeBase 
    {
    
    private MessageIdType messageIdField;
    
    private MessageMessageBody messageBodyField;

    private SignatureType signatureField;
    
    /// <remarks/>
    public MessageIdType MessageId {
        get {
            return this.messageIdField;
        }
        set {
            this.messageIdField = value;
        }
    }
    
    /// <remarks/>
    public MessageMessageBody MessageBody {
        get {
            return this.messageBodyField;
        }
        set {
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
public partial class MessageIdType {
    
    private ServiceAjuizamentoIdType serviceIdField;
    
    private VersionType versionField;
    
    private string msgDescField;
    
    private string codeField;
    
    private string fromAddressField;
    
    private string toAddressField;
    
    private string dateField;
    
    /// <remarks/>
    public ServiceAjuizamentoIdType ServiceId {
        get {
            return this.serviceIdField;
        }
        set {
            this.serviceIdField = value;
        }
    }
    
    /// <remarks/>
    public VersionType Version {
        get {
            return this.versionField;
        }
        set {
            this.versionField = value;
        }
    }
    
    /// <remarks/>
    public string MsgDesc {
        get {
            return this.msgDescField;
        }
        set {
            this.msgDescField = value;
        }
    }
    
    /// <remarks/>
    public string Code {
        get {
            return this.codeField;
        }
        set {
            this.codeField = value;
        }
    }
    
    /// <remarks/>
    public string FromAddress {
        get {
            return this.fromAddressField;
        }
        set {
            this.fromAddressField = value;
        }
    }
    
    /// <remarks/>
    public string ToAddress {
        get {
            return this.toAddressField;
        }
        set {
            this.toAddressField = value;
        }
    }
    
    /// <remarks/>
    public string Date {
        get {
            return this.dateField;
        }
        set {
            this.dateField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
public enum ServiceAjuizamentoIdType {
    
    /// <remarks/>
    Ajuizamento,
    SolicitaLogon,
    ConfirmaLogon,
    ConsultaProcesso,
   
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
public enum VersionType {
    
    /// <remarks/>
    [System.Xml.Serialization.XmlEnumAttribute("1.0")]
    Item10,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class DocumentoType {
    
    private string tipoField;
    
    private string nomeField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Tipo {
        get {
            return this.tipoField;
        }
        set {
            this.tipoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Nome {
        get {
            return this.nomeField;
        }
        set {
            this.nomeField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class TermoInscricaoType {
    
    private string credTributarioField;
    
    private string numeroField;
    
    private string dataField;
    
    private string valorField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CredTributario {
        get {
            return this.credTributarioField;
        }
        set {
            this.credTributarioField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Numero {
        get {
            return this.numeroField;
        }
        set {
            this.numeroField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Data {
        get {
            return this.dataField;
        }
        set {
            this.dataField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Valor {
        get {
            return this.valorField;
        }
        set {
            this.valorField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class CDAType {
    
    private TermoInscricaoType[] termosInscricaoField;
    
    private string numeroField;
    
    private string dataField;
    
    private string valorField;
    
    private string dataAtualizacaoField;
    
    private string valorAtualizadoField;
    
    private string nomeDocumentoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("TermoInscricao", IsNullable=false)]
    public TermoInscricaoType[] TermosInscricao {
        get {
            return this.termosInscricaoField;
        }
        set {
            this.termosInscricaoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Numero {
        get {
            return this.numeroField;
        }
        set {
            this.numeroField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Data {
        get {
            return this.dataField;
        }
        set {
            this.dataField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Valor {
        get {
            return this.valorField;
        }
        set {
            this.valorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DataAtualizacao {
        get {
            return this.dataAtualizacaoField;
        }
        set {
            this.dataAtualizacaoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ValorAtualizado {
        get {
            return this.valorAtualizadoField;
        }
        set {
            this.valorAtualizadoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NomeDocumento {
        get {
            return this.nomeDocumentoField;
        }
        set {
            this.nomeDocumentoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class CitacaoType {
    
    private CitacaoFormaType formaField;
    
    private bool formaFieldSpecified;
    
    private string tipoField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public CitacaoFormaType Forma {
        get {
            return this.formaField;
        }
        set {
            this.formaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool FormaSpecified {
        get {
            return this.formaFieldSpecified;
        }
        set {
            this.formaFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Tipo {
        get {
            return this.tipoField;
        }
        set {
            this.tipoField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
public enum CitacaoFormaType {
    
    /// <remarks/>
    PRECATORIA,
    
    /// <remarks/>
    MANDADO,
    
    /// <remarks/>
    OFICIO,
    
    /// <remarks/>
    EDITAL,
    
    /// <remarks/>
    PORTAL,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PessoaTelefoneType {
    
    private string numeroField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Numero {
        get {
            return this.numeroField;
        }
        set {
            this.numeroField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PessoaDocumentoType {
    
    private string tipoField;
    
    private string orgaoExpedidorField;
    
    private string numeroField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Tipo {
        get {
            return this.tipoField;
        }
        set {
            this.tipoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string OrgaoExpedidor {
        get {
            return this.orgaoExpedidorField;
        }
        set {
            this.orgaoExpedidorField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Numero {
        get {
            return this.numeroField;
        }
        set {
            this.numeroField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class EnderecoType {
    
    private string logradouroField;
    
    private string numeroField;
    
    private string complementoField;
    
    private string bairroField;
    
    private string municipioField;
    
    private string ufField;
    
    private string cEPField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Logradouro {
        get {
            return this.logradouroField;
        }
        set {
            this.logradouroField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Numero {
        get {
            return this.numeroField;
        }
        set {
            this.numeroField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Complemento {
        get {
            return this.complementoField;
        }
        set {
            this.complementoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Bairro {
        get {
            return this.bairroField;
        }
        set {
            this.bairroField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Municipio {
        get {
            return this.municipioField;
        }
        set {
            this.municipioField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string UF {
        get {
            return this.ufField;
        }
        set {
            this.ufField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string CEP {
        get {
            return this.cEPField;
        }
        set {
            this.cEPField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class PessoaType {
    
    private EnderecoType enderecoField;
    
    private PessoaDocumentoType[] documentoField;
    
    private PessoaTelefoneType telefoneField;
    
    private PessoaTipoType tipoPessoaField;
    
    private string nomeField;
    
    private PessoaGeneroType generoField;
    
    private bool generoFieldSpecified;
    
    private string eMailField;
    
    /// <remarks/>
    public EnderecoType Endereco {
        get {
            return this.enderecoField;
        }
        set {
            this.enderecoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Documento")]
    public PessoaDocumentoType[] Documento {
        get {
            return this.documentoField;
        }
        set {
            this.documentoField = value;
        }
    }
    
    /// <remarks/>
    public PessoaTelefoneType Telefone {
        get {
            return this.telefoneField;
        }
        set {
            this.telefoneField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PessoaTipoType TipoPessoa {
        get {
            return this.tipoPessoaField;
        }
        set {
            this.tipoPessoaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Nome {
        get {
            return this.nomeField;
        }
        set {
            this.nomeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public PessoaGeneroType Genero {
        get {
            return this.generoField;
        }
        set {
            this.generoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool GeneroSpecified {
        get {
            return this.generoFieldSpecified;
        }
        set {
            this.generoFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string EMail {
        get {
            return this.eMailField;
        }
        set {
            this.eMailField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
public enum PessoaTipoType {
    
    /// <remarks/>
    Fisica,
    
    /// <remarks/>
    Juridica,
    
    /// <remarks/>
    Outros,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
public enum PessoaGeneroType {
    
    /// <remarks/>
    Masculino,
    
    /// <remarks/>
    Feminino,
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ParteType {
    
    private PessoaType pessoaField;
    
    private CitacaoType citacaoField;
    
    private string parteIDField;
    
    private string requerBacenJudField;
    
    /// <remarks/>
    public PessoaType Pessoa {
        get {
            return this.pessoaField;
        }
        set {
            this.pessoaField = value;
        }
    }
    
    /// <remarks/>
    public CitacaoType Citacao {
        get {
            return this.citacaoField;
        }
        set {
            this.citacaoField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ParteID {
        get {
            return this.parteIDField;
        }
        set {
            this.parteIDField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string RequerBacenJud {
        get {
            return this.requerBacenJudField;
        }
        set {
            this.requerBacenJudField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class ProcessoType {
    
    private string classeField;
    
    private string assuntoPrincipalField;
    
    private string[] outrosAssuntosField;
    
    private ProcessoTypePartes partesField;
    
    private CDAType[] cDAsField;
    
    private DocumentoType[] documentosField;
    
    private string foroField;
    
    private string valorCausaField;
    
    private string nomePeticaoInicialField;
    
    private string competenciaField;
    
    private string dataValorCausaField;
    
    /// <remarks/>
    public string Classe {
        get {
            return this.classeField;
        }
        set {
            this.classeField = value;
        }
    }
    
    /// <remarks/>
    public string AssuntoPrincipal {
        get {
            return this.assuntoPrincipalField;
        }
        set {
            this.assuntoPrincipalField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Assunto", IsNullable=false)]
    public string[] OutrosAssuntos {
        get {
            return this.outrosAssuntosField;
        }
        set {
            this.outrosAssuntosField = value;
        }
    }
    
    /// <remarks/>
    public ProcessoTypePartes Partes {
        get {
            return this.partesField;
        }
        set {
            this.partesField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("CDA", IsNullable=false)]
    public CDAType[] CDAs {
        get {
            return this.cDAsField;
        }
        set {
            this.cDAsField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Documento", IsNullable=false)]
    public DocumentoType[] Documentos {
        get {
            return this.documentosField;
        }
        set {
            this.documentosField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Foro {
        get {
            return this.foroField;
        }
        set {
            this.foroField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ValorCausa {
        get {
            return this.valorCausaField;
        }
        set {
            this.valorCausaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string NomePeticaoInicial {
        get {
            return this.nomePeticaoInicialField;
        }
        set {
            this.nomePeticaoInicialField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string Competencia {
        get {
            return this.competenciaField;
        }
        set {
            this.competenciaField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string DataValorCausa {
        get {
            return this.dataValorCausaField;
        }
        set {
            this.dataValorCausaField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class ProcessoTypePartes {
    
    private ParteType[] partesAtivasField;
    
    private ParteType[] partesPassivasField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Parte", IsNullable=false)]
    public ParteType[] PartesAtivas {
        get {
            return this.partesAtivasField;
        }
        set {
            this.partesAtivasField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Parte", IsNullable=false)]
    public ParteType[] PartesPassivas {
        get {
            return this.partesPassivasField;
        }
        set {
            this.partesPassivasField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
public partial class MessageMessageBody {
    
    private ProcessoType processoField;
    
    /// <remarks/>
    public ProcessoType Processo {
        get {
            return this.processoField;
        }
        set {
            this.processoField = value;
        }
    }
}
}
