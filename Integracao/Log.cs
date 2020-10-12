using IntegradorIdea.Data;
using IntegradorIdea.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace IntegradorIdea.Integracao
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

        public TLogOperacao RegistrarLogOperacao(TLogOperacao logOperacao)
        {
            try
            {
                var config = ConfigurationManager.ConfigurationManager.AppSettings;

                var ipOrigem = Util.GetIpOrigem();

                if (config.GetValue<bool>("RegistraLog:Registrar"))
                {
                    if (logOperacao.DsCaminhoDocumentosChamada != String.Empty && logOperacao.DsCaminhoDocumentosChamada != null)
                    {
                        logOperacao.DsCaminhoDocumentosChamada = this.GravarArquivoXML(logOperacao.DsCaminhoDocumentosChamada,
                            logOperacao.CdIdea,
                            logOperacao.IdTpOperacao
                            , "chamada");
                    }

                    if (logOperacao.DsCaminhoDocumentosRetorno != String.Empty && logOperacao.DsCaminhoDocumentosRetorno != null)
                    {
                        logOperacao.DsCaminhoDocumentosRetorno = this.GravarArquivoXML(logOperacao.DsCaminhoDocumentosRetorno,
                            logOperacao.CdIdea,
                            logOperacao.IdTpOperacao
                            , "retorno");
                    }

                    logOperacao.DsIporigem = ipOrigem;
                    logOperacao.DsIpdestino = _ipDestino;

                    if(logOperacao.IdLogOperacao == 0)
                    {
                        _dataContext.TLogOperacao.Add(logOperacao);
                        _dataContext.SaveChanges();
                    }
                    else
                    {
                        var logRetorno = _dataContext.TLogOperacao.Where(l => l.IdLogOperacao == logOperacao.IdLogOperacao).FirstOrDefault();
                        logRetorno.DtFinalOperacao = DateTime.Now;
                        logRetorno.DsCaminhoDocumentosRetorno = logOperacao.DsCaminhoDocumentosRetorno;
                        logRetorno.FlOperacao = logOperacao.FlOperacao;
                        _dataContext.TLogOperacao.Add(logRetorno);
                        _dataContext.Update(logRetorno);                        
                        _dataContext.SaveChanges();
                    }
                    
                    _dataContext.Entry(logOperacao).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return logOperacao;
        }

        private string GravarArquivoXML(string XML, int? cdIdeia, int IdTipoOperacao, string tipoCaminho)
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

                string nomeFile = cdIdeia.ToString();
                
                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoConsultaProcesso:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + "_" + dsOperacao + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoConsultaProcessoESAJ:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoConsultaProcessoESAJ:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + "_" + dsOperacao + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoCiencia:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoCiencia:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoPeticionamentoInicial:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoPeticionamentoInicial:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoPeticionamentoIntermediario:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoPeticionamentoIntermediario:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoSolicitaLogon:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoSolicitaLogon:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoConfirmaLogon:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoConfirmaLogon:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoSolicitaListaIntimacoesAguardandoCiencia:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoSolicitaListaIntimacoesAguardandoCiencia:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoSolicitaListaCitacoesAguardandoCiencia:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoSolicitaListaCitacoesAguardandoCiencia:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoConsultarAvisoPendentes:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoConsultarAvisoPendentes:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoConsultarTeorComunicacao:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoConsultarTeorComunicacao:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoSolicitacaoIntimacaoAto:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoSolicitacaoIntimacaoAto:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoSolicitacaoCitacaoAto:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoSolicitacaoCitacaoAto:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoEntregarManifestacaoProcessual:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoEntregarManifestacaoProcessual:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
                }

                if (IdTipoOperacao == config.GetValue<int>("Operacoes:TipoOperacaoEntregarManifestacaoProcessualESAJ:id"))
                {
                    var dsOperacao = config.GetValue<string>("Operacoes:TipoOperacaoEntregarManifestacaoProcessualESAJ:nomeOperacaoLog");
                    nomeFile += "_" + tipoCaminho + dsOperacao + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Util.GetUUIID() + ".xml";
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
