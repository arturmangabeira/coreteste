using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class getClasseTpParteResponse
    {
        public ClasseTpParte[] Classes { get; set; }

    }

    [DebuggerDisplay("{Descricao}, {Tipo}")]
    [Serializable]
    [DataContract(Name = "Classe")]
    [XmlType(TypeName = "Classe")]
    public class ClasseTpParte
    {
        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Tipo")]
        [XmlAttribute(AttributeName = "Tipo")]
        public uint Tipo { get; set; }

        public ParteClasse[] Partes { get; set; }
    }

    [DebuggerDisplay("{Descricao}")]
    [Serializable]
    [DataContract(Name = "Parte")]
    [XmlType(TypeName = "Parte")]
    public class ParteClasse
    {
        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }
    }
}
