using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response.GetAreasCompetenciasEClassesResponse
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

        public Area[] Area { get; set; }

        public ForoArea()
        {            
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

        public CompetenciaArea[] Competencia { get; set; }

        public Area()
        {
        
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

        public Classe[] Classe { get; set; }

        public CompetenciaArea()
        {            
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
