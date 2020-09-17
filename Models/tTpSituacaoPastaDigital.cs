using System;

using System.Collections.Generic;



namespace IntegradorIdea.Models

{

    public partial class tTpSituacaoPastaDigital

    {

        public tTpSituacaoPastaDigital()

        {

            TFilaPastaDigital = new HashSet<TFilaPastaDigital>();

        }



        public int IdTpSituacaoPastaDigital { get; set; }

        public string NmTpSituacaoPastaDigital { get; set; }

        public string DsCor { get; set; }

        public DateTime DtCadastro { get; set; }



        public virtual ICollection<TFilaPastaDigital> TFilaPastaDigital { get; set; }

    }

}

