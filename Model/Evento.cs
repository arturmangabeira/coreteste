using System;

namespace Core.Api.Model
{
    public class Evento
    {
        public int EventoId { get; set; }
        public string Local { get; set; }

        public DateTime DataEvento { get; set; }

        public string Tema { get; set; }

        public int QtdPessoas { get; set; }

        public int Lote { get; set; }
    }
}