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
using Newtonsoft.Json;

namespace Core.Api.Integracao
{
    public class IntegracaoEsaj
    {
        public Proxy ObjProxy { get; }
        public IntegracaoEsaj(Proxy proxy) 
        {
            this.ObjProxy = proxy;
        }

        public Entidades.ConsultaProcessoResposta.Message ObterDadosProcesso(ConsultarProcesso consultarProcesso)
        {
            Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno = null;
            string xmlDadosProcessoRetorno = String.Empty;

            try
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
                if (consultarProcesso.movimentos)
                {
                    objDadosProcessoRetorno.MessageBody.Resposta.Processo.Area = this.obterMovimentacoes(consultarProcesso.numeroProcesso);
                }
            }
            catch (Exception ex)
            {
                //Não envia e-mail caso a string seja diferente de vazio!
                if (xmlDadosProcessoRetorno != String.Empty)
                {
                    //INSERE LOG CONTENDO ERRO.
                    //this.EnviarEmailErroConsultarDadosDoProcesso(new StringBuilder().Append("<![CDATA[ " + xmlDadosProcessoRetorno + "]]>").Append(" <br /> Erro na comunicação com o e-SAJ TJ-BA : " + ex.Message));
                }
                throw new Exception(" Erro na comunicação com o e-SAJ TJ-BA : " + ex.Message);
            }

            return objDadosProcessoRetorno;
        }

        public string obterMovimentacoes(string numeroProcesso)
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            StringBuilder dados_movimentacoes_processo = new StringBuilder();
            Dictionary<string, List<string>> jsonRetorno = new Dictionary<string, List<string>>();
            List<string> arrMovimentacoes = new List<string>();
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
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://esaj.tjba.jus.br/cpopg/searchMobile.do?dePesquisa=" + numProcessoMascara + "&localPesquisa.cdLocal=1&cbPesquisa=NUMPROC");
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
                        HttpWebRequest reqMovimentacoes = (HttpWebRequest)WebRequest.Create("http://esaj.tjba.jus.br/cpopg/obterMovimentacoes.do?processo.codigo=" + codigoProcesso + "&todasMovimentacoes=S");
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
                                        }
                                        else
                                        {
                                            if (domValor["label"].Length > 0)
                                            {
                                                arrMovimentacoes.Add(HttpUtility.HtmlEncode(domValor["label"][0].InnerHTML));
                                            }
                                        }
                                        qtdMovimentacao--;
                                    }
                                }
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
            var json = JsonConvert.SerializeObject(jsonRetorno, Formatting.Indented);
            //jss.Serialize(jsonRetorno, dados_movimentacoes_processo);
            return json;

        }

        private string obterDetalheMovimentacao(string codigoProcesso, string numMovimentacao, CookieContainer cookies)
        {
            //realiza a consulta no site para obter as movimentações.
            HttpWebRequest reqMovimentacoes = (HttpWebRequest)WebRequest.Create("http://esaj.tjba.jus.br/cpopg/obterComplementoMovimentacao.do?processo.codigo=" + codigoProcesso + "&movimentacao=" + numMovimentacao);
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
