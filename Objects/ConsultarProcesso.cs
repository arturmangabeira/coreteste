
namespace IntegradorIdea.Objects
{    
    public class ConsultarProcesso
    {
 
        public int idConsultante { get; set; }
 
        public string numeroProcesso { get; set; }
 
        public bool movimentos { get; set; }
 
        public bool incluirCabecalho { get; set; }
 
        public bool incluirDocumentos { get; set; }
 
        public string[] documento { get; set; }
    }
}
