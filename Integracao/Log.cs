using Core.Api.Data;
using Core.Api.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Core.Api.Integracao
{
    public class Log
    {
        public DataContext _dataContext { get; }

        public string _ipDestino { get; }

        public Log(DataContext dataContext, string ipDestino)
        {
            _dataContext = dataContext;
            _ipDestino = ipDestino;
        }

        public void RegistrarLogOperacao(TLogOperacao logOperacao)
        {
            try
            {
                var config = ConfigurationManager.ConfigurationManager.AppSettings;

                var ipOrigem = Util.GetIpOrigem();

                if (config.GetValue<bool>("RegistraLog:Registrar")){

                    logOperacao.DsCaminhoDocumentosChamada = this.GravarArquivoXML(logOperacao.DsCaminhoDocumentosChamada,
                        logOperacao.CdIdea,
                        logOperacao.IdTipoOperacao
                        , "chamada");

                    logOperacao.DsCaminhoDocumentosRetorno = this.GravarArquivoXML(logOperacao.DsCaminhoDocumentosRetorno,
                        logOperacao.CdIdea,
                        logOperacao.IdTipoOperacao
                        , "retorno");

                    logOperacao.DsIporigem = ipOrigem;
                    logOperacao.DsIpdestino = _ipDestino;

                    _dataContext.TLogOperacao.Add(logOperacao);
                    _dataContext.SaveChanges();
                }
            }
            catch
            {
                
            }
        }

        private string GravarArquivoXML(string XML, string cdIdeia, int IdTipoOperacao, string tipoCaminho)
        {
            var config = ConfigurationManager.ConfigurationManager.AppSettings;
            var caminhoRetorno = "";
            if (config.GetValue<bool>("RegistraLog:GravarArquivosXMLs"))
            {                
                var caminho = config["Diretorios:DsCaminhoDocumentos"];
                var caminhoPastaXmls = config["Diretorios:DsPastaXML"];
                var pathDirectorySeparator = Path.DirectorySeparatorChar;

                //ATUALIZA A INFORMAÇÃO DO CAMINHO PARA GERAÇÃO DOS ARQUIVOS.
                //caminho = caminho + pathDirectorySeparator + caminhoPastaXmls;
                //CRIA AS PASTAS NECESSÁRIAS PARA ARMAZENAR NO SERVIDOR
                this.CriarPastaLogs();

                string nomeFile = cdIdeia;

                /*if (IdTipoOperacao == config.GetValue<int>("Constantes:IdTipoOperacaoConsultaProcesso"))
                {
                    nomeFile += "_" + tipoCaminho + "_consultarprocesso_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                }*/

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoConsultaProcesso:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + "_" + dsOperacao + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoCiencia:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoCiencia:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoPeticionamentoInicial:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoPeticionamentoInicial:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoPeticionamentoIntermediario:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoPeticionamentoIntermediario:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoSolicitaLogon:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoSolicitaLogon:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoConfirmaLogon:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoConfirmaLogon:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";
                }

                caminhoRetorno = caminhoPastaXmls + pathDirectorySeparator + DateTime.Now.ToString("yyyy") + pathDirectorySeparator + DateTime.Now.ToString("MM") + pathDirectorySeparator + DateTime.Now.ToString("dd") + pathDirectorySeparator + nomeFile;
                string caminhoTotal = caminho + pathDirectorySeparator + caminhoRetorno;
                //ESCREVE NO CAMINHO ESPECIFICADO 
                File.WriteAllBytes(caminhoTotal, Convert.FromBase64String(Util.Base64Encode(XML)));
            }
            //RETORNA O CAMINHO RELATIVO 'A PARTIR DO CAMPO Diretorios:DsPastaXML DEFINIDO NO appsettings'
            return caminhoRetorno;
        }

        private void CriarPastaLogs()
        {
            var config = ConfigurationManager.ConfigurationManager.AppSettings;
            var caminho = config["Diretorios:DsCaminhoDocumentos"];
            var caminhoPastaXmls = config["Diretorios:DsPastaXML"];
            var pathDirectorySeparator = Path.DirectorySeparatorChar;
            //VERIFICA O CAMINHO DA PASTA PARA GERAR E ESCREVER NO LOCAL
            //VERIFICA SE EXISTE A PASTA INICIAL PARA SALVAR OS ARQUIVOS EM XML
            if(!Directory.Exists(caminho + pathDirectorySeparator + caminhoPastaXmls))
            {
                Directory.CreateDirectory(caminho + pathDirectorySeparator + caminhoPastaXmls);
            }
            //ATUALIZA A INFORMAÇÃO DO CAMINHO PARA GERAÇÃO DOS ARQUIVOS.
            caminho = caminho + pathDirectorySeparator + caminhoPastaXmls;
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
