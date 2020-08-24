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
                IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"),
                IdTipoRetorno = 1
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

                        if(processo.OutrosNumeros != null && processo.OutrosNumeros.Length > 0)
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
                    if (config.GetValue<bool>("Configuracoes:inserirProcessoNaFilaDaPastaDigital"))
                    {
                        InserirFilaPastaDigital(consultarProcesso);
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
                    IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"),
                    IdTipoRetorno = 1
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
                    IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultaProcesso:id"),
                    FlOperacao = false,                    
                    IdTipoRetorno = 2
                };
                //REGISTRA O LOG
                _logOperacao.RegistrarLogOperacao(operacao);
            }

            return consultar;
        }
        #endregion

        #region InserirFilaPastaDigital
        private void InserirFilaPastaDigital(ConsultarProcesso consultarProcesso)
        {
            int? nuPaginaBaixada = 0;
            try
            {
                //OBTÉM A ÚLTIMA SITUAÇÃO DA PAGINA BAIXADA PARA INCLUIR NA FILA COM ISSO MANTENDO O DIFERENCIAL.
                var IdSituacaoPastaDigitalProcessado = _configuration.GetValue<int>("Configuracoes:situacaoPastaDigitalProcessado");
                var retornoPastadigital = _dataContext.TFilaPastaDigital
                                         .Where(fila => fila.NuProcesso == consultarProcesso.numeroProcesso)
                                         .AsEnumerable()
                                         .Where(fila => fila.IdSituacaoPastaDigital == IdSituacaoPastaDigitalProcessado)
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
                CdIdea = consultarProcesso.idConsultante,
                DtCadastro = DateTime.Now,
                DtInicial = DateTime.Now,
                NuProcesso = consultarProcesso.numeroProcesso,
                DtInicioProcessamento = DateTime.Now,
                NuUltimaPaginaBaixada = nuPaginaBaixada,
                IdSituacaoPastaDigital = _configuration.GetValue<int>("Configuracoes:situacaoPastaDigitalAguardando")
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
                        var documentoParte = _dataContext.TTipoDocumentoParte.Where(
                            documento => documento.SgTipoDocumentoEsaj.ToUpper().Equals(doc.Tipo.ToUpper().Trim())
                            ).FirstOrDefault();

                        if(documentoParte != null)
                        {
                            tipoDocumento = documentoParte.SgTipoDocumentoPje;
                            emissorDocumento = documentoParte.DsDescricaoEmissorDocumento;
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
                        var documentoParte = _dataContext.TTipoDocumentoParte.Where(
                            documento => documento.SgTipoDocumentoEsaj.ToUpper().Equals(doc.Tipo.ToUpper().Trim())
                            ).FirstOrDefault();

                        if (documentoParte != null)
                        {
                            tipoDocumento = documentoParte.SgTipoDocumentoPje;
                            emissorDocumento = documentoParte.DsDescricaoEmissorDocumento;
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
                           join s in _dataContext.TSituacaoPastaDigital on p.IdSituacaoPastaDigital equals s.IdSituacaoPastaDigital
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
                               IdSituacaoPastaDigital = p.IdSituacaoPastaDigital,
                               NmServidor = sr.NmServidor,
                               NmSituacaoPastaDigital = s.NmSituacaoPastaDigital,
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

            var retornoCitacaoXML =_proxy.SolicitaListaIntimacoesAguardandoCiencia(xml);

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
                        TComunicacaoEletronica comunicacaoEletronica = new TComunicacaoEletronica()
                        {
                            DtDisponibilizacao = intimacao.dtDisponibilizacao,
                            CdAto = (int)intimacao.cdAto,
                            TpIntimacaoCitacao = "I",
                            CdAssunto = intimacao.Assunto != null ? Int32.Parse(intimacao.Assunto.cdAssunto.ToString()) : 9999,
                            NuProcesso = Util.OnlyNumbers(intimacao.nuProcesso),
                            CdForo = (int)intimacao.ForoVara.cdForo,
                            CdVara = (int)intimacao.ForoVara.cdVara,
                            DtLimiteCiencia = intimacao.dtDisponibilizacao.AddDays(10)
                        };
                        _dataContext.TComunicacaoEletronica.Add(comunicacaoEletronica);
                        _dataContext.SaveChanges();
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
                DsLogOperacao = "SolicitaListaIntimacoesAguardandoCiencia no ESAJ: ",
                DtInicioOperacao = dtInicial,
                DtFinalOperacao = dtFinal,
                DtLogOperacao = DateTime.Now,
                FlOperacao = true,
                IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoSolicitaListaIntimacoesAguardandoCiencia:id"),
                IdTipoRetorno = 1
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
                        TComunicacaoEletronica comunicacaoEletronica = new TComunicacaoEletronica()
                        {
                            DtDisponibilizacao = citacao.dtDisponibilizacao,
                            CdAto = (int)citacao.cdAto,
                            TpIntimacaoCitacao = "C",
                            CdAssunto = citacao.Assunto != null ? Int32.Parse(citacao.Assunto.cdAssunto.ToString()) : 9999,
                            NuProcesso = Util.OnlyNumbers(citacao.nuProcesso),
                            CdForo = (int)citacao.ForoVara.cdForo,
                            CdVara = (int)citacao.ForoVara.cdVara,
                            DtLimiteCiencia = citacao.dtDisponibilizacao.AddDays(10)
                        };

                        _dataContext.TComunicacaoEletronica.Add(comunicacaoEletronica);
                        _dataContext.SaveChanges();
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
                IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoSolicitaListaCitacoesAguardandoCiencia:id"),
                IdTipoRetorno = 1
            };
            //REGISTRA O LOG
            _logOperacao.RegistrarLogOperacao(operacao);
        }
        #endregion

        #region ObterDocTeorAto
        public string ObterDocTeorAto()
        {
            //OBTÉM TODOS OS ATOS QUE FORAM INCLUÍDOS NA BASE E QUE ESTEJAM COM A DATA DE CIENCIA NULA E COM O DSATO VAZIO (DIFERENCIAL)
            var intimacaoCitacaoDocAtos = _dataContext.TComunicacaoEletronica.Where(atos => atos.DtCiencia == null && atos.DsTeorAto == null).ToList();

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

                                //COM A INFORMAÇÃO DO ATO SERÁ ATUALIZADO O REGISTRO NA BASE COM O CAMINHO DO ARQUIVO.                                
                                intimacaoCitacao.DsTeorAto = teorAto;
                                intimacaoCitacao.NuDiasPrazo = Int32.Parse(prazo);
                                intimacaoCitacao.DsCaminhoDocumentosAnexoAtoDisponibilizado = this.SalvarArquivoTeorAto(ref docAnexoAto, intimacaoCitacao.IdComunicacaoEletronica);

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
        private string SalvarArquivoTeorAto(ref string TeorAto, int idComunicacaoEletronica)
        {
            var config = ConfigurationManager.ConfigurationManager.AppSettings;
            
            var caminho = config["Diretorios:DsCaminhoTeorAto"];            
            var pathDirectorySeparator = Path.DirectorySeparatorChar;

            if (!Directory.Exists(caminho + pathDirectorySeparator + idComunicacaoEletronica))
            {
                Directory.CreateDirectory(caminho + pathDirectorySeparator + idComunicacaoEletronica);
            }

            var caminhoRetorno = idComunicacaoEletronica + pathDirectorySeparator + "DocumentoAnexoAto.zip";

            var caminhoTotal = caminho + pathDirectorySeparator + caminhoRetorno;

            File.WriteAllBytes(caminhoTotal, Convert.FromBase64String(TeorAto));

            return caminhoRetorno;
        }
        #endregion

        #region ObterIntimacaoCitacao
        public List<tipoAvisoComunicacaoPendente> ObterIntimacaoCitacao()
        {
            //APÓS OBTER OS DADOS E INSERIR NA TABELA REALIZA O PARSER PARA DEVEOLVER NO FORMATO DO PJE(MNI)
            var intimacacaoCitacaoPendentesCiencia = _dataContext.TComunicacaoEletronica.Where(atos => atos.DtCiencia == null).ToList();
            List<tipoAvisoComunicacaoPendente> listaTipoAvisos = new List<tipoAvisoComunicacaoPendente>();

            if (intimacacaoCitacaoPendentesCiencia != null && intimacacaoCitacaoPendentesCiencia.Count > 0)
            {
                foreach (var intimacaoCitacao in intimacacaoCitacaoPendentesCiencia)
                {
                    listaTipoAvisos.Add(new tipoAvisoComunicacaoPendente
                    {
                        dataDisponibilizacao = intimacaoCitacao.DtDisponibilizacao.ToString("yyyymmddhhmmss"),
                        idAviso = intimacaoCitacao.CdAto.ToString(),
                        tipoComunicacao = intimacaoCitacao.TpIntimacaoCitacao == "I" ? "INT" : "CIT",
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
                                nomeOrgao = ""//intimacaoCitacao.nmVara
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

            var consultaAvisoPendenteResposta = new consultarAvisosPendentesResponse
            {
                mensagem = $"Avisos de comunicação processual consultados com sucesso!",
                sucesso = true,
                aviso = this.ObterIntimacaoCitacao().ToArray()
            };            

            TLogOperacao operacaoConsultarAvisoPendente = new TLogOperacao()
            {
                CdIdea = consultarAvisosPendentes.idConsultante,
                DsCaminhoDocumentosChamada = Util.Serializar(consultarAvisosPendentes),
                DsCaminhoDocumentosRetorno = Util.Serializar(consultaAvisoPendenteResposta),
                DsLogOperacao = "Consultar Aviso Pendentes ",
                DtInicioOperacao = dtInicial,
                DtFinalOperacao = DateTime.Now,
                DtLogOperacao = DateTime.Now,
                IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultarAvisoPendentes:id"),
                IdTipoRetorno = 1
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

    }
}
