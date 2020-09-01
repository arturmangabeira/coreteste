using System;
using System.Collections.Generic;

namespace IntegradorIdea.Models
{
    public partial class TServidor
    {
        public TServidor()
        {
            TFilaPastaDigital = new HashSet<TFilaPastaDigital>();
        }

        public int IdServidor { get; set; }
        public string NmServidor { get; set; }
        public string DsServidor { get; set; }
        public string DsIpservidor { get; set; }
        public string DsEnderecoWsdl { get; set; }
        public string SgFlSituacao { get; set; }
        public int? NuOrdem { get; set; }
        public DateTime? DtUltimaReinicializacao { get; set; }
        public DateTime DtCadastro { get; set; }

        public virtual ICollection<TFilaPastaDigital> TFilaPastaDigital { get; set; }
    }
}
