using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using IntegradorIdea.Data;
using IntegradorIdea.Entidades;
using IntegradorIdea.Entidades.AssuntoClasse;
using IntegradorIdea.Entidades.CategoriaClasse;
using IntegradorIdea.Entidades.DocDigitalClasse;
using IntegradorIdea.Entidades.ForoClasse;
using IntegradorIdea.Entidades.TipoDiversasClasse;
using IntegradorIdea.Entidades.TpParteClasse;
using IntegradorIdea.Models;
using IntegradorIdea.Objects;
using CsQuery;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;

namespace IntegradorIdea.Integracao

{

    public class IntegracaoEsaj
    {
        public Proxy _proxy { get; }
        public IConfiguration _configuration { get; }

        public ILogger<IntegracaoService> _logger;
        private DataContext _dataContext { get; }
        public Log _logOperacao { get; }
        public string _ipDestino { get; set; }

        #region IntegracaoEsaj

        public IntegracaoEsaj(Proxy proxy, DataContext dataContext, ILogger<IntegracaoService> logger, string ipDestino)

        {

            _configuration = ConfigurationManager.ConfigurationManager.AppSettings;

            _proxy = proxy;

            _logger = logger;

            _dataContext = dataContext;

            _ipDestino = ipDestino;

            _logOperacao = new Log(dataContext, ipDestino);

        }

        #endregion

        #region ConsultarProcesso

        public consultarProcessoResponse ConsultarProcesso(ConsultarProcesso consultarProcesso)

        {

            Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno = null;

            consultarProcessoResponse consultar = new consultarProcessoResponse();

            string xmlDadosProcessoRetorno = String.Empty;

            var config = ConfigurationManager.ConfigurationManager.AppSettings;

            var dtInicial = DateTime.Now;



            TLogOperacao operacaoConsultarProcesso = new TLogOperacao()

            {

                CdIdea = consultarProcesso.idConsultante,

                DsCaminhoDocumentosChamada = Util.Serializar(consultarProcesso),

                DsLogOperacao = "Consulta de Processo " + consultarProcesso.numeroProcesso,

                DtInicioOperacao = dtInicial,

                DtLogOperacao = DateTime.Now,

                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"),

                IdTpRetorno = 1

            };

            //REGISTRA O LOG QUE RETORNA O VALOR COM OS DADOS PREENCHIDO DO ID

            var ResOperacaoConsultarProcesso = new TLogOperacao();

            ResOperacaoConsultarProcesso = _logOperacao.RegistrarLogOperacao(operacaoConsultarProcesso);



            try

            {

                tipoProcessoJudicial tipoProcessoJudicial = new tipoProcessoJudicial();

                _proxy._cdIdeia = consultarProcesso.idConsultante;

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

                    xmlDadosProcessoRetorno = _proxy.ObterDadosProcesso(xml, "202020007662");

                    _logger.LogInformation("RETORNO ObterDadosProcesso PROXY " + xmlDadosProcessoRetorno);



                    objDadosProcessoRetorno = new Entidades.ConsultaProcessoResposta.Message();

                    objDadosProcessoRetorno = objDadosProcessoRetorno.ExtrairObjeto<Entidades.ConsultaProcessoResposta.Message>(xmlDadosProcessoRetorno);



                    if (objDadosProcessoRetorno.MessageBody.Resposta.Mensagem.Codigo == "0")

                    {

                        var processo = objDadosProcessoRetorno.MessageBody.Resposta.Processo;

                        //OBTÉM OS DADOS BÁSICOS

                        tipoProcessoJudicial.dadosBasicos = ObterDadosBasicos(objDadosProcessoRetorno);

                        //OBTÉM OS DADOS DA PARTE

                        tipoProcessoJudicial.dadosBasicos.polo = ObterPartes(objDadosProcessoRetorno).ToArray();

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



                        if (processo.OutrosNumeros != null && processo.OutrosNumeros.Length > 0)

                        {

                            tipoProcessoJudicial.dadosBasicos.outrosnumeros = processo.OutrosNumeros;

                        }

                        //OBTÉM OS DADOS DO VALOR CAUSA

                        tipoProcessoJudicial.dadosBasicos.valorCausa = Double.Parse(processo.ValorCausa);

                        //OBTÉM OS DADOS DO ORGAO JULGADOR

                        tipoProcessoJudicial.dadosBasicos.orgaoJulgador = new tipoOrgaoJulgador()

                        {

                            codigoOrgao = processo.Vara.Codigo,

                            instancia = processo.Vara.Competencia.Descricao,

                            nomeOrgao = processo.Vara.Nome

                        };



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

                        throw new Exception(objDadosProcessoRetorno.MessageBody.Resposta.Mensagem.Descricao);

                    }

                }



                //ACRESCENTA A MOVIMENTAÇÃO CASO SEJA INFORMADO.

                if (consultarProcesso.movimentos)

                {

                    tipoProcessoJudicial.movimento = ObterMovimentacoes(consultarProcesso.numeroProcesso).ToArray();

                }

                //ACRESCENTA O DOCUMENSO CASO SEJA INFORMADO.INCLUE NA FILA DA PASTA DIGITAL.

                if (consultarProcesso.incluirDocumentos)

                {

                    //AO SELECIONAR O INCLUIR DOCUMENTOS SERÁ ADICIONADO NA FILA DA PASTA DIGITAL:

                    if (config.GetValue<bool>("Configuracoes:inserirProcessoNaFilaDaPastaDigitalConsulta"))
                    {
                        InserirFilaPastaDigital(consultarProcesso.idConsultante, consultarProcesso.numeroProcesso);
                    }

                    //OBTÉM OS DOCUMENTOS DO PROCESSO CASO JÁ TENHA SIDP FEITO O DOWNLOAD DOS DOCUMENTOS NO E-SAJ.

                    tipoProcessoJudicial.documento = ObterDocumentos(consultarProcesso.numeroProcesso, consultarProcesso.documento).ToArray();

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

                    IdLogOperacao = ResOperacaoConsultarProcesso.IdLogOperacao,

                    DsCaminhoDocumentosRetorno = Util.Serializar(consultar),

                    DtFinalOperacao = dtFinal,

                    FlOperacao = true,

                    IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"),

                    IdTpRetorno = 1

                };

                //REGISTRA O LOG

                var logOperacao = new Log(_dataContext, _ipDestino);

                logOperacao.RegistrarLogOperacao(operacao);



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

                    IdLogOperacao = operacaoConsultarProcesso.IdLogOperacao,

                    DsCaminhoDocumentosRetorno = Util.Serializar(consultar),

                    DtFinalOperacao = dtFinal,

                    IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"),

                    FlOperacao = false,

                    IdTpRetorno = 2

                };

                //REGISTRA O LOG

                _logOperacao.RegistrarLogOperacao(operacao);

            }



            return consultar;

        }

        #endregion

        #region InserirFilaPastaDigital

        private void InserirFilaPastaDigital(int cdIdea, string nuProcesso)

        {

            int? nuPaginaBaixada = 0;

            try

            {

                //OBTÉM A ÚLTIMA SITUAÇÃO DA PAGINA BAIXADA PARA INCLUIR NA FILA COM ISSO MANTENDO O DIFERENCIAL.

                var IdSituacaoPastaDigitalProcessado = _configuration.GetValue<int>("Configuracoes:situacaoPastaDigitalProcessado");

                var retornoPastadigital = _dataContext.TFilaPastaDigital

                                         .Where(fila => fila.NuProcesso == nuProcesso)

                                         .AsEnumerable()

                                         .Where(fila => fila.IdTpSituacaoPastaDigital == IdSituacaoPastaDigitalProcessado)

                                         .LastOrDefault();



                if (retornoPastadigital != null)

                {

                    nuPaginaBaixada = retornoPastadigital.NuUltimaPaginaBaixada;

                }

            }

            catch

            {



            }



            var pastaDigital = new TFilaPastaDigital()

            {

                CdIdea = cdIdea,

                DtCadastro = DateTime.Now,

                DtInicial = DateTime.Now,

                NuProcesso = nuProcesso,

                DtInicioProcessamento = DateTime.Now,

                NuUltimaPaginaBaixada = nuPaginaBaixada,

                IdTpSituacaoPastaDigital = _configuration.GetValue<int>("Configuracoes:situacaoPastaDigitalAguardando")

            };

            try

            {

                _dataContext.TFilaPastaDigital.Add(pastaDigital);

                _dataContext.SaveChanges();

            }

            catch

            {



            }



        }

        #endregion

        #region ObterDocumentos

        private List<tipoDocumento> ObterDocumentos(string numeroProcesso, string[] nmDocumentos)

        {

            var pathDirectorySeparator = Path.DirectorySeparatorChar;



            var caminhoArquivos = ConfigurationManager.ConfigurationManager.AppSettings["Diretorios:DsCaminhoProcessos"] + pathDirectorySeparator + numeroProcesso;



            var documentos = new List<tipoDocumento>();

            try

            {

                var diretorios = Directory.GetDirectories(caminhoArquivos);



                if (nmDocumentos.Length == 0)

                {

                    nmDocumentos = new string[] { "" };

                }



                foreach (var diretorio in diretorios)

                {

                    var arquivos = Directory.GetFiles(diretorio);



                    var docVinculado = new List<tipoDocumento>();

                    if (nmDocumentos.Length > 0 && nmDocumentos[0] == "")

                    {

                        foreach (var arquivo in arquivos)

                        {

                            docVinculado.Add(new tipoDocumento

                            {

                                conteudo = null,

                                descricao = Path.GetFileName(arquivo),

                                idDocumentoVinculado = Path.GetFileName(diretorio),

                                nivelSigilo = 0,

                                dataHora = DateTime.Now.ToString("yyyymmddhhmmss")

                            });



                        }

                        documentos.Add(new tipoDocumento

                        {

                            descricao = Path.GetFileName(diretorio),

                            documentoVinculado = docVinculado.ToArray(),

                            nivelSigilo = 0,

                            dataHora = DateTime.Now.ToString("yyyymmddhhmmss")

                        });

                    }

                    else

                    {

                        var insereSubPasta = false;

                        foreach (var arquivo in arquivos)

                        {

                            byte[] conteudo = null;

                            //SE CONTIVER A INFORMAÇÃO NO PRIMEIRO ITEM COM * SIGNIFICA TRAZER TODOS OS DOCUMENTOS CONTENDO O CONTEÚDO

                            if (nmDocumentos.Length > 0 && nmDocumentos[0].Contains("*"))

                            {

                                conteudo = File.ReadAllBytes(arquivo);

                            }

                            else

                            {

                                conteudo = nmDocumentos.Where(d => Path.GetFileName(arquivo).Contains(d)).FirstOrDefault() != null ? File.ReadAllBytes(arquivo) : null;

                            }



                            if (conteudo != null)

                            {

                                insereSubPasta = true;

                            }



                            docVinculado.Add(new tipoDocumento

                            {

                                conteudo = conteudo,

                                descricao = Path.GetFileName(arquivo),

                                idDocumentoVinculado = Path.GetFileName(diretorio),

                                nivelSigilo = 0,

                                dataHora = DateTime.Now.ToString("yyyymmddhhmmss")

                            });



                        }

                        if (nmDocumentos.Length > 0 && nmDocumentos[0].Contains("*"))

                        {

                            documentos.Add(new tipoDocumento

                            {

                                descricao = Path.GetFileName(diretorio),

                                documentoVinculado = docVinculado.ToArray(),

                                nivelSigilo = 0,

                                dataHora = DateTime.Now.ToString("yyyymmddhhmmss")

                            });

                        }

                        else

                        {

                            if (insereSubPasta)

                            {

                                documentos.Add(new tipoDocumento

                                {

                                    descricao = Path.GetFileName(diretorio),

                                    documentoVinculado = docVinculado.ToArray(),

                                    nivelSigilo = 0,

                                    dataHora = DateTime.Now.ToString("yyyymmddhhmmss")

                                });

                            }

                        }

                    }

                }

            }

            catch

            {



            }

            return documentos;

        }

        #endregion

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

                competencia = Int32.Parse(processo.Vara.Competencia.Codigo),

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

                parte = ObterParteAtiva(objDadosProcessoRetorno).ToArray(),

                poloSpecified = true

            });



            tipoPoloProcessuais.Add(new tipoPoloProcessual()

            {

                polo = modalidadePoloProcessual.PA,

                parte = ObterPartePassiva(objDadosProcessoRetorno).ToArray(),

                poloSpecified = true

            });



            return tipoPoloProcessuais;

        }

        #endregion

        #region ObterParteAtiva

        private List<tipoParte> ObterParteAtiva(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)

        {

            var parteAtivas = new List<tipoParte>();

            if (objDadosProcessoRetorno.MessageBody.Resposta.Processo.Partes.PartesAtivas != null)
            {
                foreach (var pAtiva in objDadosProcessoRetorno.MessageBody.Resposta.Processo.Partes.PartesAtivas)

                {

                    var documentos = new List<tipoDocumentoIdentificacao>();



                    var advogados = new List<tipoRepresentanteProcessual>();



                    //CASO EXISTA OS ADVS RELACIONA A PARTE.

                    if (pAtiva.Advogados != null && pAtiva.Advogados.Length > 0)

                    {

                        foreach (var adv in pAtiva.Advogados)

                        {

                            advogados.Add(

                                new tipoRepresentanteProcessual()

                                {

                                    nome = adv.Nome,

                                    numeroDocumentoPrincipal = adv.OAB,

                                    tipoRepresentante = modalidadeRepresentanteProcessual.A

                                });

                        }

                    }



                    if (pAtiva.Documentos != null && pAtiva.Documentos.Length > 0)

                    {

                        foreach (var doc in pAtiva.Documentos)

                        {

                            //IDENTIFICACAO DE NOME DO DOCUMENTO:

                            var tipoDocumento = "";

                            var emissorDocumento = "";



                            //BUSCA O TIPO DE DOCUMENTO USANDO A TABELA DE DE-PARA PARA TANTO.

                            var documentoParte = _dataContext.tTpDocumentoParte.Where(

                                documento => documento.SgTpDocumentoEsaj.ToUpper().Equals(doc.Tipo.ToUpper().Trim())

                                ).FirstOrDefault();



                            if (documentoParte != null)

                            {

                                tipoDocumento = documentoParte.SgTpDocumentoPje;

                                emissorDocumento = documentoParte.DsEmissorDocumento;

                            }

                            /*switch (doc.Tipo.Trim())

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

                            }*/



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

                            sexo = genero,

                            numeroDocumentoPrincipal = documentos.Count > 0 ? Util.OnlyNumbers(documentos[0].codigoDocumento) : "",

                            tipoPessoa1 = tipoPessoa,

                            nacionalidade = "BR"

                        },

                        advogado = advogados.ToArray()

                    });

                }
            }


            return parteAtivas;

        }

        #endregion

        #region ObterPartePassiva

        private List<tipoParte> ObterPartePassiva(Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno)

        {

            var partePassivas = new List<tipoParte>();

            if (objDadosProcessoRetorno.MessageBody.Resposta.Processo.Partes.PartesPassivas != null)
            {
                foreach (var pPassiva in objDadosProcessoRetorno.MessageBody.Resposta.Processo.Partes.PartesPassivas)

                {

                    var documentos = new List<tipoDocumentoIdentificacao>();



                    var advogados = new List<tipoRepresentanteProcessual>();



                    //CASO EXISTA OS ADVS RELACIONA A PARTE.

                    if (pPassiva.Advogados != null && pPassiva.Advogados.Length > 0)

                    {

                        foreach (var adv in pPassiva.Advogados)

                        {

                            advogados.Add(

                                new tipoRepresentanteProcessual()

                                {

                                    nome = adv.Nome,

                                    numeroDocumentoPrincipal = adv.OAB,

                                    tipoRepresentante = modalidadeRepresentanteProcessual.A

                                });

                        }

                    }



                    if (pPassiva.Documentos != null && pPassiva.Documentos.Length > 0)

                    {

                        foreach (var doc in pPassiva.Documentos)

                        {

                            //IDENTIFICACAO DE NOME DO DOCUMENTO:

                            var tipoDocumento = "";

                            var emissorDocumento = "";

                            //BUSCA O TIPO DE DOCUMENTO USANDO A TABELA DE DE-PARA PARA TANTO.

                            var documentoParte = _dataContext.tTpDocumentoParte.Where(

                                documento => documento.SgTpDocumentoEsaj.ToUpper().Equals(doc.Tipo.ToUpper().Trim())

                                ).FirstOrDefault();



                            if (documentoParte != null)

                            {

                                tipoDocumento = documentoParte.SgTpDocumentoPje;

                                emissorDocumento = documentoParte.DsEmissorDocumento;

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

                            sexo = genero,

                            numeroDocumentoPrincipal = documentos.Count > 0 ? Util.OnlyNumbers(documentos[0].codigoDocumento) : "",

                            tipoPessoa1 = tipoPessoa,

                            nacionalidade = "BR"

                        },

                        advogado = advogados.ToArray()

                    });

                }
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

                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create($"{_configuration["ESAJ:UrlEsajMovimentacoes"]}/searchMobile.do?dePesquisa=" + numProcessoMascara + "&localPesquisa.cdLocal=1&cbPesquisa=NUMPROC");

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

                        HttpWebRequest reqMovimentacoes = (HttpWebRequest)WebRequest.Create($"{_configuration["ESAJ:UrlEsajMovimentacoes"]}/obterMovimentacoes.do?processo.codigo=" + codigoProcesso + "&todasMovimentacoes=S");

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

                                            string textoMovimentacao = ObterDetalheMovimentacao(codigoProcesso, qtdMovimentacao.ToString(), cookies);

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
            HttpWebRequest reqMovimentacoes = (HttpWebRequest)WebRequest.Create($"{ _configuration["ESAJ:UrlEsajMovimentacoes"]}/obterComplementoMovimentacao.do?processo.codigo=" + codigoProcesso + "&movimentacao=" + numMovimentacao);
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
            string retornoXmlEsaj = _proxy.getForosEVaras();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Foros().ExtrairObjeto<Foros>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getClasseTpParte
        public Classes getClasseTpParte()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getClasseTpParte.");
            string retornoXmlEsaj = _proxy.getClasseTpParte();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Classes().ExtrairObjeto<Classes>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getTiposDocDigital
        public Documentos getTiposDocDigital()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getTiposDocDigital.");
            string retornoXmlEsaj = _proxy.getTiposDocDigital();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Documentos().ExtrairObjeto<Documentos>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getCategoriasEClasses
        public Categorias getCategoriasEClasses()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getCategoriasEClasses.");
            string retornoXmlEsaj = _proxy.getCategoriasEClasses();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Categorias().ExtrairObjeto<Categorias>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getTiposDiversas
        public Tipos getTiposDiversas()
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getTiposDiversas.");
            string retornoXmlEsaj = _proxy.getTiposDiversas();
            //LOAD DE CLASSE PARA RETORNO EM FORMATO DE OBJETO
            var objRetorno = new Tipos().ExtrairObjeto<Tipos>(retornoXmlEsaj);
            return objRetorno;
        }
        #endregion

        #region getAreasCompetenciasEClasses
        public string getAreasCompetenciasEClasses(int cdForo)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getAreasCompetenciasEClasses.");
            return _proxy.getAreasCompetenciasEClasses(cdForo);
        }
        #endregion

        #region obterNumeroUnificadoDoProcesso
        public string obterNumeroUnificadoDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando obterNumeroUnificadoDoProcesso.");
            return _proxy.obterNumeroUnificadoDoProcesso(numeroProcesso);
        }

        #endregion

        #region obterNumeroSajDoProcesso
        public string obterNumeroSajDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando obterNumeroSajDoProcesso.");
            return _proxy.obterNumeroSajDoProcesso(numeroProcesso);
        }
        #endregion

        #region getAssuntos
        public Assuntos getAssuntos(int cdCompetencia, int cdClasse)
        {
            _logger.LogInformation("IntegracaoEsaj iniciando getAssuntos.");
            string retornoXmlEsaj = _proxy.getAssuntos(cdCompetencia, cdClasse);
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

                           join s in _dataContext.tTpSituacaoPastaDigital on p.IdTpSituacaoPastaDigital equals s.IdTpSituacaoPastaDigital

                           join sr in _dataContext.TServidor on p.IdServidor equals sr.IdServidor into cl

                           from sr in cl.DefaultIfEmpty()  //LEFT JOIN NA TABELA DE TServidor

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

                               IdSituacaoPastaDigital = p.IdTpSituacaoPastaDigital,

                               NmServidor = sr.NmServidor,

                               NmSituacaoPastaDigital = s.NmTpSituacaoPastaDigital,

                               NuUltimaPaginaBaixada = p.NuUltimaPaginaBaixada

                           }

                          ).ToList();



            return retorno;

        }

        #endregion

        #region SolicitaListaCitacoesAguardandoCiencia

        private List<tipoAvisoComunicacaoPendente> SolicitaListaCitacoesAguardandoCiencia()

        {

            Entidades.SolicitaListaCitacoesAguardandoCiencia.Message MessageCitacoes = new Entidades.SolicitaListaCitacoesAguardandoCiencia.Message();

            Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageIdType MessageIdCitacoes = new Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageIdType();

            Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageMessageBody MessageBodyCitacoes = new Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageMessageBody();



            MessageIdCitacoes.Code = "202099000001";

            MessageIdCitacoes.Date = DateTime.Now.ToString("yyyy-MM-dd"); ;

            MessageIdCitacoes.FromAddress = "MP-BA";

            MessageIdCitacoes.ToAddress = "TJ";

            MessageIdCitacoes.MsgDesc = "Solicitação da quantidade e relação de processos com citações aguardando ciência";

            MessageIdCitacoes.VersionSpecified = true;

            MessageIdCitacoes.Version = Entidades.SolicitaListaCitacoesAguardandoCiencia.VersionType.Item10;

            MessageIdCitacoes.ServiceIdSpecified = true;

            MessageIdCitacoes.ServiceId = Entidades.SolicitaListaCitacoesAguardandoCiencia.ServiceIdSolicitaListaCitacoesAguardandoCiencia.SolicitaListaCitacoesAguardandoCiencia;

            MessageCitacoes.MessageId = MessageIdCitacoes;



            // Filtro

            Entidades.SolicitaListaCitacoesAguardandoCiencia.ComarcaType Comarca = new Entidades.SolicitaListaCitacoesAguardandoCiencia.ComarcaType();

            Comarca.cdComarca = "1";

            MessageBodyCitacoes.Item = Comarca;

            MessageCitacoes.MessageBody = MessageBodyCitacoes;

            // Gerando o XML

            string xml = MessageCitacoes.Serialize();



            var retornoCitacaoXML = _proxy.SolicitaListaCitacoesAguardandoCiencia(xml);



            Entidades.CitacaoAguardandoCiencia.Message message = new Entidades.CitacaoAguardandoCiencia.Message();



            message = message.ExtrairObjeto<Entidades.CitacaoAguardandoCiencia.Message>(retornoCitacaoXML);



            List<tipoAvisoComunicacaoPendente> listaTipoAvisos = new List<tipoAvisoComunicacaoPendente>();



            Compressao objCompressao = new Compressao();

            ArquivoPdf[] colArquivos;



            if (message.MessageBody.Resposta.qtIntimacoes > 0)

            {

                //LISTA AS CITAÇÕES PARA TRANSFORMAR NO FORMATO DO PJE

                foreach (var citacao in message.MessageBody.Resposta.Citacoes)

                {

                    var prazo = "";

                    var teorAto = "";

                    if (_configuration.GetValue<bool>("Configuracoes:baixarDocumentoSolicitacaoDocCienciaAto"))

                    {

                        //OBTÉM O DOCUMENTO CONTENDO INFORMAÇÃO DO ATO

                        var docAnexoAto = SolicitacaoDocCienciaAto(citacao.cdAto.ToString());

                        colArquivos = objCompressao.DescomprimirBase64(docAnexoAto);



                        foreach (var arquivoRetorno in colArquivos)

                        {

                            byte[] dadosArquivo = arquivoRetorno.Dados;

                            //Após assinatura será feito a leitura do arquivo em pdf para retirar as informações do prazo e do complemento do ato

                            //para preenchimento do model.

                            try

                            {

                                string arquivoPDF = Util.ExtrairTextoPDF(dadosArquivo);

                                //obtem no texto do pdf o prazo

                                string prazoStr = arquivoPDF.Substring(arquivoPDF.IndexOf("Prazo:") + 6, 5).Trim();



                                foreach (var ch in prazoStr)

                                {

                                    //verifica se é número e concatena a string para formar o prazo

                                    if (Char.IsNumber(ch))

                                    {

                                        prazo += ch;

                                    }

                                }



                                //obtem no texto do pdf o campo intimado

                                teorAto = arquivoPDF.Substring(arquivoPDF.IndexOf("Teor do Ato:") + 12).Trim();

                            }

                            catch

                            {



                            }

                        }

                    }



                    listaTipoAvisos.Add(new tipoAvisoComunicacaoPendente

                    {

                        dataDisponibilizacao = citacao.dtDisponibilizacao.ToString("yyyymmddhhmmss"),

                        idAviso = citacao.cdAto.ToString(),

                        tipoComunicacao = "CIT",

                        processo = new tipoCabecalhoProcesso

                        {

                            assunto = new tipoAssuntoProcessual[]

                            {

                                new tipoAssuntoProcessual{

                                    principal = true,

                                    principalSpecified = true,

                                    codigoNacional = citacao.Assunto != null ? Int32.Parse(citacao.Assunto.cdAssunto.ToString()) : 9999,

                                    codigoNacionalSpecified = true

                                }

                            },

                            numero = Util.OnlyNumbers(citacao.nuProcesso),

                            //competencia = citacao.ForoVara.cdVara,

                            //competenciaSpecified = true,

                            nivelSigilo = 0,

                            codigoLocalidade = citacao.ForoVara.cdForo.ToString(),

                            classeProcessual = citacao.Classe.cdClasse,

                            outroParametro = new tipoParametro[]

                            {

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedientePrazo",

                                    valor = prazo + " D"

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:teorAto",

                                    valor = teorAto

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDataLimiteCiencia",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDescricaoAto",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDataExpedicao",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteMeio",

                                    valor = ""

                                },

                            },

                            orgaoJulgador = new tipoOrgaoJulgador

                            {

                                codigoOrgao = citacao.ForoVara.cdVara.ToString(),

                                instancia = "ORI",

                                nomeOrgao = citacao.ForoVara.nmVara

                            }

                        }

                    });

                }

            }



            return listaTipoAvisos;

        }

        #endregion

        #region SolicitaListaIntimacoesAguardandoCiencia

        private List<tipoAvisoComunicacaoPendente> SolicitaListaIntimacoesAguardandoCiencia()

        {

            Entidades.SolicitaListaIntimacoesAguardandoCiencia.Message MessageIntimacoes = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.Message();

            Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageIdType MessageIdIntimacoes = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageIdType();

            Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageMessageBody MessageBodyIntimacoes = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageMessageBody();



            MessageIdIntimacoes.Code = "202099000001";

            MessageIdIntimacoes.Date = DateTime.Now.ToString("yyyy-MM-dd");

            MessageIdIntimacoes.FromAddress = "MP-BA";

            MessageIdIntimacoes.ToAddress = "TJ";

            MessageIdIntimacoes.MsgDesc = "Solicitação da quantidade e relação de processos com intimações aguardando ciência";

            MessageIdIntimacoes.VersionSpecified = true;

            MessageIdIntimacoes.Version = Entidades.SolicitaListaIntimacoesAguardandoCiencia.VersionType.Item10;

            MessageIdIntimacoes.ServiceIdSpecified = true;

            MessageIdIntimacoes.ServiceId = Entidades.SolicitaListaIntimacoesAguardandoCiencia.ServiceIdSolicitaListaIntimacoesAguardandoCiencia.SolicitaListaIntimacoesAguardandoCiencia;

            MessageIntimacoes.MessageId = MessageIdIntimacoes;



            Entidades.SolicitaListaIntimacoesAguardandoCiencia.ComarcaType Comarca = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.ComarcaType();

            Comarca.cdComarca = "1";

            MessageBodyIntimacoes.Item = Comarca;

            MessageIntimacoes.MessageBody = MessageBodyIntimacoes;

            string xml = MessageIntimacoes.Serialize();



            var retornoCitacaoXML = _proxy.SolicitaListaIntimacoesAguardandoCiencia(xml);



            Entidades.IntimacaoAguardandoCiencia.Message message = new Entidades.IntimacaoAguardandoCiencia.Message();



            message = message.ExtrairObjeto<Entidades.IntimacaoAguardandoCiencia.Message>(retornoCitacaoXML);



            List<tipoAvisoComunicacaoPendente> listaTipoAvisos = new List<tipoAvisoComunicacaoPendente>();



            Compressao objCompressao = new Compressao();

            ArquivoPdf[] colArquivos;



            if (message.MessageBody.Resposta.qtIntimacoes > 0)

            {

                //LISTA AS CITAÇÕES PARA TRANSFORMAR NO FORMATO DO PJE

                foreach (var intimacao in message.MessageBody.Resposta.Intimacoes)

                {

                    var prazo = "";

                    var teorAto = "";

                    if (_configuration.GetValue<bool>("Configuracoes:baixarDocumentoSolicitacaoDocCienciaAto"))

                    {

                        //OBTÉM O DOCUMENTO CONTENDO INFORMAÇÃO DO ATO

                        var docAnexoAto = SolicitacaoDocCienciaAto(intimacao.cdAto.ToString());

                        colArquivos = objCompressao.DescomprimirBase64(docAnexoAto);



                        foreach (var arquivoRetorno in colArquivos)

                        {

                            byte[] dadosArquivo = arquivoRetorno.Dados;

                            //Após assinatura será feito a leitura do arquivo em pdf para retirar as informações do prazo e do complemento do ato

                            //para preenchimento do model. 

                            try

                            {

                                string arquivoPDF = Util.ExtrairTextoPDF(dadosArquivo);

                                //obtem no texto do pdf o prazo

                                string prazoStr = arquivoPDF.Substring(arquivoPDF.IndexOf("Prazo:") + 6, 5).Trim();



                                foreach (var ch in prazoStr)

                                {

                                    //verifica se é número e concatena a string para formar o prazo

                                    if (Char.IsNumber(ch))

                                    {

                                        prazo += ch;

                                    }

                                }



                                //obtem no texto do pdf o campo intimado

                                teorAto = arquivoPDF.Substring(arquivoPDF.IndexOf("Teor do Ato:") + 12).Trim();

                            }

                            catch

                            {



                            }

                        }

                    }

                    listaTipoAvisos.Add(new tipoAvisoComunicacaoPendente

                    {

                        dataDisponibilizacao = intimacao.dtDisponibilizacao.ToString("yyyymmddhhmmss"),

                        idAviso = intimacao.cdAto.ToString(),

                        tipoComunicacao = "INT",

                        processo = new tipoCabecalhoProcesso

                        {

                            assunto = new tipoAssuntoProcessual[]

                            {

                                new tipoAssuntoProcessual{

                                    principal = true,

                                    principalSpecified = true,

                                    codigoNacional = intimacao.Assunto != null ? Int32.Parse(intimacao.Assunto.cdAssunto.ToString()) : 9999,

                                    codigoNacionalSpecified = true

                                }

                            },

                            numero = Util.OnlyNumbers(intimacao.nuProcesso),

                            //competencia = intimacao.ForoVara.cdVara,

                            //competenciaSpecified = true,

                            nivelSigilo = 0,

                            codigoLocalidade = intimacao.ForoVara.cdForo.ToString(),

                            classeProcessual = intimacao.Classe.cdClasse,

                            outroParametro = new tipoParametro[]

                            {

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedientePrazo",

                                    valor = prazo + " D"

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:teorAto",

                                    valor = teorAto

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDataLimiteCiencia",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDescricaoAto",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDataExpedicao",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteMeio",

                                    valor = ""

                                },

                            },

                            orgaoJulgador = new tipoOrgaoJulgador

                            {

                                codigoOrgao = intimacao.ForoVara.cdVara.ToString(),

                                instancia = "ORI",

                                nomeOrgao = intimacao.ForoVara.nmVara

                            }

                        }

                    });

                }

            }



            return listaTipoAvisos;

        }

        #endregion

        #region SolicitaListaIntimacoesAguardandoCienciaBD

        private void SolicitaListaIntimacoesAguardandoCienciaBD()

        {

            var dtInicial = DateTime.Now;



            Entidades.SolicitaListaIntimacoesAguardandoCiencia.Message MessageIntimacoes = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.Message();

            Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageIdType MessageIdIntimacoes = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageIdType();

            Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageMessageBody MessageBodyIntimacoes = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.MessageMessageBody();



            MessageIdIntimacoes.Code = "202099000001";

            MessageIdIntimacoes.Date = DateTime.Now.ToString("yyyy-MM-dd");

            MessageIdIntimacoes.FromAddress = "MP-BA";

            MessageIdIntimacoes.ToAddress = "TJ";

            MessageIdIntimacoes.MsgDesc = "Solicitação da quantidade e relação de processos com intimações aguardando ciência";

            MessageIdIntimacoes.VersionSpecified = true;

            MessageIdIntimacoes.Version = Entidades.SolicitaListaIntimacoesAguardandoCiencia.VersionType.Item10;

            MessageIdIntimacoes.ServiceIdSpecified = true;

            MessageIdIntimacoes.ServiceId = Entidades.SolicitaListaIntimacoesAguardandoCiencia.ServiceIdSolicitaListaIntimacoesAguardandoCiencia.SolicitaListaIntimacoesAguardandoCiencia;

            MessageIntimacoes.MessageId = MessageIdIntimacoes;



            Entidades.SolicitaListaIntimacoesAguardandoCiencia.ComarcaType Comarca = new Entidades.SolicitaListaIntimacoesAguardandoCiencia.ComarcaType();

            Comarca.cdComarca = "1";

            MessageBodyIntimacoes.Item = Comarca;

            MessageIntimacoes.MessageBody = MessageBodyIntimacoes;

            string xml = MessageIntimacoes.Serialize();



            var retornoCitacaoXML = _proxy.SolicitaListaIntimacoesAguardandoCiencia(xml);



            Entidades.IntimacaoAguardandoCiencia.Message message = new Entidades.IntimacaoAguardandoCiencia.Message();



            message = message.ExtrairObjeto<Entidades.IntimacaoAguardandoCiencia.Message>(retornoCitacaoXML);



            if (message.MessageBody.Resposta.qtIntimacoes > 0)

            {

                //LISTA AS CITAÇÕES PARA TRANSFORMAR NO FORMATO DO PJE

                foreach (var intimacao in message.MessageBody.Resposta.Intimacoes)

                {

                    //VERIFICA SE O ATO NÃO ESTÁ CADASTRADO PARA ASSIM GERAR O INSERT.

                    var resultIntimacao = _dataContext.TComunicacaoEletronica.Where(ato => ato.CdAto == intimacao.cdAto).FirstOrDefault();

                    if (resultIntimacao is null)

                    {

                        string nuProcessoSemMascara = Util.OnlyNumbers(intimacao.nuProcesso);

                        TComunicacaoEletronica comunicacaoEletronica = new TComunicacaoEletronica()

                        {

                            DtDisponibilizacao = intimacao.dtDisponibilizacao,

                            CdAto = (int)intimacao.cdAto,

                            SgTpIntimacaoCitacao = "I",

                            CdAssunto = intimacao.Assunto != null ? Int32.Parse(intimacao.Assunto.cdAssunto.ToString()) : 9999,

                            NuProcesso = nuProcessoSemMascara,

                            CdForo = (int)intimacao.ForoVara.cdForo,

                            CdVara = (int)intimacao.ForoVara.cdVara,

                            NmVara = intimacao.ForoVara.nmVara,

                            CdClasse = (int)intimacao.Classe.cdClasse,

                            DtLimiteCiencia = intimacao.dtDisponibilizacao.AddDays(10),

                            DtRecebimento = DateTime.Now

                        };

                        _dataContext.TComunicacaoEletronica.Add(comunicacaoEletronica);

                        _dataContext.SaveChanges();

                        //AO SELECIONAR O INCLUIR DOCUMENTOS SERÁ ADICIONADO NA FILA DA PASTA DIGITAL:
                        if (_configuration.GetValue<bool>("Configuracoes:inserirProcessoNaFilaDaPastaDigitalComunicacaoEletronica"))
                        {
                            InserirFilaPastaDigital(0, nuProcessoSemMascara);
                        }

                    }

                }

            }



            var dtFinal = DateTime.Now;

            //REGISTAR LOGON

            TLogOperacao operacao = new TLogOperacao()

            {

                // CdIdea = _cdIdeia,

                DsCaminhoDocumentosChamada = xml,

                DsCaminhoDocumentosRetorno = retornoCitacaoXML,

                DsLogOperacao = "SolicitaListaIntimacoesAguardandoCiencia no ESAJ: ",

                DtInicioOperacao = dtInicial,

                DtFinalOperacao = dtFinal,

                DtLogOperacao = DateTime.Now,

                FlOperacao = true,

                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoSolicitaListaIntimacoesAguardandoCiencia:id"),

                IdTpRetorno = 1

            };

            //REGISTRA O LOG

            _logOperacao.RegistrarLogOperacao(operacao);

        }

        #endregion

        #region SolicitaListaCitacoesAguardandoCienciaBD

        private void SolicitaListaCitacoesAguardandoCienciaBD()

        {

            var dtInicial = DateTime.Now;



            Entidades.SolicitaListaCitacoesAguardandoCiencia.Message MessageCitacoes = new Entidades.SolicitaListaCitacoesAguardandoCiencia.Message();

            Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageIdType MessageIdCitacoes = new Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageIdType();

            Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageMessageBody MessageBodyCitacoes = new Entidades.SolicitaListaCitacoesAguardandoCiencia.MessageMessageBody();



            MessageIdCitacoes.Code = "202099000001";

            MessageIdCitacoes.Date = DateTime.Now.ToString("yyyy-MM-dd"); ;

            MessageIdCitacoes.FromAddress = "MP-BA";

            MessageIdCitacoes.ToAddress = "TJ";

            MessageIdCitacoes.MsgDesc = "Solicitação da quantidade e relação de processos com citações aguardando ciência";

            MessageIdCitacoes.VersionSpecified = true;

            MessageIdCitacoes.Version = Entidades.SolicitaListaCitacoesAguardandoCiencia.VersionType.Item10;

            MessageIdCitacoes.ServiceIdSpecified = true;

            MessageIdCitacoes.ServiceId = Entidades.SolicitaListaCitacoesAguardandoCiencia.ServiceIdSolicitaListaCitacoesAguardandoCiencia.SolicitaListaCitacoesAguardandoCiencia;

            MessageCitacoes.MessageId = MessageIdCitacoes;



            // Filtro

            Entidades.SolicitaListaCitacoesAguardandoCiencia.ComarcaType Comarca = new Entidades.SolicitaListaCitacoesAguardandoCiencia.ComarcaType();

            Comarca.cdComarca = "1";

            MessageBodyCitacoes.Item = Comarca;

            MessageCitacoes.MessageBody = MessageBodyCitacoes;

            // Gerando o XML

            string xml = MessageCitacoes.Serialize();



            var retornoCitacaoXML = _proxy.SolicitaListaCitacoesAguardandoCiencia(xml);



            Entidades.CitacaoAguardandoCiencia.Message message = new Entidades.CitacaoAguardandoCiencia.Message();



            message = message.ExtrairObjeto<Entidades.CitacaoAguardandoCiencia.Message>(retornoCitacaoXML);



            List<tipoAvisoComunicacaoPendente> listaTipoAvisos = new List<tipoAvisoComunicacaoPendente>();



            if (message.MessageBody.Resposta.qtIntimacoes > 0)

            {

                //LISTA AS CITAÇÕES PARA TRANSFORMAR NO FORMATO DO PJE

                foreach (var citacao in message.MessageBody.Resposta.Citacoes)

                {

                    //VERIFICA SE O ATO NÃO ESTÁ CADASTRADO PARA ASSIM GERAR O INSERT.

                    var resultCitacao = _dataContext.TComunicacaoEletronica.Where(ato => ato.CdAto == citacao.cdAto).FirstOrDefault();

                    if (resultCitacao is null)

                    {

                        string nuProcessoSemMascara = Util.OnlyNumbers(citacao.nuProcesso);

                        TComunicacaoEletronica comunicacaoEletronica = new TComunicacaoEletronica()

                        {

                            DtDisponibilizacao = citacao.dtDisponibilizacao,

                            CdAto = (int)citacao.cdAto,

                            SgTpIntimacaoCitacao = "C",

                            CdAssunto = citacao.Assunto != null ? Int32.Parse(citacao.Assunto.cdAssunto.ToString()) : 9999,

                            NuProcesso = nuProcessoSemMascara,

                            CdForo = (int)citacao.ForoVara.cdForo,

                            CdVara = (int)citacao.ForoVara.cdVara,

                            NmVara = citacao.ForoVara.nmVara,

                            CdClasse = (int)citacao.Classe.cdClasse,

                            DtLimiteCiencia = citacao.dtDisponibilizacao.AddDays(10),

                            DtRecebimento = DateTime.Now

                        };


                        _dataContext.TComunicacaoEletronica.Add(comunicacaoEletronica);

                        _dataContext.SaveChanges();

                        //AO SELECIONAR O INCLUIR DOCUMENTOS SERÁ ADICIONADO NA FILA DA PASTA DIGITAL:
                        if (_configuration.GetValue<bool>("Configuracoes:inserirProcessoNaFilaDaPastaDigitalComunicacaoEletronica"))
                        {
                            InserirFilaPastaDigital(0, nuProcessoSemMascara);
                        }

                    }

                }

            }



            var dtFinal = DateTime.Now;

            //REGISTAR LOGON

            TLogOperacao operacao = new TLogOperacao()

            {

                //CdIdea = _cdIdeia,

                DsCaminhoDocumentosChamada = xml,

                DsCaminhoDocumentosRetorno = retornoCitacaoXML,

                DsLogOperacao = "SolicitaListaCitacoesAguardandoCiencia no ESAJ: ",

                DtInicioOperacao = dtInicial,

                DtFinalOperacao = dtFinal,

                DtLogOperacao = DateTime.Now,

                FlOperacao = true,

                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoSolicitaListaCitacoesAguardandoCiencia:id"),

                IdTpRetorno = 1

            };

            //REGISTRA O LOG

            _logOperacao.RegistrarLogOperacao(operacao);

        }

        #endregion

        #region ObterDocTeorAto

        public string ObterDocTeorAto()
        {

            string[] dsProcessosObterDocTeorAto = _configuration["Configuracoes:dsProcessosObterDocTeorAto"].ToString().Split(",");

            var intimacaoCitacaoDocAtos = new List<TComunicacaoEletronica>();

            if (dsProcessosObterDocTeorAto.Length > 0)
            {
                // OBTÉM SOMENTE OS ATOS DE DETERMINADOS PROCESSOS CONFORME CONFIGURAÇÃO                

                intimacaoCitacaoDocAtos = (from p in _dataContext.TComunicacaoEletronica
                                           where dsProcessosObterDocTeorAto.Contains(p.NuProcesso)
                                           select p).ToList();
            }
            else
            {
                // OBTÉM TODOS OS ATOS QUE FORAM INCLUÍDOS NA BASE E QUE ESTEJAM COM A DATA DE CIENCIA NULA E COM O DSATO VAZIO (DIFERENCIAL)
                intimacaoCitacaoDocAtos = _dataContext.TComunicacaoEletronica.Where(atos => atos.DtCiencia == null && atos.DsTeorAto == null).ToList();
            }

            Compressao objCompressao = new Compressao();

            ArquivoPdf[] colArquivos;


            if (intimacaoCitacaoDocAtos != null && intimacaoCitacaoDocAtos.Count > 0)
            {

                foreach (var intimacaoCitacao in intimacaoCitacaoDocAtos)
                {

                    string prazo = "";

                    string teorAto = "";

                    //OBTÉM O DOCUMENTO CONTENDO INFORMAÇÃO DO ATO

                    var docAnexoAto = SolicitacaoDocCienciaAto(intimacaoCitacao.CdAto.ToString());

                    colArquivos = objCompressao.DescomprimirBase64(docAnexoAto);



                    foreach (var arquivoRetorno in colArquivos)

                    {

                        if (arquivoRetorno.Nome.ToLower().Contains("ciencia"))

                        {

                            byte[] dadosArquivo = arquivoRetorno.Dados;

                            //Após assinatura será feito a leitura do arquivo em pdf para retirar as informações do prazo e do complemento do ato

                            //para preenchimento do model.

                            try

                            {

                                string arquivoPDF = Util.ExtrairTextoPDF(dadosArquivo);

                                // obtem no texto do pdf o prazo
                                string prazoStr = arquivoPDF.Substring(arquivoPDF.IndexOf("Prazo:") + 6, 5).Trim();

                                foreach (var ch in prazoStr)

                                {

                                    //verifica se é número e concatena a string para formar o prazo

                                    if (Char.IsNumber(ch))

                                    {

                                        prazo += ch;

                                    }

                                }


                                //obtem no texto do pdf o campo intimado

                                teorAto = arquivoPDF.Substring(arquivoPDF.IndexOf("Teor do Ato:") + 12).Trim();



                                //COM A INFORMAÇÃO DO ATO SERÁ ATUALIZADO O REGISTRO NA BASE COM O CAMINHO DO ARQUIVO.                                

                                intimacaoCitacao.DsTeorAto = teorAto;

                                intimacaoCitacao.NuDiasPrazo = Int32.Parse(prazo);

                                intimacaoCitacao.DsCaminhoDocumentosAnexoAtoDisponibilizado = this.SalvarArquivoCienciaAto(ref docAnexoAto, intimacaoCitacao.IdComunicacaoEletronica);



                                _dataContext.TComunicacaoEletronica.Add(intimacaoCitacao);

                                _dataContext.Update(intimacaoCitacao);

                                _dataContext.SaveChanges();



                            }

                            catch

                            {



                            }

                        }

                    }

                }

            }



            return "Concluído...";

        }

        #endregion

        #region SalvarArquivoTeorAto

        private string SalvarArquivoCienciaAto(ref string TeorAto, int idComunicacaoEletronica, string nomeArquivo = "DocumentoAnexoAto.zip")

        {

            var config = ConfigurationManager.ConfigurationManager.AppSettings;



            var caminho = config["Diretorios:DsCaminhoTeorAto"];

            var pathDirectorySeparator = Path.DirectorySeparatorChar;



            if (!Directory.Exists(caminho + pathDirectorySeparator + idComunicacaoEletronica))

            {

                Directory.CreateDirectory(caminho + pathDirectorySeparator + idComunicacaoEletronica);

            }



            var caminhoRetorno = idComunicacaoEletronica + pathDirectorySeparator.ToString() + nomeArquivo;



            var caminhoTotal = caminho + pathDirectorySeparator.ToString() + caminhoRetorno;



            File.WriteAllBytes(caminhoTotal, Convert.FromBase64String(TeorAto));



            return caminhoRetorno;

        }

        #endregion

        #region ObterIntimacaoCitacao

        public List<tipoAvisoComunicacaoPendente> ObterIntimacaoCitacao()

        {

            //APÓS OBTER OS DADOS E INSERIR NA TABELA REALIZA O PARSER PARA DEVEOLVER NO FORMATO DO PJE(MNI)

            var intimacacaoCitacaoPendentesCiencia = _dataContext.TComunicacaoEletronica.Where(atos => atos.DtCiencia == null && atos.DsCaminhoDocumentosAnexoAtoDisponibilizado != null).ToList();

            List<tipoAvisoComunicacaoPendente> listaTipoAvisos = new List<tipoAvisoComunicacaoPendente>();



            if (intimacacaoCitacaoPendentesCiencia != null && intimacacaoCitacaoPendentesCiencia.Count > 0)

            {

                foreach (var intimacaoCitacao in intimacacaoCitacaoPendentesCiencia)

                {

                    listaTipoAvisos.Add(new tipoAvisoComunicacaoPendente

                    {

                        destinatario = new tipoParteDestinatario()

                        {

                            assistenciaJudiciaria = false,

                            intimacaoPendente = 0,

                            pessoa = new tipoPessoaDestinatario()

                            {

                                nome = "MINISTERIO PUBLICO DO ESTADO DA BAHIA",

                                numeroDocumentoPrincipal = "04142491000166",

                                tipoPessoa1 = tipoQualificacaoPessoa.juridica

                            }

                        },

                        dataDisponibilizacao = intimacaoCitacao.DtDisponibilizacao.ToString("yyyymmddhhmmss"),

                        idAviso = intimacaoCitacao.CdAto.ToString(),

                        tipoComunicacao = intimacaoCitacao.SgTpIntimacaoCitacao == "I" ? "INT" : "CIT",

                        processo = new tipoCabecalhoProcesso

                        {

                            assunto = new tipoAssuntoProcessual[]

                            {

                                new tipoAssuntoProcessual{

                                    principal = true,

                                    principalSpecified = true,

                                    codigoNacional = intimacaoCitacao.CdAssunto,

                                    codigoNacionalSpecified = true

                                }

                            },

                            numero = Util.OnlyNumbers(intimacaoCitacao.NuProcesso),

                            //competencia = intimacao.ForoVara.cdVara,

                            //competenciaSpecified = true,

                            nivelSigilo = 0,

                            codigoLocalidade = intimacaoCitacao.CdForo.ToString(),

                            classeProcessual = intimacaoCitacao.CdClasse,

                            outroParametro = new tipoParametro[]

                            {

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedientePrazo",

                                    valor = intimacaoCitacao.NuDiasPrazo + " D"

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:teorAto",

                                    valor = intimacaoCitacao.DsTeorAto

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDataLimiteCiencia",

                                    valor = intimacaoCitacao.DtLimiteCiencia != null ? intimacaoCitacao.DtLimiteCiencia?.ToString("yyyymmddhhmmss") : ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDescricaoAto",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteDataExpedicao",

                                    valor = ""

                                },

                                new tipoParametro

                                {

                                    nome = "mni:pje:expedienteMeio",

                                    valor = ""

                                },

                            },

                            orgaoJulgador = new tipoOrgaoJulgador

                            {

                                codigoOrgao = intimacaoCitacao.CdVara.ToString(),

                                instancia = "ORI",

                                nomeOrgao = intimacaoCitacao.NmVara

                            }

                        }

                    });

                }

            }

            return listaTipoAvisos;

        }

        #endregion

        #region ObterIntimacaoCitacaoService

        public string ObterIntimacaoCitacaoService()

        {

            _logger.LogInformation("Obtendo SolicitaListaCitacoesAguardandoCiencia.");

            //OBTÉM INFORMAÇÃO REFERENTE A INTIMACAO DO ESAJ

            SolicitaListaIntimacoesAguardandoCienciaBD();

            _logger.LogInformation("Concluido SolicitaListaIntimacoesAguardandoCiencia.");



            //OBTÉM INFORMAÇÃO REFERENTE A CITACAO DO ESAJ

            _logger.LogInformation("Obtendo SolicitaListaCitacoesAguardandoCiencia.");

            SolicitaListaCitacoesAguardandoCienciaBD();

            _logger.LogInformation("Concluído SolicitaListaCitacoesAguardandoCiencia.");



            //APÓS OBTER OS DADOS E INSERIR NA TABELA REALIZA O PARSER PARA DEVEOLVER NO FORMATO DO PJE(MNI)



            return "Concluído...";

        }

        #endregion

        #region consultarAvisosPendentes

        public consultarAvisosPendentesResponse consultarAvisosPendentes(ConsultarAvisosPendentes consultarAvisosPendentes)

        {

            _logger.LogInformation("IntegracaoEsaj iniciando consultarAvisosPendentes.");





            var dtInicial = DateTime.Now;



            var retornoIntimacaoCitacao = this.ObterIntimacaoCitacao().ToArray();



            var consultaAvisoPendenteResposta = new consultarAvisosPendentesResponse

            {

                mensagem = $"Avisos de comunicação processual consultados com sucesso! Consultados {retornoIntimacaoCitacao.Length} de {retornoIntimacaoCitacao.Length}",

                sucesso = true,

                aviso = retornoIntimacaoCitacao

            };



            TLogOperacao operacaoConsultarAvisoPendente = new TLogOperacao()

            {

                CdIdea = consultarAvisosPendentes.idConsultante,

                DsCaminhoDocumentosChamada = Util.Serializar(consultarAvisosPendentes),

                DsCaminhoDocumentosRetorno = Util.Serializar(consultaAvisoPendenteResposta),

                DsLogOperacao = "Consultar Aviso Pendentes",

                DtInicioOperacao = dtInicial,

                DtFinalOperacao = DateTime.Now,

                DtLogOperacao = DateTime.Now,

                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultarAvisoPendentes:id"),

                IdTpRetorno = 1,

                FlOperacao = true

            };

            _logOperacao.RegistrarLogOperacao(operacaoConsultarAvisoPendente);



            _logger.LogInformation("Obtendo SolicitaListaIntimacoesAguardandoCiencia.");



            return consultaAvisoPendenteResposta;

        }

        #endregion

        #region SolicitacaoDocCienciaAto

        private string SolicitacaoDocCienciaAto(string cdAto)

        {

            Entidades.SolicitacaoDocCienciaAto.Message Message = new Entidades.SolicitacaoDocCienciaAto.Message();

            Entidades.SolicitacaoDocCienciaAto.MessageIdType MessageIdType = new Entidades.SolicitacaoDocCienciaAto.MessageIdType();

            Entidades.SolicitacaoDocCienciaAto.MessageMessageBody MessageMessageBody = new Entidades.SolicitacaoDocCienciaAto.MessageMessageBody();



            MessageIdType.Code = "202099000001";

            MessageIdType.Date = DateTime.Now.ToString("yyyy-MM-dd"); ;

            MessageIdType.FromAddress = "MP-BA";

            MessageIdType.ToAddress = "TJ";

            MessageIdType.MsgDesc = "Solicitação de documento de ciência para um ato específico";

            MessageIdType.VersionSpecified = true;

            MessageIdType.Version = Entidades.SolicitacaoDocCienciaAto.VersionType.Item10;

            MessageIdType.ServiceId = Entidades.SolicitacaoDocCienciaAto.ServiceIdSolicitacaoDocCienciaAtoType.SolicitacaoDocCienciaAto;

            Message.MessageId = MessageIdType;



            MessageMessageBody.cdAto = cdAto;

            Message.MessageBody = MessageMessageBody;



            // Gerando o XML

            string xml = Message.Serialize();



            return _proxy.SolicitacaoDocCienciaAto(xml);

        }

        #endregion

        #region consultarTeorComunicacao
        public consultarTeorComunicacaoResponse consultarTeorComunicacao(consultarTeorComunicacaoRequest consultarTeorComunicacao)
        {
            var dtInicial = DateTime.Now;
            var retornoConsultarTeorComunicacao = new consultarTeorComunicacaoResponse();
            int cdIdea = int.Parse(consultarTeorComunicacao.idConsultante);
            TLogOperacao operacaoConsultarProcesso = new TLogOperacao()
            {
                CdIdea = cdIdea,
                DsCaminhoDocumentosChamada = Util.Serializar(consultarTeorComunicacao),
                DsLogOperacao = "ConsultarTeorComunicacao no ESAJ",
                DtInicioOperacao = dtInicial,
                DtLogOperacao = DateTime.Now,
                FlOperacao = true,
                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultarTeorComunicacao:id"),
                IdTpRetorno = 1
            };

            //REGISTRA O LOG QUE RETORNA O VALOR COM OS DADOS PREENCHIDO DO ID

            var ResOperacaoConsultarProcesso = new TLogOperacao();
            ResOperacaoConsultarProcesso = _logOperacao.RegistrarLogOperacao(operacaoConsultarProcesso);

            try
            {

                //OBTÉM INFORMACAO DA INTIMACA/CITACAO ATRAVÉS DO CAMPO IDAVISO (CdAto)

                //A DEPENDER DA INFORMACAO DO CAMPO TpintimacaoCitacao O SISTEMA IRÁ SOLICITAR A CIENCIA DE ACORDO AO TIPO "I" OU "C"

                var intimacacaoCitacao = _dataContext.TComunicacaoEletronica.Where(atos =>
                                            atos.CdAto == Int32.Parse(consultarTeorComunicacao.identificadorAviso)
                                            && atos.NuProcesso == Util.OnlyNumbers(consultarTeorComunicacao.numeroProcesso)
                                        ).FirstOrDefault();

                if (intimacacaoCitacao != null)
                {
                    //ASSINA O ARQUIVO PARA O ENVIO E ATUALIZA O ARQUIVO NA BASE:
                    ArquivoPdf[] colArquivos;
                    ArquivoPdf[] ArquivoCiencia = new ArquivoPdf[1];

                    Compressao objCompressao = new Compressao();
                    //OBTÉM O ARQUIVO (NA PASTA EM CONFIGURACAO) PARA REALIZAR A ASSINATURA:
                    var documentoAtobase64 = this.ObterArquivoIntimacaoCitacaoAto(intimacacaoCitacao.DsCaminhoDocumentosAnexoAtoDisponibilizado);

                    colArquivos = objCompressao.DescomprimirBase64(documentoAtobase64);

                    foreach (ArquivoPdf arqRetororno in colArquivos)
                    {
                        if (arqRetororno.Nome.Equals("Ciencia.pdf"))
                        {
                            byte[] dadosArquivo = arqRetororno.Dados;
                            byte[] dadosArquivoAssinar = Util.AssinarPDF(ref dadosArquivo);

                            ArquivoPdf CienciaPDF = new ArquivoPdf();
                            ArquivoPdf CienciaPDF2 = CienciaPDF.AdicionarDados(ref dadosArquivoAssinar, "Ciencia.pdf");
                            ArquivoCiencia[0] = CienciaPDF2;
                        }

                    }

                    string ArquivoCienciaBase64 = objCompressao.Comprimir2Base64(ArquivoCiencia);

                    string ArquivoConfirmacaoCiencia = String.Empty;

                    if (intimacacaoCitacao.SgTpIntimacaoCitacao.Equals("I"))
                    {
                        ArquivoConfirmacaoCiencia = this.SolicitaIntimacaoAto(cdIdea, intimacacaoCitacao.CdAto.ToString(), ArquivoCienciaBase64);
                    }
                    else
                    {
                        if (intimacacaoCitacao.SgTpIntimacaoCitacao.Equals("C"))
                        {
                            ArquivoConfirmacaoCiencia = this.SolicitaCitacaoAto(cdIdea, intimacacaoCitacao.CdAto.ToString(), ArquivoCienciaBase64);
                        }
                    }

                    //APÓS OBTER OS DADOS DSO RETORNO DO ESAJ VERIFICA SE O MESMO NÃO RETORNA VAZIO. APÓS ESSE RETORNO OBTÉM O ARQUIVO DE RESPOSTA CONFIRMANDO
                    //A CIÊNCIA EFETUADA.
                    if (ArquivoConfirmacaoCiencia != String.Empty)
                    {
                        ArquivoPdf[] colArquivosDadoCiencia = objCompressao.DescomprimirBase64(ArquivoConfirmacaoCiencia);
                        ArquivoPdf[] ArquivoDadodCiencia = new ArquivoPdf[1];

                        foreach (ArquivoPdf arqRetorno in colArquivosDadoCiencia)
                        {
                            if (arqRetorno.Nome.Equals("Resposta.xml"))
                            {
                                XmlDocument oXML = new XmlDocument();
                                XmlNodeList oNoLista = default;
                                //logProcesso.AddLog("XML de retorno: " + arqRetorno.Dados);
                                oXML.Load(new MemoryStream(arqRetorno.Dados));
                                oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta/Mensagem");

                                if (oNoLista.Count > 0)
                                {
                                    string codRetorno = oNoLista[0].ChildNodes.Item(0).InnerText;

                                    if (codRetorno != "0")
                                    {
                                        throw new Exception($"A ciencia do processo {consultarTeorComunicacao.numeroProcesso} e codigo do Ato {consultarTeorComunicacao.identificadorAviso} não foi realizada. Código ESAJ: {codRetorno}");
                                    }
                                }

                                oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta/dtIntimacao");
                                if (oNoLista.Count > 0)
                                {
                                    intimacacaoCitacao.DtIntimacao = DateTime.Parse(oNoLista[0].ChildNodes.Item(0).InnerText);
                                }

                                oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta/dtMovimentacao");
                                if (oNoLista.Count > 0)
                                {
                                    intimacacaoCitacao.DtMovimentacao = DateTime.Parse(oNoLista[0].ChildNodes.Item(0).InnerText);
                                }

                                oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta/cdMovimentacao");
                                if (oNoLista.Count > 0)
                                {
                                    intimacacaoCitacao.CdMovimentacao = int.Parse(oNoLista[0].ChildNodes.Item(0).InnerText);
                                }

                                oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta/deMovimentacao");
                                if (oNoLista.Count > 0)
                                {
                                    intimacacaoCitacao.DsMovimentacao = oNoLista[0].ChildNodes.Item(0).InnerText;
                                }

                                oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta/deComplemento");
                                if (oNoLista.Count > 0)
                                {
                                    intimacacaoCitacao.DsComplemento = oNoLista[0].ChildNodes.Item(0).InnerText;
                                }

                                oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta/OutrosNumeros/nuOutroNumero");
                                if (oNoLista.Count > 0)
                                {
                                    intimacacaoCitacao.DsOutrosNumeros = oNoLista[0].ChildNodes.Item(0).InnerText;
                                }

                                /*else

                                {
                                    //O RETORNO É VÁLIDO JÁ QUE O XML NÃO CONTÉM A TAG MENSAGEM !!

                                }*/
                            }
                        }
                    }
                    else
                    {
                        throw new Exception($"A ciencia do processo {consultarTeorComunicacao.numeroProcesso} e codigo do Ato {consultarTeorComunicacao.identificadorAviso} não foi realizada. O arquivo de retorno ao ESAJ estava vazio!");
                    }

                    //APÓS VALIDAÇÃO DO RETORNO DO ARQUIVO GRAVA NA BASE DE DADOS OS ARQUIVOS DE RETORNO E NA PASTA REFERENTE AO CdAto DO PROCESSO:
                    intimacacaoCitacao.DsCaminhoDocumentosAnexoAtoEnvio = this.SalvarArquivoCienciaAto(ref ArquivoCienciaBase64, intimacacaoCitacao.IdComunicacaoEletronica, "DocumentoAnexoAtoEnvio.zip");
                    intimacacaoCitacao.DsCaminhoDocumentosAnexoAtoRetorno = this.SalvarArquivoCienciaAto(ref ArquivoConfirmacaoCiencia, intimacacaoCitacao.IdComunicacaoEletronica, "DocumentoAnexoAtoRetorno.zip");
                    intimacacaoCitacao.DtCiencia = DateTime.Now;
                    //REALIZA O UPDATE COM OS CAMINHOS DOS ARQUIVOS:
                    _dataContext.TComunicacaoEletronica.Add(intimacacaoCitacao);
                    _dataContext.Update(intimacacaoCitacao);
                    _dataContext.SaveChanges();
                    //PREENCHE O OBJETO DE RETORNO PARA EXIBIÇÃO NO SERVIÇO:
                    retornoConsultarTeorComunicacao.mensagem = "Ato recebido com sucesso!";
                    retornoConsultarTeorComunicacao.sucesso = true;
                    retornoConsultarTeorComunicacao.comunicacao = new tipoComunicacaoProcessual[] { new tipoComunicacaoProcessual()
                    {
                        documento = new tipoDocumento[]
                        {
                            new tipoDocumento() {conteudo = Convert.FromBase64String(ArquivoConfirmacaoCiencia)}
                        }
                    }};
                }

                var dtFinal = DateTime.Now;
                //REGISTAR LOGON
                TLogOperacao operacao = new TLogOperacao()
                {
                    IdLogOperacao = ResOperacaoConsultarProcesso.IdLogOperacao,
                    DsCaminhoDocumentosRetorno = Util.Serializar(retornoConsultarTeorComunicacao),
                    DtFinalOperacao = dtFinal,
                    FlOperacao = true,
                    IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultarTeorComunicacao:id"),
                    IdTpRetorno = 1
                };

                //REGISTRA O LOG
                _logOperacao.RegistrarLogOperacao(operacao);
            }
            catch (Exception ex)
            {
                retornoConsultarTeorComunicacao.mensagem = $"Erro ao tentar realizar a ciencia! Erro: {ex.Message}";
                retornoConsultarTeorComunicacao.sucesso = false;
                retornoConsultarTeorComunicacao.comunicacao = null;

                var dtFinal = DateTime.Now;
                //REGISTAR LOGON
                TLogOperacao operacao = new TLogOperacao()
                {
                    IdLogOperacao = ResOperacaoConsultarProcesso.IdLogOperacao,
                    DsCaminhoDocumentosRetorno = Util.Serializar(retornoConsultarTeorComunicacao),
                    DtFinalOperacao = dtFinal,
                    FlOperacao = true,
                    IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultarTeorComunicacao:id"),
                    IdTpRetorno = 1
                };

                //REGISTRA O LOG
                _logOperacao.RegistrarLogOperacao(operacao);
            }

            return retornoConsultarTeorComunicacao;
        }

        #endregion

        #region ObterArquivoIntimacaoCitacaoAto
        private string ObterArquivoIntimacaoCitacaoAto(string caminhoArquivo)
        {
            var caminhoDiretorio = _configuration["Diretorios:DsCaminhoTeorAto"];
            var pathDirectorySeparator = Path.DirectorySeparatorChar;
            caminhoArquivo = caminhoDiretorio + pathDirectorySeparator + caminhoArquivo;

            Byte[] bytes = File.ReadAllBytes(caminhoArquivo);

            String fileBase64 = Convert.ToBase64String(bytes);

            return fileBase64;
        }
        #endregion

        #region SolicitaIntimacaoAto
        private string SolicitaIntimacaoAto(int cdIdeia, string cdAto, string arquivoBase64)
        {
            var dtInicial = DateTime.Now;
            Entidades.SolicitaIntimacaoAto.Message Message = new Entidades.SolicitaIntimacaoAto.Message();
            Entidades.SolicitaIntimacaoAto.MessageIdType MessageIdType = new Entidades.SolicitaIntimacaoAto.MessageIdType();
            Entidades.SolicitaIntimacaoAto.MessageMessageBody MessageMessageBody = new Entidades.SolicitaIntimacaoAto.MessageMessageBody();

            MessageIdType.Code = "202099000001";
            MessageIdType.Date = DateTime.Now.ToString("yyyy-MM-dd");
            MessageIdType.FromAddress = "MP-BA";
            MessageIdType.ToAddress = "TJ";
            MessageIdType.MsgDesc = "Solicitação intimação para um ato específico";
            MessageIdType.VersionSpecified = true;
            MessageIdType.Version = Entidades.SolicitaIntimacaoAto.VersionType.Item10;
            MessageIdType.ServiceId = Entidades.SolicitaIntimacaoAto.ServiceIdSolicitacaoIntimacaoAtoType.SolicitacaoIntimacaoAto;
            Message.MessageId = MessageIdType;

            MessageMessageBody.cdAto = cdAto;
            Message.MessageBody = MessageMessageBody;

            // Gerando o XML
            string xml = Message.Serialize();
            string Dados = _proxy.SolicitacaoIntimacaoAto(xml, arquivoBase64);

            //OBTEM A INFORMACAO DO ARQUIVO DE RETORNO PARA GRAVAR SOMENTE O XML DE RETORNO. DESCOMPACTAR O ARQUIVO NO LOG
            Compressao objCompressao = new Compressao();
            string retornoArquivoResposta = "";

            if (Dados != String.Empty)
            {
                var colArquivos = objCompressao.DescomprimirBase64(Dados);
                foreach (ArquivoPdf arqRetorno in colArquivos)
                {
                    if (arqRetorno.Nome.Equals("Resposta.xml"))
                    {
                        // Carrega o contéudo do arquivo de resposta (XML) na codificação ISO-8859-1
                        string dsConteudoArquivoResposta = Encoding.GetEncoding("iso-8859-1").GetString(arqRetorno.Dados);
                        // Converte o conteúdo do arquivo de resposta para o formato UTF-8 para persistir na base corretamente
                        retornoArquivoResposta = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(dsConteudoArquivoResposta));
                    }
                }
            }

            var dtFinal = DateTime.Now;
            //REGISTAR LOGON
            TLogOperacao operacao = new TLogOperacao()
            {
                CdIdea = cdIdeia,
                DsCaminhoDocumentosChamada = xml,
                DsCaminhoDocumentosRetorno = retornoArquivoResposta,
                DsLogOperacao = "SolicitacaoIntimacaoAto no ESAJ",
                DtInicioOperacao = dtInicial,
                DtFinalOperacao = dtFinal,
                DtLogOperacao = DateTime.Now,
                FlOperacao = true,
                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoSolicitacaoIntimacaoAto:id"),
                IdTpRetorno = 1
            };

            //REGISTRA O LOG
            _logOperacao.RegistrarLogOperacao(operacao);
            return Dados;
        }

        #endregion 

        #region SolicitaCitacaoAto
        public string SolicitaCitacaoAto(int cdIdea, string cdAto, string arquivoBase64)
        {
            var dtInicial = DateTime.Now;
            Entidades.SolicitaCitacaoAto.Message Message = new Entidades.SolicitaCitacaoAto.Message();
            Entidades.SolicitaCitacaoAto.MessageIdType MessageIdType = new Entidades.SolicitaCitacaoAto.MessageIdType();
            Entidades.SolicitaCitacaoAto.MessageMessageBody MessageMessageBody = new Entidades.SolicitaCitacaoAto.MessageMessageBody();

            MessageIdType.Code = "202099000001";
            MessageIdType.Date = DateTime.Now.ToString("yyyy-MM-dd");
            MessageIdType.FromAddress = "MP-BA";
            MessageIdType.ToAddress = "TJ";
            MessageIdType.MsgDesc = "Solicitação Citacacao para um ato específico";
            MessageIdType.VersionSpecified = true;
            MessageIdType.Version = Entidades.SolicitaCitacaoAto.VersionType.Item10;
            MessageIdType.ServiceId = Entidades.SolicitaCitacaoAto.ServiceIdSolicitacaoCitacaoAtoType.SolicitacaoCitacaoAto;
            Message.MessageId = MessageIdType;
            MessageMessageBody.cdAto = cdAto;
            Message.MessageBody = MessageMessageBody;

            // Gerando o XML
            string xml = Message.Serialize();

            string Dados = _proxy.SolicitacaoCitacaoAto(xml, arquivoBase64);

            //OBTEM A INFORMACAO DO ARQUIVO DE RETORNO PARA GRAVAR SOMENTE O XML DE RETORNO. DESCOMPACTAR O ARQUIVO NO LOG
            Compressao objCompressao = new Compressao();

            string retornoArquivoResposta = "";

            if (Dados != String.Empty)
            {
                var colArquivos = objCompressao.DescomprimirBase64(Dados);
                foreach (ArquivoPdf arqRetorno in colArquivos)
                {
                    if (arqRetorno.Nome.Equals("Resposta.xml"))
                    {
                        retornoArquivoResposta = Util.Base64EncodeStream(arqRetorno.Dados);
                    }
                }
            }
            var dtFinal = DateTime.Now;

            //REGISTAR LOGON
            TLogOperacao operacao = new TLogOperacao()
            {
                CdIdea = cdIdea,
                DsCaminhoDocumentosChamada = xml,
                DsCaminhoDocumentosRetorno = retornoArquivoResposta,
                DsLogOperacao = "SolicitaCitacaoAto no ESAJ",
                DtInicioOperacao = dtInicial,
                DtFinalOperacao = dtFinal,
                DtLogOperacao = DateTime.Now,
                FlOperacao = true,
                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoSolicitacaoCitacaoAto:id"),
                IdTpRetorno = 1
            };

            //REGISTRA O LOG
            _logOperacao.RegistrarLogOperacao(operacao);

            return Dados;
        }

        #endregion

        #region entregarManifestacaoProcessual
        public entregarManifestacaoProcessualResponse entregarManifestacaoProcessual(entregarManifestacaoProcessualRequest entregarManifestacaoProcessualRequest)
        {
            string PeticaoXML = "";
            var dtInicial = DateTime.Now;

            entregarManifestacaoProcessualResponse retorno = null;

            TLogOperacao operacao = new TLogOperacao()
            {
                CdIdea = Int32.Parse(entregarManifestacaoProcessualRequest.idManifestante),
                DsCaminhoDocumentosChamada = Util.Serializar(entregarManifestacaoProcessualRequest),
                DsLogOperacao = "PeticionarIntermediariaDiversa",
                DtInicioOperacao = dtInicial,
                DtLogOperacao = DateTime.Now,
                FlOperacao = true,
                IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoEntregarManifestacaoProcessual:id"),
                IdTpRetorno = 1
            };

            //REGISTRA O LOG
            _ = new TLogOperacao();
            TLogOperacao ResOperacaoEntregarManifestacao = _logOperacao.RegistrarLogOperacao(operacao);

            try
            {
                /*string Arquivo = @"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VGltZXMtUm9tYW4vU3VidHlwZS9UeXBlMS9FbmNvZGluZy9XaW5BbnNpRW5jb2Rpbmc+PgplbmRvYmoKNyAwIG9iago8PC9Db3VudCAyL1R5cGUvUGFnZXMvSVRYVCgyLjEuNykvS2lkc1s4IDAgUiAxMCAwIFJdPj4KZW5kb2JqCjExIDAgb2JqCjw8L1R5cGUvQ2F0YWxvZy9QYWdlcyA3IDAgUj4+CmVuZG9iagoxMiAwIG9iago8PC9DcmVhdGlvbkRhdGUoRDoyMDIwMDgzMDIyNTcyOS0wMycwMCcpL1Byb2R1Y2VyKGlUZXh0IDIuMS43IGJ5IDFUM1hUKS9Nb2REYXRlKEQ6MjAyMDA4MzAyMjU3MjktMDMnMDAnKT4+CmVuZG9iagp4cmVmCjAgMTMKMDAwMDAwMDAwMCA2NTUzNSBmIAowMDAwMDAwMDE1IDAwMDAwIG4gCjAwMDAwMDAzMDcgMDAwMDAgbiAKMDAwMDA5MDY5MyAwMDAwMCBuIAowMDAwMDkwNzgyIDAwMDAwIG4gCjAwMDAwOTA4NzAgMDAwMDAgbiAKMDAwMDA4ODkzOCAwMDAwMCBuIAowMDAwMDkwOTYwIDAwMDAwIG4gCjAwMDAwODk4MTMgMDAwMDAgbiAKMDAwMDA5MDAyMiAwMDAwMCBuIAowMDAwMDkwNTI2IDAwMDAwIG4gCjAwMDAwOTEwMzAgMDAwMDAgbiAKMDAwMDA5MTA3NiAwMDAwMCBuIAp0cmFpbGVyCjw8L0lEIFs8YWZkZjMyNmMxMDY3MWMwNWViNmU0YWI0M2M4YTUxOTE+PDU3OWE4YjE0OTFlNTFjNjQ0M2M2MDJjMzg0ZTkyMDBiPl0vUm9vdCAxMSAwIFIvU2l6ZSAxMy9JbmZvIDEyIDAgUj4+CnN0YXJ0eHJlZgo5MTE5OQolJUVPRgoKMTUgMCBvYmoKPDwvRlQvU2lnL1QoU2lnbmF0dXJlMSkvViAxMyAwIFIvRiAxMzIvVHlwZS9Bbm5vdC9TdWJ0eXBlL1dpZGdldC9SZWN0WzAgMCAwIDBdL0FQPDwvTiAxNCAwIFI+Pi9QIDggMCBSL0RSPDw+Pj4+CmVuZG9iagoxMyAwIG9iago8PC9UeXBlL1NpZy9GaWx0ZXIvQWRvYmUuUFBLTGl0ZS9TdWJGaWx0ZXIvYWRiZS5wa2NzNy5kZXRhY2hlZC9SZWFzb24oKS9Mb2NhdGlvbigpL0NvbnRhY3RJbmZvKCkvTShEOjIwMjAwODMxMTgzOTE5LTAzJzAwJykvQnl0ZVJhbmdlIFswIDkxOTcyIDEyMTk3NCAxMzY5IF0gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgL0NvbnRlbnRzIDwzMDgyMGE1NzA2MDkyYTg2NDg4NmY3MGQwMTA3MDJhMDgyMGE0ODMwODIwYTQ0MDIwMTAxMzEwYjMwMDkwNjA1MmIwZTAzMDIxYTA1MDAzMDBiMDYwOTJhODY0ODg2ZjcwZDAxMDcwMWEwODIwODBjMzA4MjA4MDgzMDgyMDVmMGEwMDMwMjAxMDIwMjEwNjgwNmMxMDkxNmM3NjRmYzVkYjZlNzhhOGQwNmNjYmYzMDBkMDYwOTJhODY0ODg2ZjcwZDAxMDEwYjA1MDAzMDc4MzEwYjMwMDkwNjAzNTUwNDA2MTMwMjQyNTIzMTEzMzAxMTA2MDM1NTA0MGExMzBhNDk0MzUwMmQ0MjcyNjE3MzY5NmMzMTM2MzAzNDA2MDM1NTA0MGIxMzJkNTM2NTYzNzI2NTc0NjE3MjY5NjEyMDY0NjEyMDUyNjU2MzY1Njk3NDYxMjA0NjY1NjQ2NTcyNjE2YzIwNjQ2ZjIwNDI3MjYxNzM2OTZjMjAyZDIwNTI0NjQyMzExYzMwMWEwNjAzNTUwNDAzMTMxMzQxNDMyMDQzNjU3Mjc0Njk3MzY5Njc2ZTIwNTI0NjQyMjA0NzM1MzAxZTE3MGQzMjMwMzAzMjMwMzczMjMwMzQzMTMwMzE1YTE3MGQzMjMxMzAzMjMwMzYzMjMwMzQzMTMwMzE1YTMwODFkYTMxMGIzMDA5MDYwMzU1MDQwNjEzMDI0MjUyMzExMzMwMTEwNjAzNTUwNDBhMGMwYTQ5NDM1MDJkNDI3MjYxNzM2OTZjMzEwYjMwMDkwNjAzNTUwNDA4MGMwMjQyNDEzMTExMzAwZjA2MDM1NTA0MDcwYzA4NTM2MTZjNzY2MTY0NmY3MjMxMzYzMDM0MDYwMzU1MDQwYjBjMmQ1MzY1NjM3MjY1NzQ2MTcyNjk2MTIwNjQ2MTIwNTI2NTYzNjU2OTc0NjEyMDQ2NjU2NDY1NzI2MTZjMjA2NDZmMjA0MjcyNjE3MzY5NmMyMDJkMjA1MjQ2NDIzMTE2MzAxNDA2MDM1NTA0MGIwYzBkNTI0NjQyMjA2NTJkNDM0ZTUwNGEyMDQxMzEzMTE3MzAxNTA2MDM1NTA0MGIwYzBlMzAzNzMwMzAzMzM1MzAzNjMwMzAzMDMxMzAzMTMxMmQzMDJiMDYwMzU1MDQwMzBjMjQ0ZDU1NGU0OTQzNDk1MDQ5NGYyMDQ0NDUyMDUzNDE0YzU2NDE0NDRmNTIzYTMxMzMzOTMyMzczODMwMzEzMDMwMzAzODMxMzUzMDgyMDEyMjMwMGQwNjA5MmE4NjQ4ODZmNzBkMDEwMTAxMDUwMDAzODIwMTBmMDAzMDgyMDEwYTAyODIwMTAxMDA5Mjc1MDA5MWQyMzMxYTM2YzkxMjgxNTkwYjg2ODkzNDY5ZGM2N2FlNzYzYjhjNzljMTM1YjVkMDMzOWQ3NTVmYWYwMjk4YzA2YzAzNTU0ZjhkNjg0MzIyMmRlZjlhZDM2MWYyZWI3YTE0YTUyY2Y2NzU5OTg0ZjNiZmNiZTY2NzA5MjY1ZjI2MTk1ZDM0ZDg4Mjc0ODQ1N2QxNGUyMTkxMjc5OWNjODRmNWMyMjc0NjcxODhkZjQ4Yzk4ZmYxZmZhYzU1NDU4NjMzZjM3OWJiYzE0MTZhNTIwMWQyZWY2ZjY4ZGFlYjlmYmQyOWRhZGUwYzg1OGU0NzAzMDNjZDdiYzMxNjY4OTQyYWVhMjViOTM3MzliZWQxMGE5ODM3MzAwYzM2MWMwZTIxYTgwM2VhZTM1YTVhYWZjMjkyYzgxYzg1MjNmOWY5Njc2NjZjNDdiMGFiN2I1MjY4ZTFhMWE4NTdiODgxY2I1OTJkMDBjYzQ4OGNkODZlMzQyOTk5ZTcxYzE2NTM5ZWQwYTZiYmQ2ZWRiYzkxNGIzMmY5NGI5MDg5MTdlYzQ5NjlmMjE5ZjVkMTdmMDg3MWRhZTAxZTg0ZjY0YmIwYmYyM2QwZjNkODRhMDQwNWVkZDY1NzEyNjRiYWEwMTZiOThmMTU5ZDEyMmE4YmVhYmNhMzNkNGMwMTAyMDMwMTAwMDFhMzgyMDMyOTMwODIwMzI1MzA4MWQ4MDYwMzU1MWQxMTA0ODFkMDMwODFjZGEwM2QwNjA1NjA0YzAxMDMwNGEwMzQwNDMyMzIzNjMwMzEzMTM5MzczOTM1MzYzNTM4MzMzNDMwMzAzNTM1MzMzMDMwMzAzMDMwMzAzMDMwMzAzMDMwMzAzMDMwMzAzMDMwMzUzODMyMzMzMTMwMzkzOTM3NTM1MzUwNDI0MWEwMzMwNjA1NjA0YzAxMDMwMmEwMmEwNDI4NDE0ZTU0NGY0ZTQ5NGYyMDQzNDE1MjRjNGY1MzIwNTA0NTQ5NTg0ZjU0NGYyMDQ0NDUyMDRkNDE0NzQxNGM0ODQxNDU1MzIwNGU0NTU0NGZhMDE5MDYwNTYwNGMwMTAzMDNhMDEwMDQwZTMxMzMzOTMyMzczODMwMzEzMDMwMzAzODMxMzVhMDE3MDYwNTYwNGMwMTAzMDdhMDBlMDQwYzMwMzAzMDMwMzAzMDMwMzAzMDMwMzAzMDgxMjM3MDY3NmQ3MzJlNjM2NTcyNzQ2OTY2Njk2MzYxNjQ2ZjQwNzM2MTZjNzY2MTY0NmY3MjJlNjI2MTJlNjc2Zjc2MmU2MjcyMzAwOTA2MDM1NTFkMTMwNDAyMzAwMDMwMWYwNjAzNTUxZDIzMDQxODMwMTY4MDE0NTM3ZDdmOWRiZWQxNjFkMDIwYmFkYTlmZTM4OWE3MTM3MzU4Y2Q0MjMwN2YwNjAzNTUxZDIwMDQ3ODMwNzYzMDc0MDYwNjYwNGMwMTAyMDEwYzMwNmEzMDY4MDYwODJiMDYwMTA1MDUwNzAyMDExNjVjNjg3NDc0NzAzYTJmMmY2OTYzNzAyZDYyNzI2MTczNjk2YzJlNjM2NTcyNzQ2OTczNjk2NzZlMmU2MzZmNmQyZTYyNzIyZjcyNjU3MDZmNzM2OTc0NmY3MjY5NmYyZjY0NzA2MzJmNDE0MzVmNDM2NTcyNzQ2OTczNjk2NzZlNWY1MjQ2NDIyZjQ0NTA0MzVmNDE0MzVmNDM2NTcyNzQ2OTczNjk2NzZlNWY1MjQ2NDIyZTcwNjQ2NjMwODFiYzA2MDM1NTFkMWYwNDgxYjQzMDgxYjEzMDU3YTA1NWEwNTM4NjUxNjg3NDc0NzAzYTJmMmY2OTYzNzAyZDYyNzI2MTczNjk2YzJlNjM2NTcyNzQ2OTczNjk2NzZlMmU2MzZmNmQyZTYyNzIyZjcyNjU3MDZmNzM2OTc0NmY3MjY5NmYyZjZjNjM3MjJmNDE0MzQzNjU3Mjc0Njk3MzY5Njc2ZTUyNDY0MjQ3MzUyZjRjNjE3NDY1NzM3NDQzNTI0YzJlNjM3MjZjMzA1NmEwNTRhMDUyODY1MDY4NzQ3NDcwM2EyZjJmNjk2MzcwMmQ2MjcyNjE3MzY5NmMyZTZmNzU3NDcyNjE2YzYzNzIyZTYzNmY2ZDJlNjI3MjJmNzI2NTcwNmY3MzY5NzQ2ZjcyNjk2ZjJmNmM2MzcyMmY0MTQzNDM2NTcyNzQ2OTczNjk2NzZlNTI0NjQyNDczNTJmNGM2MTc0NjU3Mzc0NDM1MjRjMmU2MzcyNmMzMDBlMDYwMzU1MWQwZjAxMDFmZjA0MDQwMzAyMDVlMDMwMWQwNjAzNTUxZDI1MDQxNjMwMTQwNjA4MmIwNjAxMDUwNTA3MDMwMjA2MDgyYjA2MDEwNTA1MDcwMzA0MzA4MWFjMDYwODJiMDYwMTA1MDUwNzAxMDEwNDgxOWYzMDgxOWMzMDVmMDYwODJiMDYwMTA1MDUwNzMwMDI4NjUzNjg3NDc0NzAzYTJmMmY2OTYzNzAyZDYyNzI2MTczNjk2YzJlNjM2NTcyNzQ2OTczNjk2NzZlMmU2MzZmNmQyZTYyNzIyZjcyNjU3MDZmNzM2OTc0NmY3MjY5NmYyZjYzNjU3Mjc0Njk2NjY5NjM2MTY0NmY3MzJmNDE0MzVmNDM2NTcyNzQ2OTczNjk2NzZlNWY1MjQ2NDI1ZjQ3MzUyZTcwMzc2MzMwMzkwNjA4MmIwNjAxMDUwNTA3MzAwMTg2MmQ2ODc0NzQ3MDNhMmYyZjZmNjM3MzcwMmQ2MTYzMmQ2MzY1NzI3NDY5NzM2OTY3NmUyZDcyNjY2MjJlNjM2NTcyNzQ2OTczNjk2NzZlMmU2MzZmNmQyZTYyNzIzMDBkMDYwOTJhODY0ODg2ZjcwZDAxMDEwYjA1MDAwMzgyMDIwMTAwNjlkMDUxZGE5YjlkNjBmYWY5N2Q4YWRkNGNiYzJkYWIwZTk1YzdlZmEzNDY5ZTVhM2U5YjQ5MjU0YWQxMDQ4ZDQyODA0MjE5M2Y4NjljNmRiZjY4Njc3NWNmZjdjMzdjOTBiZWRjYjM0ODNiN2Q4YmM2OWU1MTk4OWI3NWNmODk4YjAxZmQwMGVlZGU3NTEyOWZkZjE5NWFlM2Q0N2M1ZTNlOWJkYWUxZDQxYzBkY2U4ZmQ4NGFmYWYzMjg1MjM5ZDgzMjE1M2IxMGIyYWRlMTJlODNmYmJkYjFjMWM3NWVmNTIzZGIzMTUwODg4ODdjMzQwYzlkOTNlMjFlMjhhZDk2NGQ5NGVjMzI5YWFlYzRmZjZkYjljYjNkMTYxYmEzYzliMzI5NjlmOGVkYTcwNWFjOTViNTg5ZjBkNTMzNjhjYjEyNGI5OTEwNmRjZmFiMzNjMWIzYWFmZTFkZDBiMTczYWYxMmQxYmNlN2ExM2UyYzA2YjBlYWY5YzhkOTE3YWYxODU4YTdjYTQ4ODAxYzdkZjBmMTgwNWU3NTA3MmY1ZDYxMzcyOTM4OThjZWM2OTUzNjQyYmYwZjg1NDljMTIzNGE2NWFmMWU3YWNiODRhY2I5ZTBlMmNhMGM4YThlY2JjODNjZmJiNDlkZGUyYmZkZTkwYzI5NGQwNWJiNDk0NWQwZmQ4NTg3MThlOTY0YmMzYWFhZWNhNmE1YTJlYzZlYjE5MTdkMjM4N2IxZTkxOTBiZGNlYTM0OTIyYzczNDQwYmEzNDYyOWE0ZmJmNzc3MjM4NjZhMWQ1ZGFmYWJmZmE4YmZiOTlmY2VlODMxMWZiMThkZDZiZjg0MzRhM2FkYThlZWQxNTFjYmQ1ODM4ZGM1MjAxOTMzOTBhMmRkMmI1Yjg5MjUwYTY1YTE1MmRjMzA0NDAzYzM3ZjQwYTVmMWQzODRjZjRmYWVlMDUwYjFmMjYxYmFmMWI2OTEzZmIzNjEzZjQ5ODU4YzUwN2VmODY4NGNkNDhmN2E3NDkyMGE4MTNlODMxMjc2NGE4NDBkZjdhODkwZWE2MTBkOTNlODRkYzE3YjcwNGM3YjZlNTA2NTA1N2RlZDhlMjkzZGRlNjBkODA2MzkxYTZkMzU5YWU3MjYwNjMxNjQ5NTU3ZjZjOTA4MmI5MGY4NTkzYzljODgwNTI5ZmIyMTUzMWJjYWM2ZDhlYzgyMjgzMjk1NjIwMTczNGJjMWM2OGQzYTNkNmJlY2VmY2I0ODU3ODZlN2UxZTY0MzRmMDhlMmIxNWFlYTFmNmVhN2JlODQ0ODcxMDk2YjFlMmI4MmQ1Nzk4NTliNWYxMDMyYTI2ZmM3NWY2OTQzMzgxOGMyZGQ1MzMxODIwMjEzMzA4MjAyMGYwMjAxMDEzMDgxOGMzMDc4MzEwYjMwMDkwNjAzNTUwNDA2MTMwMjQyNTIzMTEzMzAxMTA2MDM1NTA0MGExMzBhNDk0MzUwMmQ0MjcyNjE3MzY5NmMzMTM2MzAzNDA2MDM1NTA0MGIxMzJkNTM2NTYzNzI2NTc0NjE3MjY5NjEyMDY0NjEyMDUyNjU2MzY1Njk3NDYxMjA0NjY1NjQ2NTcyNjE2YzIwNjQ2ZjIwNDI3MjYxNzM2OTZjMjAyZDIwNTI0NjQyMzExYzMwMWEwNjAzNTUwNDAzMTMxMzQxNDMyMDQzNjU3Mjc0Njk3MzY5Njc2ZTIwNTI0NjQyMjA0NzM1MDIxMDY4MDZjMTA5MTZjNzY0ZmM1ZGI2ZTc4YThkMDZjY2JmMzAwOTA2MDUyYjBlMDMwMjFhMDUwMGEwNWQzMDE4MDYwOTJhODY0ODg2ZjcwZDAxMDkwMzMxMGIwNjA5MmE4NjQ4ODZmNzBkMDEwNzAxMzAxYzA2MDkyYTg2NDg4NmY3MGQwMTA5MDUzMTBmMTcwZDMyMzAzMDM4MzMzMTMxMzgzMzM5MzIzMDVhMzAyMzA2MDkyYTg2NDg4NmY3MGQwMTA5MDQzMTE2MDQxNGJkMDkyNjM2Njg1MzVlZWFjNzNmNmQ3YjhjMTU4NTI1OTc1MmVkNzAzMDBkMDYwOTJhODY0ODg2ZjcwZDAxMDEwMTA1MDAwNDgyMDEwMDBjNDkzZjA2Yzc5MjM4YTY1OTQzNTQwZmU3MTRiZmJjZmQ2YWE0YzllZjRhNzMzZGZhNzJjZDVhMTdkZDZjODhiMzUwMjkyYTZkYzU4ZmY4ZjM3YzUwN2U2NGU5MDFlMTdiNTNmM2MwYTQ4NjhmOGQ4YjBjZTdkOTBjNWIyYmVmYTg1MGJkOTEwNTE4NWRiYjFmMmQ1NTE2MzU4M2Y1NTRkZjBjOWVmYjVjMGRhNzA5ZTAxZmMwNzIxZTQwODc2ZWNjNjJkMTgyZTE1ZmU0NDIyNGI4ZGUxNTg4OTg1MGVhMjFiZDIzM2I0NWU1M2QyMzM0YzExNWUxMGZkYzkyOWY3NjhmODE5Y2M2NjYwOTRhMDFlNDhjZjEzODBlZmIyOGRkMTM2YWU4MGQ1MDYxOTA2ZjI3ZTRhYmVmNzk4N2I2ZmQwZTk5NDM1YTUwNGE4MWU4ZTRlNzJjNTNlZjAxMGQwYzRmOTljN2EyYzM1YzA5YTk3ZDJkYmUwODRjZTVhMjVjOTQ5ZjE1YjgwMGEwNGI2MWM5MGQzMGQ4NTM2YTZjM2UxZDc4ZDhhNDgxMTQxZGJmOWM1OTllZjgwNTRjNjY3YTRiYjZiYWMxODM0YzBkZmI4N2NiYzRmYzQwNjY5NWJhYTk5NTIzNTIxZjI4OTk5YjY5MTM1ODc0NzA0MWMxMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDA+Pj4KZW5kb2JqCjE2IDAgb2JqCjw8L1R5cGUvRm9udC9CYXNlRm9udC9IZWx2ZXRpY2EvRW5jb2RpbmcvV2luQW5zaUVuY29kaW5nL05hbWUvSGVsdi9TdWJ0eXBlL1R5cGUxPj4KZW5kb2JqCjE3IDAgb2JqCjw8L1R5cGUvRm9udC9CYXNlRm9udC9aYXBmRGluZ2JhdHMvTmFtZS9aYURiL1N1YnR5cGUvVHlwZTE+PgplbmRvYmoKMTQgMCBvYmoKPDwvVHlwZS9YT2JqZWN0L1N1YnR5cGUvRm9ybS9SZXNvdXJjZXM8PC9Qcm9jU2V0IFsvUERGIC9UZXh0IC9JbWFnZUIgL0ltYWdlQyAvSW1hZ2VJXT4+L0JCb3hbMCAwIDAgMF0vRm9ybVR5cGUgMS9NYXRyaXggWzEgMCAwIDEgMCAwXS9MZW5ndGggOC9GaWx0ZXIvRmxhdGVEZWNvZGU+PnN0cmVhbQp4nAMAAAAAAQplbmRzdHJlYW0KZW5kb2JqCjEyIDAgb2JqCjw8L0NyZWF0aW9uRGF0ZShEOjIwMjAwODMwMjI1NzI5LTAzJzAwJykvUHJvZHVjZXIoaVRleHQgMi4xLjcgYnkgMVQzWFQ7IG1vZGlmaWVkIHVzaW5nIGlUZXh0U2hhcnAgNS4yLjAgXChjXCkgMVQzWFQgQlZCQSkvTW9kRGF0ZShEOjIwMjAwODMxMTgzOTE5LTAzJzAwJyk+PgplbmRvYmoKMTEgMCBvYmoKPDwvVHlwZS9DYXRhbG9nL1BhZ2VzIDcgMCBSL0Fjcm9Gb3JtPDwvRmllbGRzWzE1IDAgUl0vREEoL0hlbHYgMCBUZiAwIGcgKS9EUjw8L0ZvbnQ8PC9IZWx2IDE2IDAgUi9aYURiIDE3IDAgUj4+Pj4vU2lnRmxhZ3MgMz4+Pj4KZW5kb2JqCjggMCBvYmoKPDwvVHlwZS9QYWdlL0NvbnRlbnRzIDYgMCBSL1BhcmVudCA3IDAgUi9SZXNvdXJjZXM8PC9YT2JqZWN0PDwvaW1nMCAxIDAgUi9pbWcxIDIgMCBSPj4vUHJvY1NldFsvUERGL1RleHQvSW1hZ2VCL0ltYWdlQy9JbWFnZUldL0ZvbnQ8PC9GMSAzIDAgUi9GMyA1IDAgUi9GMiA0IDAgUj4+Pj4vTWVkaWFCb3hbMCAwIDU5NSA4NDJdL0Fubm90c1sxNSAwIFJdPj4KZW5kb2JqCjcgMCBvYmoKPDwvQ291bnQgMi9UeXBlL1BhZ2VzL0lUWFQoNS4yLjApL0tpZHNbOCAwIFIgMTAgMCBSXT4+CmVuZG9iagp4cmVmCjAgMQowMDAwMDAwMDAwIDY1NTM1IGYgCjcgMgowMDAwMTIyOTAyIDAwMDAwIG4gCjAwMDAxMjI2ODMgMDAwMDAgbiAKMTEgNwowMDAwMTIyNTM4IDAwMDAwIG4gCjAwMDAxMjIzNjUgMDAwMDAgbiAKMDAwMDA5MTczOSAwMDAwMCBuIAowMDAwMTIyMTYwIDAwMDAwIG4gCjAwMDAwOTE2MTEgMDAwMDAgbiAKMDAwMDEyMTk4NCAwMDAwMCBuIAowMDAwMTIyMDgzIDAwMDAwIG4gCnRyYWlsZXIKPDwvU2l6ZSAxOC9Sb290IDExIDAgUi9JbmZvIDEyIDAgUi9JRCBbPDE4Yzk4Nzc1ZTkzNmUzNDE2MjI2ZjRlYTBmNzU0MjNiPjxhZjc5ODI4Nzk2NTRjNzE5MDY5MTdjMWY1NjdmMDVkOT5dL1ByZXYgOTExOTk+PgpzdGFydHhyZWYKMTIyOTcyCiUlRU9GCg==";
                MemoryStream baseInputStream = new MemoryStream(Convert.FromBase64String(Arquivo));
                //File.WriteAllBytes(@"C:\Arquivos\arquivo.pdf", Convert.FromBase64String(Arquivo));
                //ICSharpCode.SharpZipLib.Zip.ZipInputStream zipInputStream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(baseInputStream);
                FileStream fils = File.Open(@"C:\Arquivos\arquivo.pdf",FileMode.Open);
                Stream myStream = baseInputStream;

                //myStream.CopyTo()
                FileStream fs = myStream as FileStream;

                //baseInputStream.

                if (fs != null) Console.WriteLine(fs.Name);
                */
                
                //OBTÉM O XML GERADO A PARTIR DO PARSER DAS INFORMAÇÕES INFORMADOS POR PARAMETROS.
                var xmlEnvio = ObterXMLPeticionamentoIntermediarioESAJ(entregarManifestacaoProcessualRequest);
                //OBTÉM O(S) DOCUMENTO(S) GERADOS PARA ENVIO EM FORMA DE ZIP.
                var documentos = ObterDocumentoEnvioEsaj(entregarManifestacaoProcessualRequest);
                //ENVIA PARA PROXY A SOLICITAÇÃO PARA O ESAJ
                var retornoEsaj = _proxy.peticionarIntermediariaDiversa(xmlEnvio, documentos);

                //VERIFICA O RETORNO DO ARQUIVO DO ESAJ 
                Compressao objCompressao = new Compressao();
                //Utiliza a descompressão dos arquivos para verificar se o retorno foi dado como O(zero - peticionamento realizado com sucesso).
                ArquivoPdf[] colArquivos = objCompressao.DescomprimirBase64(retornoEsaj);
                //Transforma em string XML.

                int sqTipoRetornoPeticionamento = 0;
                string dsMensagemErro = String.Empty;

                foreach (ArquivoPdf arqRetorno in colArquivos)
                {                    
                    //VERIFICA SE O ARQUIVO DE RETORNO É O Resposta.xml. PARA QUE SEJA EXTRAÍDO DO PROTOCOLO DE RETORNO DO ESAJ
                    if (arqRetorno.Nome.Equals("Resposta.xml"))
                    {
                        XmlDocument oXML = new XmlDocument();
                        XmlNodeList oNoLista = default(XmlNodeList);
                        //logProcesso.AddLog("XML de retorno: " + arqRetorno.Dados);
                        oXML.Load(new MemoryStream(arqRetorno.Dados));
                        //Armazena o xml de retorno para guardar na base do SIAP como registro.
                        string dsConteudoXMLRetorno = oXML.InnerXml;
                        //Seleciona o conteúdo da arvore do XML correspondente a resposta do protocolo.
                        oNoLista = oXML.SelectNodes("Message/MessageBody/Resposta");

                        foreach (XmlNode oNo in oNoLista)
                        {
                            //Obtém o protocolo de retorno ao consumo do webservice so Esaj.
                            sqTipoRetornoPeticionamento = Int32.Parse(oNo.ChildNodes.Item(0).ChildNodes.Item(0).InnerText.Trim());
                            if(sqTipoRetornoPeticionamento != 0)
                            {
                                dsMensagemErro = oNo.ChildNodes.Item(0).ChildNodes.Item(1).InnerText;
                            }
                        }
                    }
                }

                if(sqTipoRetornoPeticionamento != 0)
                {
                    throw new Exception($"{dsMensagemErro}. Cod: {sqTipoRetornoPeticionamento}");
                }

                retorno = new entregarManifestacaoProcessualResponse()
                {
                    dataOperacao = DateTime.Now.ToString("yyyymmddhhmmss"),
                    mensagem = "Manifestação processual enviado com sucesso!",
                    recibo = Util.Base64DecodeStrem(retornoEsaj),
                    sucesso = true
                };

                var dtFinal = DateTime.Now;
                //REGISTAR LOGON
                TLogOperacao operacaoFinal = new TLogOperacao()
                {
                    IdLogOperacao = ResOperacaoEntregarManifestacao.IdLogOperacao,
                    DsCaminhoDocumentosRetorno = Util.Serializar(retorno),
                    DtFinalOperacao = dtFinal,
                    FlOperacao = true,
                    IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoEntregarManifestacaoProcessual:id"),
                    IdTpRetorno = 1
                };

                //REGISTRA O LOG 
                _logOperacao.RegistrarLogOperacao(operacaoFinal);

            }
            catch(Exception ex)
            {
                retorno = new entregarManifestacaoProcessualResponse()
                {
                    dataOperacao = DateTime.Now.ToString("yyyymmddhhmmss"),
                    mensagem = $"Erro ao entregar a manifestação processual ao ESAJ. Erro: {ex.Message}",
                    recibo = null,
                    sucesso = false
                };

                var dtFinal = DateTime.Now;
                //REGISTAR LOGON
                TLogOperacao operacaoFinal = new TLogOperacao()
                {
                    IdLogOperacao = ResOperacaoEntregarManifestacao.IdLogOperacao,
                    DsCaminhoDocumentosRetorno = Util.Serializar(retorno),
                    DtFinalOperacao = dtFinal,
                    FlOperacao = false,
                    IdTpOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoEntregarManifestacaoProcessual:id"),
                    IdTpRetorno = 1
                };

                //REGISTRA O LOG 
                _logOperacao.RegistrarLogOperacao(operacaoFinal);
            }

            return retorno;
        }
        #endregion

        #region ObterXMLPeticionamentoIntermediarioESAJ
        private string ObterXMLPeticionamentoIntermediarioESAJ(entregarManifestacaoProcessualRequest entregarManifestacaoProcessualRequest)
        {
            //Gera o XML para o envio ao peticionamento intermediário.
            Entidades.IntermediariaDiversa.Message obJDadosProcIntermediaria = new Entidades.IntermediariaDiversa.Message();

            //Instancia do <MessageId> 
            Entidades.IntermediariaDiversa.MessageIdType messageIdIntermediaria = new Entidades.IntermediariaDiversa.MessageIdType();


            messageIdIntermediaria.ServiceId = Entidades.IntermediariaDiversa.ServicePetDiversaIdType.PetDiversa;
            messageIdIntermediaria.Version = Entidades.IntermediariaDiversa.VersionType.Item10;
            messageIdIntermediaria.MsgDesc = "Peticionamento de Intermediárias Diversas";
            messageIdIntermediaria.Code = ObterCodigoUnico(); //TODO. Falar com Rander sobre o código único. Estou usando a tabela de configuracao - CODIGO_UNICO_PETICIONAMENTO
            messageIdIntermediaria.FromAddress = "";
            messageIdIntermediaria.ToAddress = "TJ";
            messageIdIntermediaria.Date = DateTime.Now.ToString("yyyy-MM-dd"); ;

            obJDadosProcIntermediaria.MessageId = messageIdIntermediaria;                       

            //Instancia do <MessageBody>
            Entidades.IntermediariaDiversa.MessageMessageBody messageBodyIntermediario = new Entidades.IntermediariaDiversa.MessageMessageBody();

            //Instancia de <peticao>
            Entidades.IntermediariaDiversa.PeticaoType peticaoIntermediaria = new Entidades.IntermediariaDiversa.PeticaoType();
            peticaoIntermediaria.Processo = Util.OnlyNumbers(entregarManifestacaoProcessualRequest.numeroProcesso);
            peticaoIntermediaria.Foro = "001";//TODO. Verificar se vai consultar o processo para obter essa informação ou se irá ser passado via objeto de parametro para o Diversas

            peticaoIntermediaria.NomePeticao = entregarManifestacaoProcessualRequest.documento[0].descricao; //TODO. Perguntar se existe a possibilidade de colocar o nome do arquivo no campo de descrição.
            //OU pode ser extraído o nome do arquivo decodeando o mesmo.
            peticaoIntermediaria.Tipo = "8050006"; //Tipo Peticao diversa.            

            messageBodyIntermediario.Peticao = peticaoIntermediaria;
            //Instancia do <Partes>
            Entidades.IntermediariaDiversa.ParteType[] parteIntermediariaArr = new Entidades.IntermediariaDiversa.ParteType[1];
            Entidades.IntermediariaDiversa.ParteType parteIntermediaria = new Entidades.IntermediariaDiversa.ParteType();
            parteIntermediaria.Nome = _configuration["Configuracoes:parteIntermediariaNome"];
            parteIntermediaria.Tipo = _configuration["Configuracoes:parteIntermediariaTipo"];
            parteIntermediariaArr[0] = parteIntermediaria;

            messageBodyIntermediario.Partes = parteIntermediariaArr;

            int i = 0;
            //ARMAZENA INICIALMENTE A PETIÇÃO
            Entidades.IntermediariaDiversa.DocumentoType[] documentosArr = new Entidades.IntermediariaDiversa.DocumentoType[entregarManifestacaoProcessualRequest.documento[0].documentoVinculado.Length];
            
            if (entregarManifestacaoProcessualRequest.documento[0].documentoVinculado.Length > 0)
            {
                //Insere as informações dos documentos                
                //A partir do resultado dos documentos cria-se uma linha de documento 
                Entidades.IntermediariaDiversa.DocumentoType documento = null;
                foreach (Documento item in entregarManifestacaoProcessualRequest.documento[0].documentoVinculado)
                {
                    documento = new Entidades.IntermediariaDiversa.DocumentoType();
                    documento.Tipo = "8200002";
                    //Tratar o nome do arquivo
                    //string[] infoArquivo = item.NomeArquivo.Split('/');
                    //Obtém o último registro que contém a informação do nome e extensão do arquivo.
                    documento.Nome = item.descricao;
                    documentosArr[i] = documento;
                    i++;
                }

                if (documentosArr.Length > 0)
                {
                    messageBodyIntermediario.Documentos = documentosArr;
                }
            }
            else
            {   
                messageBodyIntermediario.Documentos = documentosArr;
            }

            obJDadosProcIntermediaria.MessageBody = messageBodyIntermediario;
            //RETORNA O XML GERADO PELO OBJETO.
            return obJDadosProcIntermediaria.Serialize();
        }
        #endregion

        #region ObterDocumentoEnvioEsaj
        private string ObterDocumentoEnvioEsaj(entregarManifestacaoProcessualRequest entregarManifestacaoProcessualRequest)
        {
            ArquivoPdf[] documentosEnvio = new ArquivoPdf[(1 + entregarManifestacaoProcessualRequest.documento[0].documentoVinculado.Length)];

            Compressao objCompressao = new Compressao();

            var docPeticao = entregarManifestacaoProcessualRequest.documento[0].conteudo;

            ArquivoPdf peticao = new ArquivoPdf();
            ArquivoPdf peticaoAux = peticao.AdicionarDados(ref docPeticao, entregarManifestacaoProcessualRequest.documento[0].descricao);
            //DOCUMENTO INCIAL É A PETIÇÃO 
            documentosEnvio[0] = peticaoAux;
            int i = 1;
            foreach (var arqRetorno in entregarManifestacaoProcessualRequest.documento[0].documentoVinculado)
            {
                byte[] dadosArquivo = arqRetorno.conteudo;
                ArquivoPdf doc = new ArquivoPdf();
                ArquivoPdf docAux = doc.AdicionarDados(ref dadosArquivo, arqRetorno.descricao);
                documentosEnvio[i] = docAux;
                i++;
            }

            string ArquivoCienciaBase64 = objCompressao.Comprimir2Base64(documentosEnvio);

            return ArquivoCienciaBase64;
        }
        #endregion

        #region ObterCodigoUnico
        private string ObterCodigoUnico()
        {
            //"dsConfiguracaoCodigoUnico": "CODIGO_UNICO_PETICIONAMENTO"
            var configuracaoRetorno = _dataContext.TConfiguracao.Where(c => c.DsChave.Equals(_configuration["Configuracoes:dsConfiguracaoCodigoUnico"])).FirstOrDefault();
            string codigoUnico = String.Empty;
            if (configuracaoRetorno != null)
            {
                try
                {
                    //VERIFICA SE ESTÁ NO MESMO ANO. CASO TENHA MODIFICADO SERÁ ZERADO O CONTADO COM O INÍCIO DO ANO.
                    if(Int32.Parse(configuracaoRetorno.DsValor.Substring(0,4)) != DateTime.Now.Year)
                    {
                        configuracaoRetorno.DsValor = DateTime.Now.Year.ToString() + "01000001";
                    }
                    else
                    {
                        //OBTÉM O VALOR ATUAL E INCREMENTA EM MAIS 1 ATUALIZANDO O VALOR AO OBJETO
                        configuracaoRetorno.DsValor = (Int64.Parse(configuracaoRetorno.DsValor) + 1).ToString();
                    }
                    _dataContext.TConfiguracao.Add(configuracaoRetorno);
                    _dataContext.TConfiguracao.Update(configuracaoRetorno);
                    _dataContext.SaveChanges();

                    codigoUnico = configuracaoRetorno.DsValor;
                }
                catch
                {

                }
            }

            return codigoUnico;
        }
        #endregion

    }

}

