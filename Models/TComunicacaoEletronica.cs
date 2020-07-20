using System;
using System.Collections.Generic;

namespace Core.Api.Models
{
    public partial class TComunicacaoEletronica
    {
        public int IdComunicacaoEletronica { get; set; }
        public DateTime DtDisponibilizacao { get; set; }
        public int CdClasse { get; set; }
        public int CdVara { get; set; }
        public int CdAssunto { get; set; }
        public int CdAto { get; set; }
        public string TpIntimacaoCitacao { get; set; }
        public DateTime? DtRecebimento { get; set; }
        public string NuProcesso { get; set; }
        public DateTime? DtCiencia { get; set; }
        public int? NuDiasPrazo { get; set; }
        public int? CdForo { get; set; }
        public DateTime? DtIntimacao { get; set; }
        public DateTime? DtMovimentacao { get; set; }
        public int? CdMovimentacao { get; set; }
        public string DsMovimentacao { get; set; }
        public string DsComplemento { get; set; }
        public string NuOutrosNumeros { get; set; }
        public string NuDocumentosAnexos { get; set; }
        public string DsCaminhoDocumentosAnexoAtoRecebidos { get; set; }
        public string DsCaminhoDocumentosAnexoAtoEnviados { get; set; }
        public string DsCaminhoDocumentosAnexoAtoAssinado { get; set; }
        public int? NuPrazo { get; set; }
        public string DsTeorAto { get; set; }
        public string FlRecebimentoAutomatico { get; set; }
        public DateTime? DtLimiteCiencia { get; set; }
        public DateTime DtCadastro { get; set; }
    }
}
