using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Objects
{
    public class TipoOperacao
    {
        public int IdTipoOperacao { get; set; }        
        public string NmTipoOperacao { get; set; }        
        public DateTime DtCadastro { get; set; }
    }
}
