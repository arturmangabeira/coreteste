using System;
using System.IO;
using System.Net;
using System.ServiceProcess;

namespace ServicoComunicacoesEletronicas
{
    public partial class ServicoComunicacaoEletronica : ServiceBase
    {
        System.Timers.Timer _timer;
        int _scheduleTime;

        public ServicoComunicacaoEletronica()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer();
            _scheduleTime = int.Parse(Settings.Default.FREQUENCIA_EXECUCAO_ROTINA) * 60000;
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Serviço Iniciado em: " + DateTime.Now);
            // For first time, set amount of seconds between current time and schedule time
            _timer.Enabled = true;
            _timer.Interval = double.Parse(_scheduleTime.ToString());
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);

            ObterComunicacoesEletronicas();
        }

        protected override void OnStop()
        {
            WriteToFile("O Serviço parou em: " + DateTime.Now);
        }

        protected void OnElapsedTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            WriteToFile("Serviço de invocado novamente em: " + DateTime.Now);
            ObterComunicacoesEletronicas();
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        public void ObterComunicacoesEletronicas()
        {
            string urlObterListaIntimacaoCitacao = Settings.Default.URL_OBTER_LISTA_INTIMACOES;
            var reqObterListaIntimacoes = (HttpWebRequest)WebRequest.Create(urlObterListaIntimacaoCitacao);
            reqObterListaIntimacoes.Timeout = _scheduleTime;
            using (var streamObterListaIntimacaoCitacao = reqObterListaIntimacoes.GetResponse().GetResponseStream())
            using (var readerObterListaIntimacaoCitacao = new StreamReader(streamObterListaIntimacaoCitacao))
            {
                WriteToFile("Obtenção da Lista de Intimação/Citação realizada com sucesso! Retorno: " + readerObterListaIntimacaoCitacao.ReadToEnd());

                string urlObterDocCiencia = Settings.Default.URL_OBTER_DOC_CIENCIA;
                var reqObterDocCiencia = (HttpWebRequest)WebRequest.Create(urlObterDocCiencia);
                reqObterDocCiencia.Timeout = _scheduleTime;
                using (var streamObterDocCiencia = reqObterDocCiencia.GetResponse().GetResponseStream())
                using (var readerObterDocCiencia = new StreamReader(streamObterDocCiencia))
                {
                    WriteToFile("Obtenção dos documentos das Intimação/Citação realizada sucesso! Retorno: " + readerObterDocCiencia.ReadToEnd());
                }
            }
        }
    }
}
