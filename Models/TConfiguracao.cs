using System;

namespace IntegradorIdea.Models
{
    public partial class TConfiguracao
    {
        public int IdConfiguracao { get; set; }
        public string DsChave { get; set; }
        public string DsValor { get; set; }
        public string TxDescricao { get; set; }
        public DateTime DtCadastro { get; set; }
    }
}
