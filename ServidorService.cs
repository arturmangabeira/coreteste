using IntegradorIdea.Integracao;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegradorIdea
{
    public class ServidorService : IServidorService
    {

        public IHostApplicationLifetime _appLifetime { get; set; }
        private readonly IConfiguration _configuration;
        public ServidorService(IHostApplicationLifetime appLifetime, IConfiguration config)
        {
            _appLifetime = appLifetime;
            _configuration = config;
        }

        string IServidorService.PararServicoIntegracao(string Login, string Senha)
        {
            string retorno = "";
            try
            {
                string dsLoginServico = _configuration["Configuracoes:dsLoginServico"];
                string dsSenhaServico = _configuration["Configuracoes:dsSenhaServico"];

                if (Login != dsLoginServico || Senha != dsSenhaServico)
                {
                    throw new Exception("Login/Senha do serviço não conferem.");
                }

                _appLifetime.StopApplication();

                retorno = "Serviço parado com sucesso !";
            }
            catch(Exception ex)
            {
                retorno = $"Ocorreu um erro ao tentar para o serviço. Erro: {ex.Message}";
            }

            return retorno;
        }
    }
}
