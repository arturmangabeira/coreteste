using System;

namespace IntegradorIdea.Models
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
        public int? NuDiasPrazo { get; set; }
        public int? CdForo { get; set; }
        public DateTime? DtIntimacao { get; set; }
        public DateTime? DtMovimentacao { get; set; }
        public int? CdMovimentacao { get; set; }
        public string DsMovimentacao { get; set; }
        public string DsComplemento { get; set; }
        public string DsOutrosNumeros { get; set; }
        public string DsDocumentosAnexos { get; set; }
        public string DsCaminhoDocumentosAnexoAtoDisponibilizado { get; set; }
        public string DsCaminhoDocumentosAnexoAtoEnvio { get; set; }
        public string DsCaminhoDocumentosAnexoAtoRetorno { get; set; }        
        public string DsTeorAto { get; set; }
        public DateTime? DtCiencia { get; set; }
        public DateTime? DtLimiteCiencia { get; set; }
        
    }
}
