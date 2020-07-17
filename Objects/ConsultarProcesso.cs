using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Objects
{
    public class ConsultarProcesso
    {
        public string numeroProcesso { get; set; }
        public bool movimentos { get; set; }
        
        public bool incluirCabecalho { get; set; }

        public bool incluirDocumentos { get; set; }        
    }
}
