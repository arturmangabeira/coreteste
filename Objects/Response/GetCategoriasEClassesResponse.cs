using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IntegradorIdea.Objects.Response
{
    public class GetCategoriasEClassesResponse
    {

        public List<Categoria> Categorias { get; set; }

        public GetCategoriasEClassesResponse()
        {
            Categorias = new List<Categoria>();
        }

        public void adicionarCategoria(Categoria categoria)
        {
            Categorias.Add(categoria);
        }
    }

    [DebuggerDisplay("{Descricao}, {Tipo}")]
    [Serializable]
    [DataContract(Name = "Categoria")]
    [XmlType(TypeName = "Categoria")]
    public class Categoria
    {
        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Tipo")]
        [XmlAttribute(AttributeName = "Tipo")]
        public int Tipo { get; set; }

        public List<ClasseCategoria> Classes { get; set; }

        public Categoria ()
        {
            Classes = new List<ClasseCategoria>();
        }

        public void adicionarClasseCategoria(ClasseCategoria classe)
        {
            Classes.Add(classe);
        }
    }

    [DebuggerDisplay("{Descricao}, {Tipo}")]
    [Serializable]
    [DataContract(Name = "Classe")]
    [XmlType(TypeName = "Classe")]
    public class ClasseCategoria
    {
        [DataMember(Name = "Descricao")]
        [XmlAttribute(AttributeName = "Descricao")]
        public string Descricao { get; set; }

        [DataMember(Name = "Tipo")]
        [XmlAttribute(AttributeName = "Tipo")]
        public uint Tipo { get; set; }
    }
}
