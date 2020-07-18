using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Api.Entidades;
using Core.Api.Objects;
using CsQuery;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Core.Api.Integracao
{
    public class IntegracaoEsaj
    {
        public Proxy ObjProxy { get; }
        public IConfiguration Configuration { get; }
        public IntegracaoEsaj(Proxy proxy) 
        {
            this.Configuration = ConfigurationManager.ConfigurationManager.AppSettings;
            this.ObjProxy = proxy;
        }

        public consultarProcessoResponse ObterDadosProcesso(ConsultarProcesso consultarProcesso)
        {
            Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno = null;
            consultarProcessoResponse consultar = new consultarProcessoResponse();
            string xmlDadosProcessoRetorno = String.Empty;

            try
            {
                tipoProcessoJudicial tipoProcessoJudicial = new tipoProcessoJudicial();
                if (consultarProcesso.incluirCabecalho)
                {
                    Entidades.ConsultaProcesso.Message Message = new Entidades.ConsultaProcesso.Message();
                    Entidades.ConsultaProcesso.MessageMessageId MessageMessageId = new Entidades.ConsultaProcesso.MessageMessageId();
                    Entidades.ConsultaProcesso.MessageMessageBody MessageMessageBody = new Entidades.ConsultaProcesso.MessageMessageBody();

                    MessageMessageId.Code = "201220001662";
                    MessageMessageId.Date = DateTime.Now.ToString("yyyy-MM-dd"); ;
                    MessageMessageId.FromAddress = "PGMS";
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

                    xmlDadosProcessoRetorno = this.ObjProxy.ObterDadosProcesso(xml, "201220001662");
                    objDadosProcessoRetorno = new Entidades.ConsultaProcessoResposta.Message();
                    objDadosProcessoRetorno = objDadosProcessoRetorno.ExtrairObjeto<Entidades.ConsultaProcessoResposta.Message>(xmlDadosProcessoRetorno);

                    if (objDadosProcessoRetorno.MessageBody.Resposta.Mensagem.Codigo == "0")
                    {                        
                        //OBTÉM OS DADOS BÁSICOS
                        tipoProcessoJudicial.dadosBasicos = this.ObterDadosBasicos(objDadosProcessoRetorno);
                        //OBTÉM OS DADOS DA PARTE
                        tipoProcessoJudicial.dadosBasicos.polo = this.ObterPartes(objDadosProcessoRetorno).ToArray();                        
                        //ACRESCENTA A MOVIMENTAÇÃO CASO SEJA INFORMADO.
                        if (consultarProcesso.movimentos)
                        {
                            tipoProcessoJudicial.movimento = this.ObterMovimentacoes(consultarProcesso.numeroProcesso).ToArray();
                        }
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
                }
                else
                {
                    //ACRESCENTA A MOVIMENTAÇÃO CASO SEJA INFORMADO.
                    if (consultarProcesso.movimentos)
                    {
                        tipoProcessoJudicial.movimento = this.ObterMovimentacoes(consultarProcesso.numeroProcesso).ToArray();
                    }
                }

                //DEVOLVE O OBJETO DE ACORDO COM O CABEÇALHO SOLICITADO.
                consultar = new consultarProcessoResponse()
                {
                    mensagem = "Processo consultado com sucesso",
                    sucesso = true,
                    processo = tipoProcessoJudicial
                };
            }
            catch (Exception ex)
            {   
                consultar = new consultarProcessoResponse()
                {
                    mensagem = $"Erro ao tentar consultar os dados do Processo. Ex:{ex.Message}",
                    sucesso = false,
                    processo = null
                };
            }

            return consultar;
        }

        /// <summary>
        /// Método para obter os dados básicos informadas no XML do ESAJ e devolver no padrão do MNI PJE
        /// </summary>
        /// <param name="objDadosProcessoRetorno"></param>
        /// <returns></returns>
        private tipoCabecalhoProcesso ObterDadosBasicos(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)
        {
            var processo = objDadosProcessoRetorno.MessageBody.Resposta.Processo;
            var dadosBasicos = new tipoCabecalhoProcesso();

            dadosBasicos.codigoLocalidade = "1";
            dadosBasicos.dataAjuizamento = processo.DataAjuizamento;
            dadosBasicos.classeProcessual = Int32.Parse(processo.Classe.Codigo);
            dadosBasicos.competencia = 4;
            dadosBasicos.numero = processo.Numero;

            return dadosBasicos;
        }

        /// <summary>
        /// Método para obter as partes informadas no XML do ESAJ e devolver no padrão do MNI PJE
        /// </summary>
        /// <param name="objDadosProcessoRetorno"></param>
        /// <returns></returns>
        private List<tipoPoloProcessual> ObterPartes(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)
        {
            List<tipoPoloProcessual> tipoPoloProcessuais = new List<tipoPoloProcessual>();

            var partes = objDadosProcessoRetorno.MessageBody.Resposta.Processo.Partes;

            var parteAtivas = new List<tipoParte>();
            foreach (var pAtiva in partes.PartesAtivas)
            {
                var documentos = new List<tipoDocumentoIdentificacao>();

                var pessoaRelacionadas = new List<tipoRelacionamentoPessoal>();

                //CASO EXISTA OS ADVS RELACIONA A PARTE.
                if(pAtiva.Advogados != null && pAtiva.Advogados.Length > 0)
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
                                        { nome = "AOB",
                                          codigoDocumento = adv.OAB
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
                        documentos.Add(new tipoDocumentoIdentificacao()
                        {
                            nome = doc.Tipo,
                            codigoDocumento = doc.Numero                            
                        });
                    }
                }
                
                parteAtivas.Add(new tipoParte()
                {
                    pessoa = new tipoPessoa()
                    {
                        nome = pAtiva.Nome,
                        documento = documentos.ToArray(),
                        pessoaRelacionada = pessoaRelacionadas.ToArray(),
                        sexo = modalidadeGeneroPessoa.M ,
                        numeroDocumentoPrincipal = documentos.Count > 0 ? documentos[0].codigoDocumento : "",
                        tipoPessoa1 = tipoQualificacaoPessoa.fisica,
                        nacionalidade = "BR"
                    }
                });
            }

            tipoPoloProcessuais.Add(new tipoPoloProcessual()
            {
                polo = modalidadePoloProcessual.AT,
                parte = parteAtivas.ToArray(),
                poloSpecified = true
            });

            var partePassivas = new List<tipoParte>();
            foreach (var pPassiva in partes.PartesPassivas)
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
                                        { nome = "AOB",
                                          codigoDocumento = adv.OAB
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
                        documentos.Add(new tipoDocumentoIdentificacao()
                        {
                            nome = doc.Tipo,
                            codigoDocumento = doc.Numero
                        });
                    }
                }

                partePassivas.Add(new tipoParte()
                {
                    pessoa = new tipoPessoa()
                    {
                        nome = pPassiva.Nome,
                        documento = documentos.ToArray(),
                        pessoaRelacionada = pessoaRelacionadas.ToArray(),
                        sexo = modalidadeGeneroPessoa.M,
                        numeroDocumentoPrincipal = documentos.Count > 0 ? documentos[0].codigoDocumento : "",
                        tipoPessoa1 = tipoQualificacaoPessoa.fisica,
                        nacionalidade = "BR"
                    }
                });
            }

            tipoPoloProcessuais.Add(new tipoPoloProcessual()
            {
                polo = modalidadePoloProcessual.PA,
                parte = partePassivas.ToArray(),
                poloSpecified = true
            });

            return tipoPoloProcessuais;
        }

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
                                            string textoMovimentacao = this.obterDetalheMovimentacao(codigoProcesso, qtdMovimentacao.ToString(), cookies);
                                            arrMovimentacoes.Add(HttpUtility.HtmlEncode(domValor["a"][0].InnerHTML + "||" + textoMovimentacao.Trim()));
                                            try
                                            {
                                                var dadosTratado = HttpUtility.HtmlDecode(domValor["a"][0].InnerHTML).Replace("\n", "").Replace("\t", "").Trim();
                                                string data = dadosTratado.Substring(0, 10).Trim();
                                                string[] dataFormat = data.Split("/");
                                                data = dataFormat[2] + dataFormat[1] + dataFormat[0]+ "000000";
                                                string texto = dadosTratado.Substring(10).Trim();
                                                //Dictionary<string, string> dados = new Dictionary<string, string>();
                                                //dados.Add("texto", texto + " - " + HttpUtility.HtmlDecode(textoMovimentacao).Replace("\n", "").Replace("\t", "").Trim());
                                                //arrMovimentacoesNovo.Add(data, dados);
                                                var movimento = new tipoMovimentoProcessual()
                                                {
                                                    dataHora = data,
                                                    complemento = new string [] {texto + HttpUtility.HtmlDecode(textoMovimentacao).Replace("\n", "").Replace("\t", "").Trim()}                                                    
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
                                                    data = dataFormat[2] + dataFormat[1] + dataFormat[0]+ "000000";
                                                    string texto = HttpUtility.HtmlDecode(domValor["label"][0].InnerHTML).Replace("\n", "").Replace("\t", "").Substring(10).Trim();
                                                    Dictionary<string, string> dados = new Dictionary<string, string>
                                                    {
                                                        { "texto", texto.Trim() }
                                                    };
                                                    //arrMovimentacoesNovo.Add(data, dados);
                                                    var movimento = new tipoMovimentoProcessual()
                                                    {
                                                        dataHora = data,
                                                        complemento = new string[] { texto.Trim() }
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

        private string obterDetalheMovimentacao(string codigoProcesso, string numMovimentacao, CookieContainer cookies)
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
    }
}
