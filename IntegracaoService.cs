using IntegradorIdea.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using IntegradorIdea.Integracao;
using Microsoft.Extensions.Logging;
using IntegradorIdea.Objects;
using IntegradorIdea.Entidades.ForoClasse;
using IntegradorIdea.Entidades.TpParteClasse;
using IntegradorIdea.Entidades.CategoriaClasse;
using IntegradorIdea.Entidades.TipoDiversasClasse;
using IntegradorIdea.Entidades.AssuntoClasse;
using Microsoft.AspNetCore.Http;
using IntegradorIdea.Objects.Response;

namespace IntegradorIdea

{

    public class IntegracaoService : IIntegracaoService
    {
        private readonly IConfiguration _configuration;
        public DataContext _dataContext { get; }
        public System.IServiceProvider _serviceProvider { get; }
        private readonly ILogger<IntegracaoService> _logger;
        public Proxy _proxy { get; }
        public IntegracaoEsaj _integracaoEsaj { get; }


        public IntegracaoService(DataContext dataContext, IConfiguration config, ILogger<IntegracaoService> logger, IHttpContextAccessor contexto)
        {
            _configuration = config;
            _dataContext = dataContext;
            _logger = logger;
            var ipDestino = contexto.HttpContext.Connection.RemoteIpAddress.ToString();
            _proxy = new Proxy(dataContext, _logger, ipDestino);
            _integracaoEsaj = new IntegracaoEsaj(_proxy, _dataContext, _logger, ipDestino);
        }

        public List<TipoOperacao> ObterTipoOperacaoBD()
        {
            _logger.LogInformation("Iniciando ObterTipoOperacaoBD.");
            var collection = _dataContext.tTpOperacao.ToList();
            List<TipoOperacao> operacaos = new List<TipoOperacao>();
            foreach (var item in collection)
            {
                operacaos.Add(new TipoOperacao()
                {
                    DtCadastro = item.DtCadastro,
                    IdTipoOperacao = item.IdTpOperacao,
                    NmTipoOperacao = item.NmTpOperacao
                });
            }

            return operacaos;
        }

        public consultarProcessoResponse consultarProcesso(int idConsultante, string numeroProcesso, bool movimentos, bool incluirCabecalho, bool incluirDocumentos, string[] documento)
        {
            _logger.LogInformation("Iniciando consultarProcesso.");

            var consultarProcesso = new ConsultarProcesso()
            {
                idConsultante = idConsultante,
                numeroProcesso = numeroProcesso,
                movimentos = movimentos,
                incluirCabecalho = incluirCabecalho,
                incluirDocumentos = incluirDocumentos,
                documento = documento
            };

            /*consultarProcessoResponse _consultarProcessoResponse = new consultarProcessoResponse()
            {
                mensagem = "Teste serviço OK!",
                sucesso = true,
                processo = new tipoProcessoJudicial()
                {
                    documento = new tipoDocumento[]
                   {
                        new tipoDocumento()
                        {
                            descricao = "teste.pdf",
                            conteudo = Util.Base64EncodeStream("teste de conteudo"),
                            tipoDocumento1 = "1"
                        }
                   }
                }
            };

            return _consultarProcessoResponse;
            */
            return _integracaoEsaj.ConsultarProcesso(consultarProcesso);
        }



        public IntegradorIdea.Objects.Response.GetForosEVarasResponse getForosEVaras()
        {
            _logger.LogInformation("Iniciando getForosEVaras.");
            return _integracaoEsaj.getForosEVaras();
        }

        public IntegradorIdea.Objects.Response.getClasseTpParteResponse getClasseTpParte()
        {
            _logger.LogInformation("Iniciando getClasseTpParte.");
            return _integracaoEsaj.getClasseTpParte();
        }

        public IntegradorIdea.Objects.Response.GetTiposDocDigitalResponse getTiposDocDigital()
        {
            _logger.LogInformation("Iniciando getTiposDocDigital.");
            return _integracaoEsaj.getTiposDocDigital();
        }

        public Objects.Response.GetCategoriasEClassesResponse getCategoriasEClasses()
        {
            _logger.LogInformation("Iniciando getCategoriasEClasses.");
            return _integracaoEsaj.getCategoriasEClasses();
        }

        public IntegradorIdea.Objects.Response.GetTiposDiversasResponse getTiposDiversas()
        {
            _logger.LogInformation("Iniciando getTiposDiversas.");
            return _integracaoEsaj.getTiposDiversas();
        }

        public Objects.Response.GetAreasCompetenciasEClassesResponse.GetAreasCompetenciasEClassesResponse getAreasCompetenciasEClasses(int cdForo)
        {
            _logger.LogInformation("Iniciando getAreasCompetenciasEClasses.");
            return _integracaoEsaj.getAreasCompetenciasEClasses(cdForo);
        }

        public string obterNumeroUnificadoDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("Iniciando obterNumeroUnificadoDoProcesso.");
            return _integracaoEsaj.obterNumeroUnificadoDoProcesso(numeroProcesso);
        }



        public string obterNumeroSajDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("Iniciando obterNumeroSajDoProcesso.");
            return _integracaoEsaj.obterNumeroSajDoProcesso(numeroProcesso);
        }

        public IntegradorIdea.Objects.Response.GetAssuntosResponse getAssuntos(int cdCompetencia, int cdClasse)
        {
            _logger.LogInformation("Iniciando getAssuntos.");
            return _integracaoEsaj.getAssuntos(cdCompetencia, cdClasse);
        }



        public List<FilaPastaDigital> consultarSituacaoDocumentosProcesso(int Cdidea, string numeroProcesso)
        {
            _logger.LogInformation("Iniciando consultarSituacaoDocumentosProcesso.");
            return _integracaoEsaj.consultarSituacaoDocumentosProcesso(Cdidea, numeroProcesso);
        }



        public consultarAvisosPendentesResponse consultarAvisosPendentes(string idRepresentado, int idConsultante, string senhaConsultante, string dataReferencia)
        {
            _logger.LogInformation("Iniciando consultarAvisosPendentes.");
            var consultarAvisosPendentes = new ConsultarAvisosPendentes()
            {
                idRepresentado = idRepresentado,
                idConsultante = idConsultante,
                senhaConsultante = senhaConsultante,
                dataReferencia = dataReferencia
            };

            return _integracaoEsaj.consultarAvisosPendentes(consultarAvisosPendentes);
        }



        public consultarTeorComunicacaoResponse consultarTeorComunicacao(string idConsultante, string senhaConsultante, string numeroProcesso, string identificadorAviso)
        {
            var consultarTeorComunicacao = new consultarTeorComunicacaoRequest()
            {
                idConsultante = idConsultante,
                senhaConsultante = senhaConsultante,
                numeroProcesso = numeroProcesso,
                identificadorAviso = identificadorAviso
            };

            _logger.LogInformation("Iniciando consultarTeorComunicacao.");
            return _integracaoEsaj.consultarTeorComunicacao(consultarTeorComunicacao);

        }

        public entregarManifestacaoProcessualResponse entregarManifestacaoProcessual(string idManifestante, string senhaManifestante, string numeroProcesso, tipoCabecalhoProcesso dadosBasicos, Documento documento, string dataEnvio, tipoParametro[] parametros)
        {
            var _entregarManifestacaoProcessual = new entregarManifestacaoProcessualRequest()
            {
                dadosBasicos = dadosBasicos,
                dataEnvio = dataEnvio,
                documento = documento,
                idManifestante = idManifestante,
                numeroProcesso = numeroProcesso,
                parametros = parametros,
                senhaManifestante = senhaManifestante
            };

            _logger.LogInformation("Iniciando entregarManifestacaoProcessual.");
            return _integracaoEsaj.entregarManifestacaoProcessual(_entregarManifestacaoProcessual);

        }

        public string testeServico()
        {
            return "Servico OK!";
        }

        public Objects.ConsultarProcessoESAJ.consultarProcessoResposta consultarProcesso02(int idConsultante, string numeroProcesso, bool movimentos, bool incluirCabecalho, bool incluirDocumentos, string[] documento)
        {
            _logger.LogInformation("Iniciando consultarProcesso.");

            var consultarProcesso = new ConsultarProcesso()
            {
                idConsultante = idConsultante,
                numeroProcesso = numeroProcesso,
                movimentos = movimentos,
                incluirCabecalho = incluirCabecalho,
                incluirDocumentos = incluirDocumentos,
                documento = documento
            };

            Objects.ConsultarProcessoESAJ.consultarProcessoResposta _consultarProcessoResponse = new Objects.ConsultarProcessoESAJ.consultarProcessoResposta()
            {
                mensagem = "Teste serviço OK!",
                sucesso = true,
                processo = new Objects.ConsultarProcessoESAJ.tipoProcessoJudicialESAJ()
                {
                    documento = new Documento[]
                    {
                        new Documento()
                        {
                            descricao = "teste.pdf",
                            conteudo = Util.Base64EncodeStream("teste de conteudo"),
                            tipoDocumento = "8000"
                        }
                    }
                }
            };

            return _consultarProcessoResponse;

            //return _integracaoEsaj.ConsultarProcesso(consultarProcesso);

        }

    }

}

