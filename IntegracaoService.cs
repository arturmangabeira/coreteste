using Core.Api.Data;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using Core.Api.Models;
using Core.Api.Integracao;
using Microsoft.Extensions.Logging;
using Core.Api.Objects;

namespace Core.Api
{
    public class IntegracaoService: IIntegracaoService
    {
        private readonly IConfiguration configuration;
        public DataContext DataContext { get; }
        public System.IServiceProvider ServiceProvider { get; }

        private readonly ILogger<IntegracaoService> _logger;

        public Proxy ObjProxy { get; }

        public IntegracaoEsaj integracaoEsaj { get; }

        public IntegracaoService(DataContext dataContext, IConfiguration config, ILogger<IntegracaoService> logger)
        {
            this.configuration = config;
            this.DataContext = dataContext;
            _logger = logger;
            this.ObjProxy = new Proxy(dataContext);
            this.integracaoEsaj = new IntegracaoEsaj(this.ObjProxy, this.DataContext);
        }

        public string AutenticarESAJ()
        {
            if (ObjProxy.Autenticar("1", out string strLogin))
            {
                return "Autenticação realizado com sucesso!";
            }
            else
            {
                return $"Não foi possível autenticar no ESAJ. Erro: {strLogin}";
            }
        }

        public string GetTiposDocDigitalXML(string codigo)
        {
            return "<![CDATA[" + ObjProxy.GetTiposDocDigital(codigo) + "]]>";
        }

        public List<TTipoOperacao> ObterTipoOperacaoBD()
        {
            var retorno = this.DataContext.TTipoOperacao.ToList();

            return retorno;
        }

        public consultarProcessoResponse ObterDadosProcesso(ConsultarProcesso consultarProcesso)
        {            
            return integracaoEsaj.ObterDadosProcesso(consultarProcesso);
        }
    }
}
