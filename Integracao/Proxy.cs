using System;
using System.ServiceModel;
using Core.Api.Data;
using Core.Api.Entidades;
using Core.Api.Models;
using IntegracaoTJBA;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;


namespace Core.Api.Integracao
{
    public class Proxy
    {
        private readonly ServicoPJ2PortType objProxy;
        private readonly string repositorio;
        private readonly string certificado;
        private Log logOperacao { get; }

        private DataContext DataContext { get; }

        public IConfiguration Configuration { get; }

        public string CdIdeia { get; set; }

        public Proxy(DataContext dataContext)
        {
            this.Configuration = ConfigurationManager.ConfigurationManager.AppSettings;
            this.logOperacao = new Log(dataContext);

            if (this.objProxy == null)
            {
                this.objProxy = new ServicoPJ2PortTypeClient(
                    new BasicHttpBinding()
                    {
                        Name = Configuration.GetValue<string>("ESAJ:BindingName"),
                        AllowCookies = true,
                        TextEncoding = System.Text.Encoding.UTF8,
                        TransferMode = System.ServiceModel.TransferMode.Buffered,
                        UseDefaultWebProxy = true,
                        MaxReceivedMessageSize = Configuration.GetValue<long>("ESAJ:MaxReceivedMessageSize"),
                        MaxBufferSize = Configuration.GetValue<int>("ESAJ:MaxBufferSize")
                    },
                    new EndpointAddress(Configuration.GetValue<string>("ESAJ:UrlTJ"))
                );
            }
            
            this.repositorio = this.Configuration["Certificado:RepositorioCertificado"];
            this.certificado = this.Configuration["Certificado:ThumberPrint"];
        }

        public string Login(Mensagem objmensagem)
        {
            var dtInicial = DateTime.Now;
            Entidades.SolicitaLogonRetorno.Message objSolicitaLoginRetorno = SolicitaLogon(objmensagem);
            var dtFinal = DateTime.Now;
            //REGISTAR LOGON
            TLogOperacao operacao = new TLogOperacao()
            {
                CdIdea = this.CdIdeia,
                DsCaminhoDocumentosChamada = objmensagem.Serialize(),
                DsCaminhoDocumentosRetorno = objSolicitaLoginRetorno.Serialize(),
                DsIpdestino = "192.168.0.1",
                DsIporigem = "192.168.0.1",
                DsLogOperacao = "Login",
                DtInicioOperacao = dtInicial,
                DtFinalOperacao = dtFinal,
                DtLogOperacao = DateTime.Now,
                FlOperacao = true,
                IdTipoOperacao = this.Configuration.GetValue<int>("Constantes:IdTipoOperacaoConsultaProcesso"),
                IdTipoRetorno = 1
            };
            //REGISTRA O LOG
            this.logOperacao.RegistrarLogOperacao(operacao);
            return ConfirmaLogon(objSolicitaLoginRetorno);

        }

        private string ConfirmaLogon(Entidades.SolicitaLogonRetorno.Message objSolicitaLoginRetorno)
        {
            IXml objXML = new Xml();

            Entidades.ConfirmaLogon.Message objConfirmaLogon = new Entidades.ConfirmaLogon.Message();
            objConfirmaLogon.MessageId = objSolicitaLoginRetorno.MessageId;
            objConfirmaLogon.MessageId.ServiceId = ServiceAjuizamentoIdType.ConfirmaLogon;
            objConfirmaLogon.MessageId.MsgDesc = "Confirmação do Desafio de Login";
            objConfirmaLogon.MessageId.Date = DateTime.Now.ToString("yyyy-MM-dd");

            Entidades.ConfirmaLogon.MessageMessageBody messageBody = new Entidades.ConfirmaLogon.MessageMessageBody();
            messageBody.DesafioAssinado = objXML.AssinarDados(objSolicitaLoginRetorno.MessageBody.Resposta.Desafio, this.repositorio, this.certificado, "Desafio");

            objConfirmaLogon.MessageBody = messageBody;

            string mensagem = objXML.AssinarXmlString(objConfirmaLogon.Serialize(), this.repositorio, this.certificado, "");

            var dtInicial = DateTime.Now;                        
            var retorno = objProxy.confirmaLogonAsync(mensagem).Result;
            var dtFinal = DateTime.Now;
            //REGISTAR LOGON
            TLogOperacao operacao = new TLogOperacao()
            {
                CdIdea = this.CdIdeia,
                DsCaminhoDocumentosChamada = mensagem,
                DsCaminhoDocumentosRetorno = retorno,
                DsIpdestino = "192.168.0.1",
                DsIporigem = "192.168.0.1",
                DsLogOperacao = "ConfirmaLogon",
                DtInicioOperacao = dtInicial,
                DtFinalOperacao = dtFinal,
                DtLogOperacao = DateTime.Now,
                FlOperacao = true,
                IdTipoOperacao = this.Configuration.GetValue<int>("Constantes:IdTipoOperacaoConsultaProcesso"),
                IdTipoRetorno = 1
            };
            //REGISTRA O LOG
            this.logOperacao.RegistrarLogOperacao(operacao);

            return retorno;

        }

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

            string mensagem = objXML.AssinarXmlString(objSolicitaLogin.Serialize(), repositorio, certificado, "");

            Entidades.SolicitaLogonRetorno.Message objRespostaSolicitaLogon = new Entidades.SolicitaLogonRetorno.Message();
            //setProxy();
            string retorno = objProxy.solicitaLogonAsync(mensagem).Result;

            if (retorno == null)
            {
                throw new System.Exception("Não foi possível realizar o login no servidor do TJBA");
            }

            return objRespostaSolicitaLogon.ExtrairObjeto<Entidades.SolicitaLogonRetorno.Message>(retorno);


        }

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

        public string GetTiposDocDigital(string codigo)
        {
            string RespostaGetTiposDocDigital = "";
            string strLogin;

            if (Autenticar(codigo, out strLogin))
            {
                RespostaGetTiposDocDigital = objProxy.getTiposDocDigitalAsync().Result;
            }
            else
            {
                RespostaGetTiposDocDigital = strLogin;
            }
            return RespostaGetTiposDocDigital;
        }

        public string ObterDadosProcesso(string DadosProcesso, string codigo)
        {
            string RespostaDadosProcesso = "";
            
            string strLogin;

            if (Autenticar(codigo, out strLogin))
            {

                Entidades.ConsultaProcesso.Message objAjuizamento = new Entidades.ConsultaProcesso.Message();
                objAjuizamento = objAjuizamento.ExtrairObjeto<Entidades.ConsultaProcesso.Message>(DadosProcesso);
                string str = objAjuizamento.Serialize();

                IXml objXML = new Xml();
                str = objXML.AssinarXmlString(str, repositorio, certificado, "");
                RespostaDadosProcesso = objProxy.getDadosProcessoAsync(str).Result;
            }
            else
            {   
                throw new Exception($"Erro ao tentar obter autenticar. Erro: {strLogin}");
            }

            return RespostaDadosProcesso;

        }

        private string Serializar(string objeto)
        {
            Message objAjuizamento = new Message();
            objAjuizamento = objAjuizamento.ExtrairObjeto<Message>(objeto);
            string str = objAjuizamento.Serialize();

            IXml objXML = new Xml();
            return objXML.AssinarXmlString(str, repositorio, certificado, "");
        }
    }
}
