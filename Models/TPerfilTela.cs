using System;

namespace IntegradorIdea.Models
{
    public partial class TPerfilTela
    {
        public int IdPerfilTela { get; set; }
        public int? IdPerfil { get; set; }
        public int? IdTela { get; set; }
        public DateTime DtCadastro { get; set; }

        public virtual TPerfil IdPerfilNavigation { get; set; }
        public virtual TTela IdTelaNavigation { get; set; }
    }
}
