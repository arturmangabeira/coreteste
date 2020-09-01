using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IntegradorIdea.Models
{
    [DataContract]
    public partial class tTpRetorno
    {

        public tTpRetorno()
        {
            TLogOperacao = new HashSet<TLogOperacao>();
        }
        [DataMember]
        public int IdTipoRetorno { get; set; }
        [DataMember]
        public int CdTpRetorno { get; set; }
        [DataMember]
        public string NmTpRetorno { get; set; }
        [DataMember]
        public DateTime DtCadastro { get; set; }
        [DataMember]

        public virtual ICollection<TLogOperacao> TLogOperacao { get; set; }
    }
}
