using System;
using System.Collections.Generic;

namespace IntegradorIdea.Models
{
    public partial class TUsuario
    {
        public TUsuario()
        {
            TLogAtividade = new HashSet<TLogAtividade>();
        }

        public int IdUsuario { get; set; }
        public int IdPerfil { get; set; }
        public string NmUsuario { get; set; }
        public string DsLogin { get; set; }
        public string DsSenha { get; set; }
        public DateTime DtCadastro { get; set; }
        public DateTime? DtUltimoAcesso { get; set; }

        public virtual TPerfil IdPerfilNavigation { get; set; }
        public virtual ICollection<TLogAtividade> TLogAtividade { get; set; }
    }
}
