using System;

namespace IntegradorIdea.Models
{
    public partial class TFilaPastaDigital
    {
        public int IdFilaPastaDigital { get; set; }
        public int IdSituacaoPastaDigital { get; set; }
        public int? IdServidor { get; set; }
        public string NuProcesso { get; set; }
        public int CdIdea { get; set; }
        public DateTime DtInicial { get; set; }
        public DateTime? DtFinal { get; set; }
        public DateTime DtInicioProcessamento { get; set; }
        public string DsErro { get; set; }
        public int? NuUltimaPaginaBaixada { get; set; }
        public string DsCaminhoPastaDigital { get; set; }
        public DateTime DtCadastro { get; set; }

        public virtual TServidor IdServidorNavigation { get; set; }
        public virtual TSituacaoPastaDigital IdSituacaoPastaDigitalNavigation { get; set; }
    }
}
