using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class GetAreasCompetenciasEClassesResponse
    {
        public ForoArea Foro { get; set; }

        public GetAreasCompetenciasEClassesResponse()
        {
            Foro = new ForoArea();
        }
    }

    [DebuggerDisplay("{Codigo}, {Nome}")]
    [Serializable]
    [DataContract(Name = "Foro")]
    [XmlType(TypeName = "Foro")]
    public class ForoArea
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public int Codigo { get; set; }

        [DataMember(Name = "Nome")]
        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }

        public List<Area> Area { get; set; }

        public ForoArea()
        {
            Area = new List<Area>();
        }

        public void adicionarArea(Area area)
        {
            Area.Add(area);
        }
    }

    [DebuggerDisplay("{Codigo}")]
    [Serializable]
    [DataContract(Name = "Area")]
    [XmlType(TypeName = "Area")]
    public class Area
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public string Codigo { get; set; }
        public List<CompetenciaArea> Competencia { get; set; }

        public Area()
        {
            Competencia = new List<CompetenciaArea>();
        }

        public void adicionarDocumento(CompetenciaArea competencia)
        {
            Competencia.Add(competencia);
        }
    }

    [DebuggerDisplay("{Codigo}, {Descricao}")]
    [Serializable]
    [DataContract(Name = "Competencia")]
    [XmlType(TypeName = "Competencia")]
    public class CompetenciaArea
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public int Codigo { get; set; }

        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }

        public List<Classe> Classe { get; set; }

        public CompetenciaArea()
        {
            Classe = new List<Classe>();
        }

        public void adicionarDocumento(Classe classe)
        {
            Classe.Add(classe);
        }
    }

    [DebuggerDisplay("{Codigo}, {Descricao}")]
    [Serializable]
    [DataContract(Name = "Competencia")]
    [XmlType(TypeName = "Competencia")]
    public class Classe
    {
        [DataMember(Name = "Codigo")]
        [XmlAttribute(AttributeName = "Codigo")]
        public int Codigo { get; set; }

        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }
    }
}
