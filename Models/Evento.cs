using System.Runtime.Serialization;

namespace Core.Api.Models
{
    [DataContract]
    public class Events
    {        
        [DataMember]
        public int EventoId { get; set; }
        [DataMember]
        public string Local { get; set; }
        [DataMember]
        public string DataEvento { get; set; }
        [DataMember]
        public string Tema { get; set; }
        [DataMember]
        public int QtdPessoas { get; set; }
        [DataMember]
        public string Lote { get; set; }
    }
}