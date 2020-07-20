using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.Api.Models
{
    [DataContract]
    public partial class TTipoRetorno
    {

        public TTipoRetorno()
        {
            TLogOperacao = new HashSet<TLogOperacao>();
        }
        [DataMember]
        public int IdTipoRetorno { get; set; }
        [DataMember]
        public int CdTipoRetorno { get; set; }
        [DataMember]
        public string NmTipoRetorno { get; set; }
        [DataMember]
        public DateTime DtCadastro { get; set; }
        [DataMember]

        public virtual ICollection<TLogOperacao> TLogOperacao { get; set; }
    }
}
