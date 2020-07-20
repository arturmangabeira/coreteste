using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.Api.Models
{
    [DataContract]
    public partial class TLogOperacao
    {
        [DataMember]
        public int IdLogOperacao { get; set; }
        [DataMember]
        public int IdTipoOperacao { get; set; }
        [DataMember]
        public int IdTipoRetorno { get; set; }
        [DataMember]
        public string DsLogOperacao { get; set; }
        [DataMember]
        public DateTime DtLogOperacao { get; set; }
        [DataMember]
        public string DsIporigem { get; set; }
        [DataMember]
        public string DsIpdestino { get; set; }
        [DataMember]
        public string DsCaminhoDocumentosChamada { get; set; }
        [DataMember]
        public string DsCaminhoDocumentosRetorno { get; set; }
        [DataMember]
        public bool? FlOperacao { get; set; }
        [DataMember]
        public string CdIdea { get; set; }
        [DataMember]
        public DateTime? DtInicioOperacao { get; set; }
        [DataMember]
        public DateTime? DtFinalOperacao { get; set; }
        [DataMember]
        public virtual TTipoOperacao IdTipoOperacaoNavigation { get; set; }
        [DataMember]
        public virtual TTipoRetorno IdTipoRetornoNavigation { get; set; }
    }
}
