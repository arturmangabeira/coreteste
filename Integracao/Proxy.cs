using System;
using System.ServiceModel;
using IntegradorIdea.Data;
using IntegradorIdea.Entidades;
using IntegradorIdea.Models;
using IntegracaoTJBA;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace IntegradorIdea.Integracao
{
    public class Proxy
    {
        private readonly ServicoPJ2PortType _serviceESAJ;
        private readonly string _repositorio;
        private readonly string _certificado;
        private Log _logOperacao { get; }

        private string _codigo { get; }

        public ILogger<IntegracaoService> _logger;

        //private DataContext DataContext { get; }

        public IConfiguration _configuration { get; }

        public int _cdIdeia { get; set; }

        #region Proxy
        public Proxy(DataContext dataContext, ILogger<IntegracaoService> logger, string ipDestino)
        {
            _configuration = ConfigurationManager.ConfigurationManager.AppSettings;
            _logOperacao = new Log(dataContext, ipDestino);
            _logger = logger;
            _codigo = "1";            

            if (_serviceESAJ == null)
            {
                _serviceESAJ = new ServicoPJ2PortTypeClient(
                    new BasicHttpBinding()
                    {
                        Name = _configuration.GetValue<string>("ESAJ:BindingName"),
                        AllowCookies = true,
                        TextEncoding = System.Text.Encoding.UTF8,
                        TransferMode = System.ServiceModel.TransferMode.Buffered,
                        UseDefaultWebProxy = true,
                        MaxReceivedMessageSize = _configuration.GetValue<long>("ESAJ:MaxReceivedMessageSize"),
                        MaxBufferSize = _configuration.GetValue<int>("ESAJ:MaxBufferSize")
                    },
                    new EndpointAddress(_configuration.GetValue<string>("ESAJ:UrlTJ"))
                );
            }

            _repositorio = _configuration["Certificado:RepositorioCertificado"];
            _certificado = _configuration["Certificado:ThumberPrint"];
        }
        #endregion

        #region Login
        public string Login(Mensagem objmensagem)
        {            
            Entidades.SolicitaLogonRetorno.Message objSolicitaLoginRetorno = SolicitaLogon(objmensagem);
            return ConfirmaLogon(objSolicitaLoginRetorno);

        }
        #endregion

        #region ConfirmaLogon
        private string ConfirmaLogon(Entidades.SolicitaLogonRetorno.Message objSolicitaLoginRetorno)
        {
            IXml objXML = new Xml();

            Entidades.ConfirmaLogon.Message objConfirmaLogon = new Entidades.ConfirmaLogon.Message();
            objConfirmaLogon.MessageId = objSolicitaLoginRetorno.MessageId;
            objConfirmaLogon.MessageId.ServiceId = ServiceAjuizamentoIdType.ConfirmaLogon;
            objConfirmaLogon.MessageId.MsgDesc = "Confirmação do Desafio de Login";
            objConfirmaLogon.MessageId.Date = DateTime.Now.ToString("yyyy-MM-dd");

            Entidades.ConfirmaLogon.MessageMessageBody messageBody = new Entidades.ConfirmaLogon.MessageMessageBody();
            messageBody.DesafioAssinado = objXML.AssinarDados(objSolicitaLoginRetorno.MessageBody.Resposta.Desafio, _repositorio, _certificado, "Desafio");

            objConfirmaLogon.MessageBody = messageBody;

            string mensagem = objXML.AssinarXmlString(objConfirmaLogon.Serialize(), _repositorio, _certificado, "");

            var dtInicial = DateTime.Now;
            var retorno = _serviceESAJ.confirmaLogonAsync(mensagem).Result;
            var dtFinal = DateTime.Now;
            if (_configuration.GetValue<bool>("RegistraLog:Metodos:ConfirmaLogon"))
            {
                //REGISTAR LOGON
                TLogOperacao operacao = new TLogOperacao()
                {
                    CdIdea = _cdIdeia,
                    DsCaminhoDocumentosChamada = mensagem,
                    DsCaminhoDocumentosRetorno = retorno,
                    DsLogOperacao = "ConfirmaLogon",
                    DtInicioOperacao = dtInicial,
                    DtFinalOperacao = dtFinal,
                    DtLogOperacao = DateTime.Now,
                    FlOperacao = true,
                    IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConfirmaLogon:id"),
                    IdTipoRetorno = 1
                };
                //REGISTRA O LOG
                _logOperacao.RegistrarLogOperacao(operacao);
            }
            return retorno;

        }
        #endregion

        #region SolicitaLogon
        public Entidades.SolicitaLogonRetorno.Message SolicitaLogon(Mensagem objmensagem)
        {
            Message objSolicitaLogin = new Message();
            MessageIdType objMessageId = new MessageIdType();
            MessageMessageBody objMessageBody = new MessageMessageBody();


            objMessageId.ServiceId = ServiceAjuizamentoIdType.SolicitaLogon;
            objMessageId.Code = objmensagem.Codigo;
            objMessageId.Date = objmensagem.Data;
            objMessageId.FromAddress = objmensagem.Origem;
            objMessageId.MsgDesc = objmensagem.MsgDesc;
            objMessageId.Version = VersionType.Item10;
            objMessageId.ToAddress = objmensagem.Destino;
            objSolicitaLogin.MessageId = objMessageId;
            objSolicitaLogin.MessageBody = objMessageBody;

            IXml objXML = new Xml();

            string mensagem = objXML.AssinarXmlString(objSolicitaLogin.Serialize(), _repositorio, _certificado, "");

            Entidades.SolicitaLogonRetorno.Message objRespostaSolicitaLogon = new Entidades.SolicitaLogonRetorno.Message();
            //setProxy();
            string retorno = _serviceESAJ.solicitaLogonAsync(mensagem).Result;

            var dtInicial = DateTime.Now;
            var dtFinal = DateTime.Now;
            if (_configuration.GetValue<bool>("RegistraLog:Metodos:SolicitaLogon"))
            {
                //REGISTAR LOGON
                TLogOperacao operacao = new TLogOperacao()
                {
                    CdIdea = _cdIdeia,
                    DsCaminhoDocumentosChamada = mensagem,
                    DsCaminhoDocumentosRetorno = retorno,
                    DsLogOperacao = "SolicitaLogon",
                    DtInicioOperacao = dtInicial,
                    DtFinalOperacao = dtFinal,
                    DtLogOperacao = DateTime.Now,
                    FlOperacao = true,
                    IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoSolicitaLogon:id"),
                    IdTipoRetorno = 1
                };
                //REGISTRA O LOG
                _logOperacao.RegistrarLogOperacao(operacao);
            }


            if (retorno == null)
            {
                throw new System.Exception("Não foi possível realizar o login no servidor do TJBA");
            }

            return objRespostaSolicitaLogon.ExtrairObjeto<Entidades.SolicitaLogonRetorno.Message>(retorno);


        }
        #endregion

        #region Autenticar
        public bool Autenticar(string codigo, out string retorno)
        {
            Mensagem objMensagem = new Mensagem()
            {

                Codigo = codigo,
                Data = DateTime.Now.ToString("yyyy-MM-dd"),
                Destino = "TJ",
                Origem = "MP-BA",
                Servico = "SolicitaLogon",
                MsgDesc = "Solicitação do Desafio de Logon"
            };

            retorno = Login(objMensagem);

            Entidades.SolicitaLogonRetorno.Message objConfirmaLogon = new Entidades.SolicitaLogonRetorno.Message();
            objConfirmaLogon = objConfirmaLogon.ExtrairObjeto<Entidades.SolicitaLogonRetorno.Message>(retorno);

            if (objConfirmaLogon.MessageBody.Resposta.Mensagem.Codigo.Equals("0"))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region GetTiposDocDigital
        public string GetTiposDocDigital(string codigo)
        {
            string strLogin;

            string resposta;
            if (Autenticar(codigo, out strLogin))
            {
                resposta = _serviceESAJ.getTiposDocDigitalAsync().Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region ObterDadosProcesso
        public string ObterDadosProcesso(string DadosProcesso, string codigo)
        {
            string RespostaDadosProcesso = "";

            string strLogin;

            if (Autenticar(codigo, out strLogin))
            {

                Entidades.ConsultaProcesso.Message objAjuizamento = new Entidades.ConsultaProcesso.Message();
                objAjuizamento = objAjuizamento.ExtrairObjeto<Entidades.ConsultaProcesso.Message>(DadosProcesso);
                string xmlDadosProcesso = objAjuizamento.Serialize();

                IXml objXML = new Xml();
                xmlDadosProcesso = objXML.AssinarXmlString(xmlDadosProcesso, _repositorio, _certificado, "");
                var dtInicial = DateTime.Now;
                RespostaDadosProcesso = _serviceESAJ.getDadosProcessoAsync(xmlDadosProcesso).Result;
                var dtFinal = DateTime.Now;
                //REGISTAR LOGON
                TLogOperacao operacao = new TLogOperacao()
                {
                    CdIdea = _cdIdeia,
                    DsCaminhoDocumentosChamada = xmlDadosProcesso,
                    DsCaminhoDocumentosRetorno = RespostaDadosProcesso,
                    DsLogOperacao = "Consulta do Processo no ESAJ: " + objAjuizamento.MessageBody.Processo.Numero,
                    DtInicioOperacao = dtInicial,
                    DtFinalOperacao = dtFinal,
                    DtLogOperacao = DateTime.Now,
                    FlOperacao = true,
                    IdTipoOperacao = _configuration.GetValue<int>("Operacoes:TipoOperacaoConsultaProcessoESAJ:id"),
                    IdTipoRetorno = 1
                };
                //REGISTRA O LOG
                _logOperacao.RegistrarLogOperacao(operacao);
            }
            else
            {
                throw new Exception($"Erro ao tentar obter dados do processo. Erro: {strLogin}");
            }

            return RespostaDadosProcesso;

        }
        #endregion

        #region getForosEVaras
        public string getForosEVaras()
        {
            _logger.LogInformation("Proxy iniciando getForosEVaras.");
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.getForosEVarasAsync().Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region getClasseTpParte
        public string getClasseTpParte()
        {
            _logger.LogInformation("Proxy iniciando getClasseTpParte.");
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.getClasseTpParteAsync().Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region getTiposDocDigital
        public string getTiposDocDigital()
        {
            _logger.LogInformation("Proxy iniciando getTiposDocDigital.");
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.getTiposDocDigitalAsync().Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region getCategoriasEClasses
        public string getCategoriasEClasses()
        {
            _logger.LogInformation("Proxy iniciando getCategoriasEClasses.");
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.getCategoriasEClassesAsync().Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region getTiposDiversas
        public string getTiposDiversas()
        {
            _logger.LogInformation("Proxy iniciando getTiposDiversas.");
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.getTiposDiversasAsync().Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region getAreasCompetenciasEClasses
        public string getAreasCompetenciasEClasses(int cdForo)
        {
            _logger.LogInformation("Proxy iniciando getAreasCompetenciasEClasses.");
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.getAreasCompetenciasEClassesAsync(cdForo).Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region obterNumeroUnificadoDoProcesso
        public string obterNumeroUnificadoDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("Proxy iniciando obterNumeroUnificadoDoProcesso.");
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.obterNumeroUnificadoDoProcessoAsync(numeroProcesso).Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region obterNumeroSajDoProcesso
        public string obterNumeroSajDoProcesso(string numeroProcesso)
        {
            _logger.LogInformation("Proxy iniciando obterNumeroSajDoProcesso.");
            string resposta = "";
            string strLogin;

            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.obterNumeroSajDoProcessoAsync(numeroProcesso).Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region getAssuntos
        public string getAssuntos(int cdCompetencia, int cdClasse)
        {
            _logger.LogInformation("Proxy iniciando getAssuntos.");
            string resposta = "";
            string strLogin;

            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.getAssuntosAsync(cdCompetencia, cdClasse).Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region SolicitaListaCitacoesAguardandoCiencia
        public string SolicitaListaCitacoesAguardandoCiencia(string strXMLSolicitacao)
        {
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.SolicitaListaCitacoesAguardandoCienciaAsync(strXMLSolicitacao).Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;

        }
        #endregion

        #region SolicitaListaIntimacoesAguardandoCiencia
        public string SolicitaListaIntimacoesAguardandoCiencia(string strXMLSolicitacao)
        {
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.SolicitaListaIntimacoesAguardandoCienciaAsync(strXMLSolicitacao).Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region SolicitacaoDocCienciaAto
        public string SolicitacaoDocCienciaAto(string strXMLSolicitacao)
        {
            string strLogin;

            string resposta;
            if (Autenticar(_codigo, out strLogin))
            {
                resposta = _serviceESAJ.solicitacaoDocCienciaAtoAsync(strXMLSolicitacao).Result;
            }
            else
            {
                resposta = strLogin;
            }
            return resposta;
        }
        #endregion

        #region Serializar
        private string Serializar(string objeto)
        {
            Message objAjuizamento = new Message();
            objAjuizamento = objAjuizamento.ExtrairObjeto<Message>(objeto);
            string str = objAjuizamento.Serialize();

            IXml objXML = new Xml();
            return objXML.AssinarXmlString(str, _repositorio, _certificado, "");
        }
        #endregion

    }
}
