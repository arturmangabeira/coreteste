using System;

using System.Collections.Generic;

using System.Runtime.Serialization;



namespace IntegradorIdea.Models

{

    [DataContract]

    public partial class tTpOperacao

    {

        public tTpOperacao()

        {

            TLogOperacao = new HashSet<TLogOperacao>();

        }



        [DataMember]

        public int IdTpOperacao { get; set; }

        [DataMember]

        public string NmTpOperacao { get; set; }

        [DataMember]

        public DateTime DtCadastro { get; set; }



        [DataMember]

        public virtual ICollection<TLogOperacao> TLogOperacao { get; set; }

    }

}

