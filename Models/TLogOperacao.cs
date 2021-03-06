﻿using System;
using System.Runtime.Serialization;

namespace IntegradorIdea.Models
{
    [DataContract]
    public partial class TLogOperacao
    {
        [DataMember]
        public int IdLogOperacao { get; set; }
        [DataMember]
        public int IdTpOperacao { get; set; }
        [DataMember]
        public int IdTpRetorno { get; set; }
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
        public int? CdIdea { get; set; }
        [DataMember]
        public DateTime? DtInicioOperacao { get; set; }
        [DataMember]
        public DateTime? DtFinalOperacao { get; set; }
        [DataMember]
        public virtual tTpOperacao IdTipoOperacaoNavigation { get; set; }
        [DataMember]
        public virtual tTpRetorno IdTipoRetornoNavigation { get; set; }
    }
}
