using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class GetAssuntosResponse
    {
        public List<AssuntoTmp> Assuntos { get; set; }

        public GetAssuntosResponse()
        {
            Assuntos = new List<AssuntoTmp>();
        }

        public void adicionarAssunto(AssuntoTmp assunto)
        {
            Assuntos.Add(assunto);
        }
    }

    [DebuggerDisplay("{codigo}, {codigoPai}, {descricao}")]
    [Serializable]
    [DataContract(Name = "Assunto")]
    [XmlType(TypeName = "Assunto")]
    public class AssuntoTmp
    {
        [DataMember(Name = "codigo")]
        [XmlAttribute(AttributeName = "codigo")]
        public uint codigo { get; set; }

        [DataMember(Name = "codigoPai")]
        [XmlAttribute(AttributeName = "codigoPai")]
        public ushort codigoPai { get; set; }

        [DataMember(Name = "descricao")]
        [XmlAttribute(AttributeName = "descricao")]
        public string descricao { get; set; }


    }
}
