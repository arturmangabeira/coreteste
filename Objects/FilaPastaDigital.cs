using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Objects
{
    public class FilaPastaDigital
    {
        public int IdFilaPastaDigital { get; set; }
        public int IdSituacaoPastaDigital { get; set; }
        public string NmSituacaoPastaDigital { get; set; }
        public int IdServidor { get; set; }
        public string NmServidor { get; set; }
        public string DsServidor { get; set; }
        public string NuProcesso { get; set; }
        public int CdIdea { get; set; }
        public DateTime DtInicial { get; set; }
        public DateTime DtFinal { get; set; }
        public DateTime DtInicioProcessamento { get; set; }
        public string DsErro { get; set; }
        public int NuUltimaPaginaBaixada { get; set; }
        public string DsCaminhoPastaDigital { get; set; }
        public DateTime DtCadastro { get; set; }

    }
}
