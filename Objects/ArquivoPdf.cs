using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Objects
{
    public class ArquivoPdf
    {
        public string Nome { get; set; }
        public byte[] Dados { get; set; }
    }
}
