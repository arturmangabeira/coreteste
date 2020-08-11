using System;
using System.Collections.Generic;

namespace IntegradorIdea.Models
{
    public partial class TTela
    {
        public TTela()
        {
            TLogAtividade = new HashSet<TLogAtividade>();
            TPerfil = new HashSet<TPerfil>();
            TPerfilTela = new HashSet<TPerfilTela>();
        }

        public int IdTela { get; set; }
        public int? IdGrupoMenu { get; set; }
        public string NmTela { get; set; }
        public string DsTela { get; set; }
        public string DsIcone { get; set; }
        public string DsControlador { get; set; }
        public string DsAcao { get; set; }
        public bool FlExcecao { get; set; }
        public bool? FlRegistrarLog { get; set; }
        public bool FlExibirMenu { get; set; }
        public short NuOrdem { get; set; }
        public DateTime DtCadastro { get; set; }

        public virtual TGrupoMenu IdGrupoMenuNavigation { get; set; }
        public virtual ICollection<TLogAtividade> TLogAtividade { get; set; }
        public virtual ICollection<TPerfil> TPerfil { get; set; }
        public virtual ICollection<TPerfilTela> TPerfilTela { get; set; }
    }
}
