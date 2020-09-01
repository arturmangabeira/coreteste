using System;

namespace IntegradorIdea.Models
{
    public partial class tTpDocumentoParte
    {
        public tTpDocumentoParte()
        {

        }

        public int IdTipoDocumentoParte { get; set; }
        public string SgTpDocumentoPje { get; set; }
        public string SgTpDocumentoEsaj { get; set; }
        public string DsEmissorDocumento { get; set; }
        public DateTime DtCadastro { get; set; }
        
    }
}
