namespace IntegradorIdea.Objects.ConsultarProcessoESAJ
{
    //[Serializable]
    //[DataContract(Name = "consultarProcessoResponse")]
    //[XmlType(TypeName = "consultarProcessoResponse")]
    public class consultarProcessoResposta
    {
        //[DataMember(Name = "sucesso")]
        //[XmlAttribute( Namespace = "IntegradoIdea")]
        public bool sucesso { get; set; }

        //[DataMember(Name = "sucesso")]
        //[XmlAttribute(Namespace = "IntegradoIdea")]
        public string mensagem { get; set; }

        public tipoProcessoJudicialESAJ processo { get; set; }
    }

    //[Serializable]
    //[DataContract(Name = "tipoProcessoJudicial")]
    //[XmlType(TypeName = "tipoProcessoJudicial")]
    public class tipoProcessoJudicialESAJ
    {      

        private Documento[] documentoField;
        
        /// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute("documento", IsNullable = true, Order = 2)]
        public Documento[] documento
        {
            get
            {
                return this.documentoField;
            }
            set
            {
                this.documentoField = value;
            }
        }
    }
}
