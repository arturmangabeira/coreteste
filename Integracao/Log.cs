using Core.Api.Data;
using Core.Api.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;

namespace Core.Api.Integracao
{
    public class Log
    {
        public DataContext DataContext { get; }

        public Log(DataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public void RegistrarLogOperacao(TLogOperacao logOperacao)
        {
            try
            {                
                logOperacao.DsCaminhoDocumentosChamada = this.GravarArquivoXML(logOperacao.DsCaminhoDocumentosChamada,
                    logOperacao.CdIdea,
                    logOperacao.IdTipoOperacao
                    , "chamada");
                logOperacao.DsCaminhoDocumentosRetorno = this.GravarArquivoXML(logOperacao.DsCaminhoDocumentosRetorno,
                    logOperacao.CdIdea,
                    logOperacao.IdTipoOperacao
                    , "retorno");                
                this.DataContext.TLogOperacao.Add(logOperacao);
                this.DataContext.SaveChanges();
            }
            catch(Exception ex)
            {
                
            }
        }

        private string GravarArquivoXML(string XML, string cdIdeia, int IdTipoOperacao, string tipoCaminho)
        {
            var config = ConfigurationManager.ConfigurationManager.AppSettings;
            var caminho = config["Diretorios:DsCaminhoDocumentos"];
            var pathDirectorySeparator = Path.DirectorySeparatorChar;
            //CRIA AS PASTAS NECESSÁRIAS PARA ARMAZENAR NO SERVIDOR
            this.CriarPastaLogs();

            string nomeFile = cdIdeia;

            if(IdTipoOperacao == config.GetValue<int>("Constantes:IdTipoOperacaoConsultaProcesso"))
            {
                nomeFile += "_" + tipoCaminho + "_consultarprocesso_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
            }
            var caminhoTotal = caminho + pathDirectorySeparator + DateTime.Now.ToString("yyyy") + pathDirectorySeparator + DateTime.Now.ToString("MM") + pathDirectorySeparator + DateTime.Now.ToString("dd") + pathDirectorySeparator + nomeFile;
            //ESCREVE NO CAMINHO ESPECIFICADO 
            File.WriteAllBytes(caminhoTotal, Convert.FromBase64String(Util.Base64Encode(XML)));

            return caminhoTotal;
        }

        private void CriarPastaLogs()
        {
            var config = ConfigurationManager.ConfigurationManager.AppSettings;
            var caminho = config["Diretorios:DsCaminhoDocumentos"];
            var pathDirectorySeparator = Path.DirectorySeparatorChar;
            //VERIFICA O CAMINHO DA PASTA PARA GERAR E ESCREVER NO LOCAL
            //VERIFICACAO POR ANO
            if (!Directory.Exists(caminho + pathDirectorySeparator + DateTime.Now.ToString("yyyy")))
            {
                Directory.CreateDirectory(caminho + pathDirectorySeparator + DateTime.Now.ToString("yyyy"));
            }

            //VERIFICACAO POR MÊS
            if (!Directory.Exists(caminho + pathDirectorySeparator + DateTime.Now.ToString("yyyy") + pathDirectorySeparator + DateTime.Now.ToString("MM")))
            {
                Directory.CreateDirectory(caminho + pathDirectorySeparator + DateTime.Now.ToString("yyyy") + pathDirectorySeparator + DateTime.Now.ToString("MM"));
            }
            //VERIFICACAO POR DIA
            if (!Directory.Exists(caminho + pathDirectorySeparator + DateTime.Now.ToString("yyyy") + pathDirectorySeparator + DateTime.Now.ToString("MM") + pathDirectorySeparator + DateTime.Now.ToString("dd")))
            {
                Directory.CreateDirectory(caminho + pathDirectorySeparator + DateTime.Now.ToString("yyyy") + pathDirectorySeparator + DateTime.Now.ToString("MM") + pathDirectorySeparator + DateTime.Now.ToString("dd"));
            }
        }
    }
}
