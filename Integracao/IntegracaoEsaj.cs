﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Api.ConfigurationManager;
using Core.Api.Data;
using Core.Api.Entidades;
using Core.Api.Entidades.AssuntoClasse;
using Core.Api.Entidades.CategoriaClasse;
using Core.Api.Entidades.DocDigitalClasse;
using Core.Api.Entidades.ForoClasse;
using Core.Api.Entidades.TipoDiversasClasse;
using Core.Api.Entidades.TpParteClasse;
using Core.Api.Models;
using Core.Api.Objects;
using CsQuery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Core.Api.Integracao
{
    public class IntegracaoEsaj
    {
        public Proxy _Proxy { get; }
        public IConfiguration Configuration { get; }

        public ILogger<IntegracaoService> _logger;

        private DataContext _dataContext { get; }
        public Log logOperacao { get; }
        public IntegracaoEsaj(Proxy proxy, DataContext dataContext, ILogger<IntegracaoService> logger) 
        {
            this.Configuration = ConfigurationManager.ConfigurationManager.AppSettings;
            _Proxy = proxy;
            _logger = logger;
            _dataContext = dataContext;
            this.logOperacao = new Log(dataContext);
        }

        #region ObterDadosProcesso
        public consultarProcessoResponse ObterDadosProcesso(ConsultarProcesso consultarProcesso)
        {
            Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno = null;
            consultarProcessoResponse consultar = new consultarProcessoResponse();
            string xmlDadosProcessoRetorno = String.Empty;
            var dtInicial = DateTime.Now;
            try
            {
                tipoProcessoJudicial tipoProcessoJudicial = new tipoProcessoJudicial();
                this._Proxy.CdIdeia = consultarProcesso.idConsultante;
                _logger.LogInformation("ObterDadosProcesso ", consultarProcesso);
                if (consultarProcesso.incluirCabecalho)
                {
                    Entidades.ConsultaProcesso.Message Message = new Entidades.ConsultaProcesso.Message();
                    Entidades.ConsultaProcesso.MessageMessageId MessageMessageId = new Entidades.ConsultaProcesso.MessageMessageId();
                    Entidades.ConsultaProcesso.MessageMessageBody MessageMessageBody = new Entidades.ConsultaProcesso.MessageMessageBody();

                    MessageMessageId.Code = "202020007662";
                    MessageMessageId.Date = DateTime.Now.ToString("yyyy-MM-dd"); ;
                    MessageMessageId.FromAddress = "MP-BA";
                    MessageMessageId.ToAddress = "TJ";
                    MessageMessageId.Version = Entidades.ConsultaProcesso.VersionType.Item10;
                    MessageMessageId.MsgDesc = "Consulta Processo";
                    MessageMessageId.ServiceId = Entidades.ConsultaProcesso.ServiceIdType.ConsultaProcesso;
                    Message.MessageId = MessageMessageId;

                    Entidades.ConsultaProcesso.MessageMessageBodyProcesso MessageBodyProcesso = new Entidades.ConsultaProcesso.MessageMessageBodyProcesso();
                    MessageBodyProcesso.Numero = consultarProcesso.numeroProcesso;
                    MessageMessageBody.Processo = MessageBodyProcesso;
                    Message.MessageBody = MessageMessageBody;

                    string xml = Message.Serialize();

                    _logger.LogInformation("ObterDadosProcesso PROXY");
                    xmlDadosProcessoRetorno = this._Proxy.ObterDadosProcesso(xml, "202020007662");
                    _logger.LogInformation("RETORNO ObterDadosProcesso PROXY " + xmlDadosProcessoRetorno);

                    objDadosProcessoRetorno = new Entidades.ConsultaProcessoResposta.Message();
                    objDadosProcessoRetorno = objDadosProcessoRetorno.ExtrairObjeto<Entidades.ConsultaProcessoResposta.Message>(xmlDadosProcessoRetorno);

                    if (objDadosProcessoRetorno.MessageBody.Resposta.Mensagem.Codigo == "0")
                    {
                        var processo = objDadosProcessoRetorno.MessageBody.Resposta.Processo;
                        //OBTÉM OS DADOS BÁSICOS
                        tipoProcessoJudicial.dadosBasicos = this.ObterDadosBasicos(objDadosProcessoRetorno);
                        //OBTÉM OS DADOS DA PARTE
                        tipoProcessoJudicial.dadosBasicos.polo = this.ObterPartes(objDadosProcessoRetorno).ToArray();
                        //OBTÉM OS DADOS DO ASSUNTO
                        tipoProcessoJudicial.dadosBasicos.assunto = new tipoAssuntoProcessual[]{
                            new tipoAssuntoProcessual()
                            {
                                codigoNacional = Int32.Parse(processo.AssuntoPrincipal.Codigo),
                                assuntoLocal = new tipoAssuntoLocal(){
                                    codigoAssunto = Int32.Parse(processo.AssuntoPrincipal.Codigo),
                                    descricao = processo.AssuntoPrincipal.Descricao
                                },
                                principal = true,
                                principalSpecified = true
                            }
                        };
                        //OBTÉM OS DADOS OUTROS PARAMETROS
                        tipoProcessoJudicial.dadosBasicos.outroParametro = new tipoParametro[]
                        {
                            new tipoParametro()
                            {
                                nome = "mni:esaj:dataDistribuicao",
                                valor = processo.DataDistribuicao.Replace("-","")+"000000"
                            }
                        };
                        //OBTÉM OS DADOS DO VALOR CAUSA
                        tipoProcessoJudicial.dadosBasicos.valorCausa = Double.Parse(processo.ValorCausa);
                        //OBTÉM OS DADOS DO ORGAO JULGADOR
                        tipoProcessoJudicial.dadosBasicos.orgaoJulgador = new tipoOrgaoJulgador()
                        {
                            codigoOrgao = processo.Vara.Codigo,
                            instancia = processo.Vara.Competencia.Descricao,
                            nomeOrgao = processo.Vara.Nome
                        };
                        //ACRESCENTA A MOVIMENTAÇÃO CASO SEJA INFORMADO.
                        if (consultarProcesso.movimentos)
                        {
                            tipoProcessoJudicial.movimento = this.ObterMovimentacoes(consultarProcesso.numeroProcesso).ToArray();
                        }
                        //ACRESCENTA O DOCUMENSO CASO SEJA INFORMADO.INCLUE NA FILA DA PASTA DIGITAL.
                        if (consultarProcesso.incluirDocumentos)
                        {
                            tipoProcessoJudicial.documento = this.ObterDocumentos(consultarProcesso.numeroProcesso).ToArray();
                        }

                        //RETORNA O ERRO ENCONTRADO NO E-SAJ PARA REFLETIR NO OBJETO IGUAL A DESCRIÇÃO NO E-SAJ
                        consultar = new consultarProcessoResponse()
                        {
                            mensagem = objDadosProcessoRetorno.MessageBody.Resposta.Mensagem.Descricao,
                            sucesso = true,
                            processo = tipoProcessoJudicial
                        };
                    }
                    else
                    {
                        //RETORNA O ERRO ENCONTRADO NO E-SAJ PARA REFLETIR NO OBJETO IGUAL A DESCRIÇÃO NO E-SAJ
                        consultar = new consultarProcessoResponse()
                        {
                            mensagem = objDadosProcessoRetorno.MessageBody.Resposta.Mensagem.Descricao,
                            sucesso = false,
                            processo = null
                        };
                    }

                    var dtFinal = DateTime.Now;
                    //REGISTAR LOGON
                    TLogOperacao operacao = new TLogOperacao()
                    {
                        CdIdea = consultarProcesso.idConsultante,
                        DsCaminhoDocumentosChamada = xml,
                        DsCaminhoDocumentosRetorno = Util.Serializar(consultar),
                        DsIpdestino = "192.168.0.1",
                        DsIporigem = "192.168.0.1",
                        DsLogOperacao = "Consulta de Processo " + consultarProcesso.numeroProcesso,
                        DtInicioOperacao = dtInicial,
                        DtFinalOperacao = dtFinal,
                        DtLogOperacao = DateTime.Now,
                        FlOperacao = true,
                        IdTipoOperacao = this.Configuration.GetValue<int>("Constantes:IdTipoOperacaoConsultaProcesso"),
                        IdTipoRetorno = 1
                    };
                    //REGISTRA O LOG
                    this.logOperacao.RegistrarLogOperacao(operacao);
                }
                else
                {
                    //ACRESCENTA A MOVIMENTAÇÃO CASO SEJA INFORMADO.
                    if (consultarProcesso.movimentos)
                    {
                        tipoProcessoJudicial.movimento = this.ObterMovimentacoes(consultarProcesso.numeroProcesso).ToArray();
                    }
                    //DEVOLVE O OBJETO DE ACORDO COM O CABEÇALHO SOLICITADO.
                    consultar = new consultarProcessoResponse()
                    {
                        mensagem = "Processo consultado com sucesso",
                        sucesso = true,
                        processo = tipoProcessoJudicial
                    };

                    var dtFinal = DateTime.Now;
                    //REGISTAR LOGON
                    TLogOperacao operacao = new TLogOperacao()
                    {
                        CdIdea = consultarProcesso.idConsultante,
                        DsCaminhoDocumentosChamada = Util.Serializar(consultarProcesso),
                        DsCaminhoDocumentosRetorno = Util.Serializar(consultar),
                        DsIpdestino = "192.168.0.1",
                        DsIporigem = "192.168.0.1",
                        DsLogOperacao = "Consulta de Processo " + consultarProcesso.numeroProcesso,
                        DtInicioOperacao = dtInicial,
                        DtFinalOperacao = dtFinal,
                        DtLogOperacao = DateTime.Now,
                        FlOperacao = true,
                        IdTipoOperacao = this.Configuration.GetValue<int>("Constantes:IdTipoOperacaoConsultaProcesso"),
                        IdTipoRetorno = 1
                    };
                    //REGISTRA O LOG
                    this.logOperacao.RegistrarLogOperacao(operacao);
                }
            }
            catch (Exception ex)
            {
                consultar = new consultarProcessoResponse()
                {
                    mensagem = $"Erro ao tentar consultar os dados do Processo. Ex:{ex.Message}",
                    sucesso = false,
                    processo = null
                };
                var dtFinal = DateTime.Now;
                //REGISTAR LOGON
                TLogOperacao operacao = new TLogOperacao()
                {
                    CdIdea = consultarProcesso.idConsultante,
                    DsCaminhoDocumentosChamada = Util.Serializar(consultarProcesso),
                    DsCaminhoDocumentosRetorno = Util.Serializar(consultar),
                    DsIpdestino = "192.168.0.1",
                    DsIporigem = "192.168.0.1",
                    DsLogOperacao = "Consulta de Processo " + consultarProcesso.numeroProcesso,
                    DtInicioOperacao = dtInicial,
                    DtFinalOperacao = dtFinal,
                    DtLogOperacao = DateTime.Now,
                    FlOperacao = false,
                    IdTipoOperacao = this.Configuration.GetValue<int>("Constantes:IdTipoOperacaoConsultaProcesso"),
                    IdTipoRetorno = 2
                };
                //REGISTRA O LOG
                this.logOperacao.RegistrarLogOperacao(operacao);
            }

            return consultar;
        }
        #endregion

        private List<tipoDocumento> ObterDocumentos(string numeroProcesso)
        {
            var pathDirectorySeparator = Path.DirectorySeparatorChar;

             var caminhoArquivos = ConfigurationManager.ConfigurationManager.AppSettings["Diretorios:DsCaminhoProcessos"] + pathDirectorySeparator + numeroProcesso;

            var documentos = new List<tipoDocumento>();

            var diretorios = Directory.GetDirectories(caminhoArquivos);

            foreach (var diretorio in diretorios)
            {
                var arquivos = Directory.GetFiles(diretorio);

                var docVinculado = new List<tipoDocumento>();
                foreach (var arquivo in arquivos)
                {
                    docVinculado.Add(new tipoDocumento
                    {
                        conteudo = File.ReadAllBytes(arquivo),
                        descricao = arquivo,
                    });
                }
                documentos.Add(new tipoDocumento
                {                    
                    descricao = diretorio,
                    documentoVinculado = docVinculado.ToArray()
                });
            }

            return documentos;
        }

        #region ObterDadosBasicos
        /// <summary>
        /// Método para obter os dados básicos informadas no XML do ESAJ e devolver no padrão do MNI PJE
        /// </summary>
        /// <param name="objDadosProcessoRetorno"></param>
        /// <returns></returns>
        private tipoCabecalhoProcesso ObterDadosBasicos(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)
        {
            var processo = objDadosProcessoRetorno.MessageBody.Resposta.Processo;

            var dtAjuizamento = processo.DataAjuizamento.Replace("-", "") + "000000";

            return new tipoCabecalhoProcesso()
            {
                codigoLocalidade = processo.Foro.Codigo,
                dataAjuizamento = dtAjuizamento,
                classeProcessual = Int32.Parse(processo.Classe.Codigo),
                competencia = Int32.Parse(processo.Foro.Codigo),
                numero = processo.Numero,
                nivelSigilo = processo.SegredoJustica == "S" ? 1 : 0
            };
        }
        #endregion

        #region ObterPartes
        /// <summary>
        /// Método para obter as partes informadas no XML do ESAJ e devolver no padrão do MNI PJE
        /// </summary>
        /// <param name="objDadosProcessoRetorno"></param>
        /// <returns></returns>
        private List<tipoPoloProcessual> ObterPartes(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)
        {
            List<tipoPoloProcessual> tipoPoloProcessuais = new List<tipoPoloProcessual>();

            tipoPoloProcessuais.Add(new tipoPoloProcessual()
            {
                polo = modalidadePoloProcessual.AT,
                parte = this.ObterParteAtiva(objDadosProcessoRetorno).ToArray(),
                poloSpecified = true
            });

            tipoPoloProcessuais.Add(new tipoPoloProcessual()
            {
                polo = modalidadePoloProcessual.PA,
                parte = this.ObterPartePassiva(objDadosProcessoRetorno).ToArray(),
                poloSpecified = true
            });

            return tipoPoloProcessuais;
        }
        #endregion

        #region ObterParteAtiva
        private List<tipoParte> ObterParteAtiva(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)
        {
            var parteAtivas = new List<tipoParte>();
            foreach (var pAtiva in objDadosProcessoRetorno.MessageBody.Resposta.Processo.Partes.PartesAtivas)
            {
                var documentos = new List<tipoDocumentoIdentificacao>();

                var pessoaRelacionadas = new List<tipoRelacionamentoPessoal>();

                //CASO EXISTA OS ADVS RELACIONA A PARTE.
                if (pAtiva.Advogados != null && pAtiva.Advogados.Length > 0)
                {
                    foreach (var adv in pAtiva.Advogados)
                    {
                        pessoaRelacionadas.Add(
                            new tipoRelacionamentoPessoal()
                            {
                                pessoa = new tipoPessoa()
                                {
                                    nome = adv.Nome,
                                    documento = new tipoDocumentoIdentificacao[]
                                    {
                                        new tipoDocumentoIdentificacao()
                                        {
                                            nome = adv.Nome,
                                            tipoDocumento = modalidadeDocumentoIdentificador.OAB,
                                            codigoDocumento = Util.OnlyNumbers(adv.OAB),
                                            emissorDocumento = ""
                                        }
                                    }
                                },
                                modalidadeRelacionamento = modalidadesRelacionamentoPessoal.AP
                            });
                    }
                }



                if (pAtiva.Documentos != null && pAtiva.Documentos.Length > 0)
                {
                    foreach (var doc in pAtiva.Documentos)
                    {
                        //IDENTIFICACAO DE NOME DO DOCUMENTO:
                        modalidadeDocumentoIdentificador tipoDocumento = modalidadeDocumentoIdentificador.CMF;
                        var emissorDocumento = "";

                        switch (doc.Tipo.Trim())
                        {
                            case "CPF":
                                tipoDocumento = modalidadeDocumentoIdentificador.CMF;
                                emissorDocumento = "Secretaria da Receita Federal do Brasil";
                                break;
                            case "RG":
                                tipoDocumento = modalidadeDocumentoIdentificador.CI;
                                emissorDocumento = "SSP";
                                break;
                            case "CNPJ":
                                tipoDocumento = modalidadeDocumentoIdentificador.CMF;
                                emissorDocumento = "Secretaria da Receita Federal do Brasil";
                                break;
                            case "OAB":
                                tipoDocumento = modalidadeDocumentoIdentificador.OAB;
                                emissorDocumento = "Ordem dos Advogados do Brasil";
                                break;
                            default:
                                break;
                        }

                        documentos.Add(new tipoDocumentoIdentificacao()
                        {
                            nome = pAtiva.Nome.Trim(),
                            tipoDocumento = tipoDocumento,
                            emissorDocumento = emissorDocumento,
                            codigoDocumento = Util.OnlyNumbers(doc.Numero)
                        });
                    }
                }

                modalidadeGeneroPessoa genero = modalidadeGeneroPessoa.M;
                if (pAtiva.Genero == "Masculino")
                {
                    genero = modalidadeGeneroPessoa.M;
                }
                else
                {
                    if (pAtiva.Genero == "Feminino")
                    {
                        genero = modalidadeGeneroPessoa.F;
                    }
                    else
                    {
                        genero = modalidadeGeneroPessoa.D;
                    }
                }
                tipoQualificacaoPessoa tipoPessoa = tipoQualificacaoPessoa.fisica;
                if (pAtiva.TipoPessoa == "Juridica")
                {
                    tipoPessoa = tipoQualificacaoPessoa.juridica;
                }
                else
                {
                    tipoPessoa = tipoQualificacaoPessoa.fisica;
                }
                parteAtivas.Add(new tipoParte()
                {
                    pessoa = new tipoPessoa()
                    {
                        nome = pAtiva.Nome,
                        documento = documentos.ToArray(),
                        pessoaRelacionada = pessoaRelacionadas.ToArray(),
                        sexo = genero,
                        numeroDocumentoPrincipal = documentos.Count > 0 ? Util.OnlyNumbers(documentos[0].codigoDocumento) : "",
                        tipoPessoa1 = tipoPessoa,
                        nacionalidade = "BR"
                    }
                });
            }

            return parteAtivas;
        }
        #endregion

        #region ObterPartePassiva
        private List<tipoParte> ObterPartePassiva(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)
        {
            var partePassivas = new List<tipoParte>();
            foreach (var pPassiva in objDadosProcessoRetorno.MessageBody.Resposta.Processo.Partes.PartesPassivas)
            {
                var documentos = new List<tipoDocumentoIdentificacao>();

                var pessoaRelacionadas = new List<tipoRelacionamentoPessoal>();

                //CASO EXISTA OS ADVS RELACIONA A PARTE.
                if (pPassiva.Advogados != null && pPassiva.Advogados.Length > 0)
                {
                    foreach (var adv in pPassiva.Advogados)
                    {
                        pessoaRelacionadas.Add(
                            new tipoRelacionamentoPessoal()
                            {
                                pessoa = new tipoPessoa()
                                {
                                    nome = adv.Nome,
                                    documento = new tipoDocumentoIdentificacao[]
                                    {
                                        new tipoDocumentoIdentificacao()
                                        {
                                            nome = adv.Nome,
                                            tipoDocumento = modalidadeDocumentoIdentificador.OAB,
                                            codigoDocumento = Util.OnlyNumbers(adv.OAB),
                                            emissorDocumento = ""
                                        }
                                    }
                                },
                                modalidadeRelacionamento = modalidadesRelacionamentoPessoal.AP
                            });
                    }
                }

                if (pPassiva.Documentos != null && pPassiva.Documentos.Length > 0)
                {
                    foreach (var doc in pPassiva.Documentos)
                    {
                        //IDENTIFICACAO DE NOME DO DOCUMENTO:
                        modalidadeDocumentoIdentificador tipoDocumento = modalidadeDocumentoIdentificador.CMF;
                        var emissorDocumento = "";

                        switch (doc.Tipo.Trim())
                        {
                            case "CPF":
                                tipoDocumento = modalidadeDocumentoIdentificador.CMF;
                                emissorDocumento = "Secretaria da Receita Federal do Brasil";
                                break;
                            case "RG":
                                tipoDocumento = modalidadeDocumentoIdentificador.CI;
                                emissorDocumento = "SSP";
                                break;
                            case "CNPJ":
                                tipoDocumento = modalidadeDocumentoIdentificador.CMF;
                                emissorDocumento = "Secretaria da Receita Federal do Brasil";
                                break;
                            case "OAB":
                                tipoDocumento = modalidadeDocumentoIdentificador.OAB;
                                emissorDocumento = "Ordem dos Advogados do Brasil";
                                break;
                            default:
                                break;
                        }

                        documentos.Add(new tipoDocumentoIdentificacao()
                        {
                            nome = pPassiva.Nome.Trim(),
                            tipoDocumento = tipoDocumento,
                            emissorDocumento = emissorDocumento,
                            codigoDocumento = Util.OnlyNumbers(doc.Numero)
                        });
                    }
                }

                modalidadeGeneroPessoa genero = modalidadeGeneroPessoa.M;
                if (pPassiva.Genero == "Masculino")
                {
                    genero = modalidadeGeneroPessoa.M;
                }
                else
                {
                    if (pPassiva.Genero == "Feminino")
                    {
                        genero = modalidadeGeneroPessoa.F;
                    }
                    else
                    {
                        genero = modalidadeGeneroPessoa.D;
                    }
                }
                tipoQualificacaoPessoa tipoPessoa = tipoQualificacaoPessoa.fisica;
                if (pPassiva.TipoPessoa == "Juridica")
                {
                    tipoPessoa = tipoQualificacaoPessoa.juridica;
                }
                else
                {
                    tipoPessoa = tipoQualificacaoPessoa.fisica;
                }

                partePassivas.Add(new tipoParte()
                {
                    pessoa = new tipoPessoa()
                    {
                        nome = pPassiva.Nome,
                        documento = documentos.ToArray(),
                        pessoaRelacionada = pessoaRelacionadas.ToArray(),
                        sexo = genero,
                        numeroDocumentoPrincipal = documentos.Count > 0 ? Util.OnlyNumbers(documentos[0].codigoDocumento) : "",
                        tipoPessoa1 = tipoPessoa,
                        nacionalidade = "BR"
                    }
                });
            }

            return partePassivas;
        }
        #endregion

        #region ObterMovimentacoes
        /// <summary>
        /// Método para obter os dados básicos informadas no site do ESAJ - padrão HTML - e devolver no padrão do MNI PJE
        /// </summary>
        /// <param name="numeroProcesso"></param>
        /// <returns></returns>
        public List<tipoMovimentoProcessual> ObterMovimentacoes(string numeroProcesso)
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //StringBuilder dados_movimentacoes_processo = new StringBuilder();
            Dictionary<string, List<string>> jsonRetorno = new Dictionary<string, List<string>>();
            List<string> arrMovimentacoes = new List<string>();

            Dictionary<string, Object> jsonRetornoNovo = new Dictionary<string, Object>();
            Dictionary<string, Dictionary<string, string>> arrMovimentacoesNovo = new Dictionary<string, Dictionary<string, string>>();

            List<tipoMovimentoProcessual> tipoMovimentoProcessual = new List<tipoMovimentoProcessual>();

            if (numeroProcesso.Length >= 20)
            {
                try
                {
                    numeroProcesso = numeroProcesso.Replace("-", "").Replace(".", "");
                    long numProcesso = Int64.Parse(numeroProcesso);
                    //define a mascara feita pelo CNJ para realização da busca dos dados do processo no esaj. 
                    string numProcessoMascara = String.Format(@"{0:0000000\-00\.0000\.0\.00\.0000}", numProcesso);
                    //cookie gerado para permanecer a mesma sessão das requisições.
                    CookieContainer cookies = new CookieContainer();
                    //realiza a primeira busca para ir a pagina que retona o código saj do processo.
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create($"{this.Configuration["ESAJ:UrlEsajMovimentacoes"]}/searchMobile.do?dePesquisa=" + numProcessoMascara + "&localPesquisa.cdLocal=1&cbPesquisa=NUMPROC");
                    req.Method = "GET";
                    req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:15.0) Gecko/20100101 Firefox/15.0";
                    req.ContentType = "text/html; charset=utf-8";
                    req.Referer = "Web Site Referer";
                    req.KeepAlive = true;
                    req.CookieContainer = cookies;
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    //após o ok do servidor, será feita uma nova requisição para obter as movimentações do processo
                    if (res.StatusCode == HttpStatusCode.OK)
                    {
                        Stream StreamIni = res.GetResponseStream();
                        StreamReader readerInic = new StreamReader(StreamIni);
                        string answerIni = readerInic.ReadToEnd();
                        //extrai o código do processo para buscar as movimentações do processo no esaj.
                        string codigoProcesso = res.ResponseUri.Query.ToString().Substring(17, (res.ResponseUri.Query.ToString().IndexOf("&") - 17));
                        //realiza a consulta no site para obter as movimentações.
                        HttpWebRequest reqMovimentacoes = (HttpWebRequest)WebRequest.Create($"{this.Configuration["ESAJ:UrlEsajMovimentacoes"]}/obterMovimentacoes.do?processo.codigo=" + codigoProcesso + "&todasMovimentacoes=S");
                        reqMovimentacoes.Method = "GET";
                        reqMovimentacoes.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                        reqMovimentacoes.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:15.0) Gecko/20100101 Firefox/15.0";
                        reqMovimentacoes.ContentType = "text/html; charset=utf-8";
                        reqMovimentacoes.Referer = "Web Site Referer";
                        reqMovimentacoes.KeepAlive = true;
                        reqMovimentacoes.CookieContainer = cookies;

                        HttpWebResponse resMovimentacoes = (HttpWebResponse)reqMovimentacoes.GetResponse();
                        //caso o servidor retorno ok o sistema lista as moviemtações
                        if (resMovimentacoes.StatusCode == HttpStatusCode.OK)
                        {
                            Stream Stream = resMovimentacoes.GetResponseStream();
                            StreamReader reader = new StreamReader(Stream);
                            string answer = reader.ReadToEnd();
                            //Carrega o html de retorno para que o csquery possa realiar o bind do DOM
                            CQ dom = CQ.Create(answer);
                            //Percorre todos os elementos li do HTML buscando pela informação no objeto html "a"
                            int qtdMovimentacao = dom["li"].Length;
                            //verifica se o retorno é de um único registro, caso seja verifica se não possie movimentações
                            //de acordo com informação do esaj "Não foram encontradas movimentações para o processo."
                            if (qtdMovimentacao == 1)
                            {
                                if (HttpUtility.HtmlDecode(dom["li"][0].InnerHTML).Contains("Não foram encontradas movimentações para o processo"))
                                {
                                    arrMovimentacoes.Add(HttpUtility.HtmlEncode("Erro!Não foram encontradas movimentações para o processo no ESAJ!"));
                                    jsonRetorno.Add("false", arrMovimentacoes);
                                }
                            }
                            else
                            {
                                foreach (var item in dom["li"])
                                {
                                    if (item.HasChildren)
                                    {
                                        //redupera o texto do link para exibição da movimentação
                                        CQ domValor = CQ.Create(item.InnerHTML);
                                        if (domValor["a"].Length > 0)
                                        {
                                            string textoMovimentacao = this.ObterDetalheMovimentacao(codigoProcesso, qtdMovimentacao.ToString(), cookies);
                                            arrMovimentacoes.Add(HttpUtility.HtmlEncode(domValor["a"][0].InnerHTML + "||" + textoMovimentacao.Trim()));
                                            try
                                            {
                                                var dadosTratado = HttpUtility.HtmlDecode(domValor["a"][0].InnerHTML).Replace("\n", "").Replace("\t", "").Trim();
                                                string data = dadosTratado.Substring(0, 10).Trim();
                                                string[] dataFormat = data.Split("/");
                                                data = dataFormat[2] + dataFormat[1] + dataFormat[0] + "000000";
                                                string texto = dadosTratado.Substring(10).Trim();
                                                //Dictionary<string, string> dados = new Dictionary<string, string>();
                                                //dados.Add("texto", texto + " - " + HttpUtility.HtmlDecode(textoMovimentacao).Replace("\n", "").Replace("\t", "").Trim());
                                                //arrMovimentacoesNovo.Add(data, dados);
                                                var movimento = new tipoMovimentoProcessual()
                                                {
                                                    dataHora = data,
                                                    movimentoNacional = new tipoMovimentoNacional()
                                                    {
                                                        complemento = new string[] { texto + HttpUtility.HtmlDecode(textoMovimentacao).Replace("\n", "").Replace("\t", "").Trim() }
                                                    }
                                                };
                                                tipoMovimentoProcessual.Add(movimento);
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else
                                        {
                                            if (domValor["label"].Length > 0)
                                            {
                                                arrMovimentacoes.Add(HttpUtility.HtmlEncode(domValor["label"][0].InnerHTML));
                                                try
                                                {
                                                    string data = HttpUtility.HtmlDecode(domValor["label"][0].InnerHTML).Replace("\n", "").Replace("\t", "").Substring(0, 10).Trim();
                                                    string[] dataFormat = data.Split("/");
                                                    data = dataFormat[2] + dataFormat[1] + dataFormat[0] + "000000";
                                                    string texto = HttpUtility.HtmlDecode(domValor["label"][0].InnerHTML).Replace("\n", "").Replace("\t", "").Substring(10).Trim();
                                                    Dictionary<string, string> dados = new Dictionary<string, string>
                                                    {
                                                        { "texto", texto.Trim() }
                                                    };
                                                    //arrMovimentacoesNovo.Add(data, dados);
                                                    var movimento = new tipoMovimentoProcessual()
                                                    {
                                                        dataHora = data,
                                                        movimentoNacional = new tipoMovimentoNacional()
                                                        {
                                                            complemento = new string[] { texto.Trim() }
                                                        }
                                                    };
                                                    tipoMovimentoProcessual.Add(movimento);
                                                }
                                                catch
                                                {

                                                }
                                            }
                                        }
                                        qtdMovimentacao--;
                                    }
                                }
                                jsonRetornoNovo.Add("true", arrMovimentacoesNovo);
                                jsonRetorno.Add("true", arrMovimentacoes);
                            }
                        }
                        else
                        {
                            arrMovimentacoes.Add(HttpUtility.HtmlEncode("Não foi possível obter as movimentações do processo no ESAJ!"));
                            jsonRetorno.Add("false", arrMovimentacoes);
                        }
                    }
                    else
                    {
                        arrMovimentacoes.Add(HttpUtility.HtmlEncode("Não foi possível obter as movimentações do processo no ESAJ!"));
                        jsonRetorno.Add("false", arrMovimentacoes);
                    }
                }
                catch
                {
                    arrMovimentacoes.Add(HttpUtility.HtmlEncode("Erro!Ocorreu um erro ao processar a solicitação no servidor do ESAJ!"));
                    jsonRetorno.Add("false", arrMovimentacoes);
                }
            }


            //Serializa os dados para exibir o retorno das movimentações do processo.
            //var json = JsonConvert.SerializeObject(jsonRetornoNovo, Formatting.Indented);
            //jss.Serialize(jsonRetorno, dados_movimentacoes_processo);
            return tipoMovimentoProcessual;

        }
        #endregion

        #region ObterDetalheMovimentacao
        private string ObterDetalheMovimentacao(string codigoProcesso, string numMovimentacao, CookieContainer cookies)
        {
            //realiza a consulta no site para obter as movimentações.
            HttpWebRequest reqMovimentacoes = (HttpWebRequest)WebRequest.Create($"{ this.Configuration["ESAJ:UrlEsajMovimentacoes"]}/obterComplementoMovimentacao.do?processo.codigo=" + codigoProcesso + "&movimentacao=" + numMovimentacao);
            reqMovimentacoes.Method = "GET";
            reqMovimentacoes.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            reqMovimentacoes.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:15.0) Gecko/20100101 Firefox/15.0";
            reqMovimentacoes.ContentType = "text/html; charset=utf-8";
            reqMovimentacoes.Referer = "Web Site Referer";
            reqMovimentacoes.KeepAlive = true;
            reqMovimentacoes.CookieContainer = cookies;

            HttpWebResponse resMovimentacoes = (HttpWebResponse)reqMovimentacoes.GetResponse();
            //caso o servidor retorno ok o sistema lista as moviemtações
            if (resMovimentacoes.StatusCode == HttpStatusCode.OK)
            {
                Stream Stream = resMovimentacoes.GetResponseStream();
                StreamReader reader = new StreamReader(Stream);
                return reader.ReadToEnd();
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region getForosEVaras
        public Foros getForosEVaras()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getForosEVaras.");
            string retornoXmlEsaj = _Proxy.getForosEVaras();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Foros().ExtrairObjeto<Foros>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getClasseTpParte
        public Classes getClasseTpParte()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getClasseTpParte.");
            string retornoXmlEsaj = _Proxy.getClasseTpParte();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Classes().ExtrairObjeto<Classes>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getTiposDocDigital
        public Documentos getTiposDocDigital()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getTiposDocDigital.");
            string retornoXmlEsaj = _Proxy.getTiposDocDigital();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Documentos().ExtrairObjeto<Documentos>(retornoXmlEsaj);
            return objRetorno;            
        }
        #endregion

        #region getCategoriasEClasses
        public Categorias getCategoriasEClasses()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getCategoriasEClasses.");
            string retornoXmlEsaj = _Proxy.getCategoriasEClasses();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Categorias().ExtrairObjeto<Categorias>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getTiposDiversas
        public Tipos getTiposDiversas()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getTiposDiversas.");
            string retornoXmlEsaj = _Proxy.getTiposDiversas();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Tipos().ExtrairObjeto<Tipos>(retornoXmlEsaj);
            return objRetorno;            
        }
        #endregion

        #region getAreasCompetenciasEClasses
        public string getAreasCompetenciasEClasses(int cdForo)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getAreasCompetenciasEClasses.");
            return _Proxy.getAreasCompetenciasEClasses(cdForo);
        }
        #endregion

        #region obterNumeroUnificadoDoProcesso
        public string obterNumeroUnificadoDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando obterNumeroUnificadoDoProcesso.");
            return _Proxy.obterNumeroUnificadoDoProcesso(numeroProcesso);
        }
        #endregion

        #region obterNumeroSajDoProcesso
        public string obterNumeroSajDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando obterNumeroSajDoProcesso.");
            return _Proxy.obterNumeroSajDoProcesso(numeroProcesso);
        }
        #endregion

        #region getAssuntos
        public Assuntos getAssuntos(int cdCompetencia, int cdClasse)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getAssuntos.");
            string retornoXmlEsaj = _Proxy.getAssuntos(cdCompetencia, cdClasse);
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Assuntos().ExtrairObjeto<Assuntos>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region consultarSituacaoDocumentosProcesso
        public List<FilaPastaDigital> consultarSituacaoDocumentosProcesso(int Cdidea, string numeroProcesso)
        {
            //var filaPastaDigital = _dataContext.TFilaPastaDigital.ToList();
            //JOIN IN LINQ
            var retorno = (from p in _dataContext.TFilaPastaDigital
                           join s in _dataContext.TSituacaoPastaDigital on p.IdSituacaoPastaDigital equals s.IdSituacaoPastaDigital
                           join sr in _dataContext.TServidor on p.IdServidor equals sr.IdServidor
                           where (p.CdIdea == Cdidea && p.NuProcesso == numeroProcesso)
                           select new FilaPastaDigital
                           {
                               DsServidor = sr.DsServidor,
                               NuProcesso = p.NuProcesso,
                               CdIdea = p.CdIdea,
                               DsCaminhoPastaDigital = p.DsCaminhoPastaDigital,
                               DsErro = p.DsErro,
                               DtCadastro = p.DtCadastro,
                               DtFinal = p.DtFinal,
                               DtInicial = p.DtInicial,
                               DtInicioProcessamento = p.DtInicioProcessamento,
                               IdFilaPastaDigital = p.IdFilaPastaDigital,
                               IdServidor = p.IdServidor,
                               IdSituacaoPastaDigital = p.IdSituacaoPastaDigital,
                               NmServidor = sr.NmServidor,
                               NmSituacaoPastaDigital = s.NmSituacaoPastaDigital,
                               NuUltimaPaginaBaixada = p.NuUltimaPaginaBaixada
                           }
                           ).ToList();

            return retorno;
        }
        #endregion

    }
}
