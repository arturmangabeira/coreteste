using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class GetTiposDiversasResponse
    {
        public List<AssuntoGetTiposDiversas> Tipos { get; set; }

        public GetTiposDiversasResponse()
        {
            Tipos = new List<AssuntoGetTiposDiversas>();
        }

        public void adicionarTiposDiversas(AssuntoGetTiposDiversas tiposDiversas)
        {
            Tipos.Add(tiposDiversas);
        }
    }

    [DebuggerDisplay("{Descricao}, {Tipo}")]
    [Serializable]
    [DataContract(Name = "Tipo")]
    [XmlType(TypeName = "Tipo")]
    public class AssuntoGetTiposDiversas
    {
        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Tipo")]
        [XmlAttribute(AttributeName = "Tipo")]
        public uint Tipo { get; set; }
    }
}
