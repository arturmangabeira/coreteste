using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegradorIdea.Objects
{
    public class ArquivoPdf
    {
        public string Nome { get; set; }
		private byte[] dados;
		public byte[] Dados
		{
			get
			{
				return this.dados;
			}
			set
			{
				this.dados = value;
			}
		}

		public ArquivoPdf AdicionarDados(ref byte[] dados, string nome)
		{
			return new ArquivoPdf
			{
				Nome = nome,
				Dados = dados
			};
		}
	}

	
}
