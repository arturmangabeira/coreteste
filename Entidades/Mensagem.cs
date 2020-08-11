
namespace IntegradorIdea.Entidades
{
    public class Mensagem: EntidadeBase
    {
        private string servico;

        public string Servico
        {
            get { return servico; }
            set { servico = value; }
        }
        private string versao;

        public string Versao
        {
            get { return versao; }
            set { versao = value; }
        }
        private string msgDesc;

        public string MsgDesc
        {
            get { return msgDesc; }
            set { msgDesc = value; }
        }
        private string codigo;

        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        private string origem;

        public string Origem
        {
            get { return origem; }
            set { origem = value; }
        }
        private string destino;

        public string Destino
        {
            get { return destino; }
            set { destino = value; }
        }
        private string data;

        public string Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}