using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegradorIdea.Data;
using IntegradorIdea.Integracao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IntegradorIdea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntimacaoCitacaoController : ControllerBase
    {
        public Proxy _proxy { get; }
        public IConfiguration _configuration { get; }

        public ILogger<IntegracaoService> _logger;

        private DataContext _dataContext { get; }
        public Log _logOperacao { get; }
        public string _ipDestino { get; set; }

        Integracao.IntegracaoEsaj _integracaoEsaj { get; set; }

        public IntimacaoCitacaoController(DataContext dataContext, IConfiguration config, ILogger<IntegracaoService> logger, IHttpContextAccessor contexto)
        {
            _ipDestino = contexto.HttpContext.Connection.RemoteIpAddress.ToString();
            _proxy = new Proxy(dataContext, logger, _ipDestino);            
            _logger = logger;
            _configuration = config;
            _dataContext = dataContext;
            _integracaoEsaj = new Integracao.IntegracaoEsaj(_proxy, _dataContext, logger, _ipDestino);
        }

        [HttpGet("ObterListaIntimacaoCitacao")]
        public string ObterListaInimacaoCitacao()
        {
            
            //EXECUTA OS INSERTS DAS INTIMACOES CITACOES
            _integracaoEsaj.ObterIntimacaoCitacaoService();
            
            return "Concluído";
        }

        [HttpGet("ObterDocCiencia")]
        public string ObterDocCiencia()
        {            
            //EXECURTA ROTINA PARA INCLUIR O DOCUMENTO DO TEOR DO ATO
            _integracaoEsaj.ObterDocTeorAto();

            return "Concluído";
        }
    }
}
