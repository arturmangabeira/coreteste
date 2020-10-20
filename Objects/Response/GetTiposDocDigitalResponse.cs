using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class GetTiposDocDigitalResponse
    {
        // [XmlElement("documento")]
        public List<TipoDocDigital> documentos { get; set; }

        public GetTiposDocDigitalResponse()
        {
            documentos = new List<TipoDocDigital>();
        }

        public void adicionarDocumento(TipoDocDigital documento)
        {
            documentos.Add(documento);
        }
    }

    [DebuggerDisplay("{Descricao}, {Tipo}")]
    [Serializable]
    [DataContract(Name = "documento")]
    [XmlType(TypeName = "documento")]
    public class TipoDocDigital
    {

        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }


        [DataMember(Name = "Tipo")]
        [XmlAttribute(AttributeName = "Tipo")]
        public ushort Tipo { get; set; }
    }
}
