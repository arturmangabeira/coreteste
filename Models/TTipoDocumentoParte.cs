using System;

namespace IntegradorIdea.Models
{
    public partial class TTipoDocumentoParte
    {
        public TTipoDocumentoParte()
        {

        }

        public int IdTipoDocumentoParte { get; set; }
        public string SgTipoDocumentoPje { get; set; }
        public string SgTipoDocumentoEsaj { get; set; }
        public string DsDescricaoEmissorDocumento { get; set; }
        public DateTime DtCadastro { get; set; }
        
    }
}
