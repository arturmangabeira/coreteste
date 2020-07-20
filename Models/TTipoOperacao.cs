using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Core.Api.Models
{
    [DataContract]
    public partial class TTipoOperacao
    {
        public TTipoOperacao()
        {
            TLogOperacao = new HashSet<TLogOperacao>();
        }

        [DataMember]
        public int IdTipoOperacao { get; set; }
        [DataMember]
        public string NmTipoOperacao { get; set; }
        [DataMember]
        public DateTime DtCadastro { get; set; }

        [DataMember]
        public virtual ICollection<TLogOperacao> TLogOperacao { get; set; }
    }
}
