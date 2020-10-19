using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class GetForosEVarasResponse
    {
        public List<Foro> Foros { get; set; }

        public GetForosEVarasResponse()
        {
            Foros = new List<Foro>();
        }

        public void adicionarForo(Foro foro)
        {
            Foros.Add(foro);
        }
    }

    [DebuggerDisplay("{Codigo}, {Nome}")]
    [Serializable]
    [DataContract(Name = "Foro")]
    [XmlType(TypeName = "Foro")]
    public class Foro
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public int Codigo { get; set; }
        [DataMember(Name = "Nome")]
        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }
        public Comarca Comarca { get; set; }
        public Municipio Municipio { get; set; }
        public List<Vara> Varas {get; set;}

        public Foro()
        {
            Varas = new List<Vara>();
        }

        public void adicionarVara(Vara vara)
        {
            Varas.Add(vara);
        }
    }

    [DebuggerDisplay("{Codigo}, {Nome}")]
    [Serializable]
    [DataContract(Name = "Comarca")]
    [XmlType(TypeName = "Comarca")]
    public class Comarca
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public int Codigo { get; set; }

        [DataMember(Name = "Nome")]
        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }
    }

    [DebuggerDisplay("{Codigo}, {Nome}")]
    [Serializable]
    [DataContract(Name = "Municipio")]
    [XmlType(TypeName = "Municipio")]
    public class Municipio
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public int Codigo { get; set; }

        [DataMember(Name = "Nome")]
        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }
    }

    [DebuggerDisplay("{Codigo}, {Nome}")]
    [Serializable]
    [DataContract(Name = "Vara")]
    [XmlType(TypeName = "Vara")]
    public class Vara
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public int Codigo { get; set; }

        [DataMember(Name = "Nome")]
        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }
        public Competencia Competencia { get; set; }
    }

    public class Competencia
    {
        [DataMember(Name = "Tipo")]
        [XmlAttribute(AttributeName = "Tipo")]
        public int Tipo { get; set; }

        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }
    }
}
