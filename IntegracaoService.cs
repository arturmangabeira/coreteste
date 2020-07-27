﻿using Core.Api.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using Core.Api.Models;
using Core.Api.Integracao;
using Microsoft.Extensions.Logging;
using Core.Api.Objects;
using System;
using Core.Api.Entidades;
using Core.Api.Entidades.ForoClasse;
using Core.Api.Entidades.TpParteClasse;
using Core.Api.Entidades.DocDigitalClasse;
using Core.Api.Entidades.CategoriaClasse;
using Core.Api.Entidades.TipoDiversasClasse;
using Core.Api.Entidades.AssuntoClasse;

namespace Core.Api
{
    public class IntegracaoService: IIntegracaoService
    {
        private readonly IConfiguration configuration;
        public DataContext DataContext { get; }
        public System.IServiceProvider ServiceProvider { get; }

        private readonly ILogger<IntegracaoService> _logger;

        public Proxy ObjProxy { get; }

        public IntegracaoEsaj _integracaoEsaj { get; }

        public IntegracaoService(DataContext dataContext, IConfiguration config, ILogger<IntegracaoService> logger)
        {
            this.configuration = config;
            this.DataContext = dataContext;
            _logger = logger;
            this.ObjProxy = new Proxy(dataContext, _logger);
            this._integracaoEsaj = new IntegracaoEsaj(this.ObjProxy, this.DataContext, _logger);
        }

        public string AutenticarESAJ()
        {
            _logger.LogInformation("Iniciando a AutenticarESAJ.");
            try
            {
                if (ObjProxy.Autenticar("1", out string strLogin))
                {
                    return "Autenticação realizado com sucesso!";
                }
                else
                {
                    _logger.LogInformation("Iniciando a AutenticarESAJ. ERRO");
                    return $"Não foi possível autenticar no ESAJ. Erro: {strLogin}";
                }
            }catch(Exception ex)
            {
                _logger.LogInformation("Iniciando a AutenticarESAJ. ERRO");
                return $"Não foi possível autenticar no ESAJ. Erro: {ex.Message}";
            }
        }
               
        public List<TipoOperacao> ObterTipoOperacaoBD()
        {
            _logger.LogInformation("Iniciando ObterTipoOperacaoBD.");
            var collection = this.DataContext.TTipoOperacao.ToList();
            List<TipoOperacao> operacaos = new List<TipoOperacao>();
            foreach (var item in collection)
            {
                operacaos.Add(new TipoOperacao()
                {
                    DtCadastro = item.DtCadastro,
                    IdTipoOperacao = item.IdTipoOperacao,
                    NmTipoOperacao = item.NmTipoOperacao
                });
            }

            return operacaos;
        }

        public consultarProcessoResponse consultarProcesso(ConsultarProcesso consultarProcesso)
        {
            _logger.LogInformation("Iniciando consultarProcesso.");
            return _integracaoEsaj.ConsultarProcesso(consultarProcesso);
        }

        public Foros getForosEVaras()
        {
            _logger.LogInformation("Iniciando getForosEVaras.");
            return _integracaoEsaj.getForosEVaras(); 
        }

        public Classes getClasseTpParte()
        {
            _logger.LogInformation("Iniciando getClasseTpParte.");
            return _integracaoEsaj.getClasseTpParte();
        }

        public Documentos getTiposDocDigital()
        {
            _logger.LogInformation("Iniciando getTiposDocDigital.");
            return _integracaoEsaj.getTiposDocDigital();
        }

        public Categorias getCategoriasEClasses()
        {
            _logger.LogInformation("Iniciando getCategoriasEClasses.");
            return _integracaoEsaj.getCategoriasEClasses();
        }

        public Tipos getTiposDiversas()
        {
            _logger.LogInformation("Iniciando getTiposDiversas.");
            return _integracaoEsaj.getTiposDiversas();
        }

        public string getAreasCompetenciasEClasses(int cdForo)
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

        public Assuntos getAssuntos(int cdCompetencia, int cdClasse)
        {
            _logger.LogInformation("Iniciando getAssuntos.");
            return _integracaoEsaj.getAssuntos(cdCompetencia, cdClasse);
        }

        public List<FilaPastaDigital> consultarSituacaoDocumentosProcesso(int Cdidea, string numeroProcesso)
        {
            _logger.LogInformation("Iniciando consultarSituacaoDocumentosProcesso.");
            return _integracaoEsaj.consultarSituacaoDocumentosProcesso(Cdidea, numeroProcesso);
        }

        public consultarAvisosPendentesResponse consultarAvisosPendentes(ConsultarAvisosPendentes consultarAvisosPendentes)
        {
            _logger.LogInformation("Iniciando consultarAvisosPendentes.");
            return _integracaoEsaj.consultarAvisosPendentes(consultarAvisosPendentes);
        }
    }
}
