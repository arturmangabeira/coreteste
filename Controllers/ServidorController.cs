using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntegradorIdea.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IntegradorIdea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ServidorController : ControllerBase
    {
        public IHostApplicationLifetime _appLifetime { get; set; }
        private readonly IConfiguration _configuration;
        public ServidorController(IConfiguration config, IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _configuration = config;
        }

        [HttpPost("PararServicoIntegracao")]
        public string PararServicoIntegracao(Seguranca seguranca)
        {
            string retorno = "";
            try
            {
                string dsLoginServico = _configuration["Configuracoes:dsLoginServico"];
                string dsSenhaServico = _configuration["Configuracoes:dsSenhaServico"];

                if (seguranca.Login != dsLoginServico || seguranca.Senha != dsSenhaServico)
                {
                    throw new Exception("Login/Senha do serviço não conferem.");
                }

                _appLifetime.StopApplication();

                retorno = "Serviço parado com sucesso !";
            }
            catch (Exception ex)
            {
                retorno = $"Ocorreu um erro ao tentar para o serviço. Erro: {ex.Message}";
            }

            return retorno;
        }
    }
}
