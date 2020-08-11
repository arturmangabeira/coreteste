using System;
using System.Collections.Generic;

namespace IntegradorIdea.Models
{
    public partial class TSituacaoPastaDigital
    {
        public TSituacaoPastaDigital()
        {
            TFilaPastaDigital = new HashSet<TFilaPastaDigital>();
        }

        public int IdSituacaoPastaDigital { get; set; }
        public string NmSituacaoPastaDigital { get; set; }
        public string DsCor { get; set; }
        public DateTime DtCadastro { get; set; }

        public virtual ICollection<TFilaPastaDigital> TFilaPastaDigital { get; set; }
    }
}
