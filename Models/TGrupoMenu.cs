using System;
using System.Collections.Generic;

namespace IntegradorIdea.Models
{
    public partial class TGrupoMenu
    {
        public TGrupoMenu()
        {
            TTela = new HashSet<TTela>();
        }

        public int IdGrupoMenu { get; set; }
        public string DsIcone { get; set; }
        public string NmGrupoMenu { get; set; }
        public short NuOrdem { get; set; }
        public DateTime DtCadastro { get; set; }

        public virtual ICollection<TTela> TTela { get; set; }
    }
}
