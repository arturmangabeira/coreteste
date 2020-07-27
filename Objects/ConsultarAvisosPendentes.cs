using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Objects
{
    public class ConsultarAvisosPendentes
    {
        public string idRepresentado { get; set; }
        public string idConsultante { get; set; }

        public string senhaConsultante { get; set; }

        public string dataReferencia { get; set; }
    }
}
