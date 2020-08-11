using System;

namespace IntegradorIdea.Models
{
    public partial class TLogAtividade
    {
        public int IdLogAtividade { get; set; }
        public int IdTela { get; set; }
        public int IdUsuario { get; set; }
        public string DsDados { get; set; }
        public string DsIp { get; set; }
        public string DsDispositivo { get; set; }
        public DateTime DtLog { get; set; }

        public virtual TTela IdTelaNavigation { get; set; }
        public virtual TUsuario IdUsuarioNavigation { get; set; }
    }
}
