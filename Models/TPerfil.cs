using System;
using System.Collections.Generic;

namespace Core.Api.Models
{
    public partial class TPerfil
    {
        public TPerfil()
        {
            TPerfilTela = new HashSet<TPerfilTela>();
            TUsuario = new HashSet<TUsuario>();
        }

        public int IdPerfil { get; set; }
        public int? IdTela { get; set; }
        public string NmPerfil { get; set; }
        public string DsPerfil { get; set; }
        public DateTime DtCadastro { get; set; }

        public virtual TTela IdTelaNavigation { get; set; }
        public virtual ICollection<TPerfilTela> TPerfilTela { get; set; }
        public virtual ICollection<TUsuario> TUsuario { get; set; }
    }
}
