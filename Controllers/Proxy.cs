using System;
using System.ServiceModel;
using Core.Api.ConfigurationManager;
using Core.Api.Entidades;
using coreteste.Entidades;
using coreteste.Models;
using IntegracaoTJBA;
using Microsoft.Extensions.Configuration;

namespace coreteste.Controllers
{
    public class Proxy
    {
        private readonly ServicoPJ2PortType objProxy;
        private readonly string repositorio;
        private readonly string certificado;

        public IConfiguration Configuration { get; }

        public Proxy()
        {
            this.Configuration = ConfigurationManager.AppSettings;            
            if (this.objProxy == null){
                this.objProxy = new IntegracaoTJBA.ServicoPJ2PortTypeClient(
                new BasicHttpBinding()
                {
                    Name = "ServicoPJ2HttpBinding",
                    AllowCookies = true,
                    TextEncoding = System.Text.Encoding.UTF8,
                    TransferMode = System.ServiceModel.TransferMode.Buffered,
                    UseDefaultWebProxy = true,
                    MaxReceivedMessageSize = 2147483647,
                    MaxBufferSize = 2147483647
                },
                new EndpointAddress(Configuration.GetValue<string>("ESAJ:UrlTJ"))
            );
           }            
            //this.repositorio = Configuration.GetValue<string>("Certificado:RepositorioCertificado");
            //this.certificado = Configuration.GetValue<string>("Certificado:ThumberPrint");
            this.repositorio = ConfigurationManager.AppSettings["Certificado:RepositorioCertificado"];
            this.certificado = ConfigurationManager.AppSettings["Certificado:ThumberPrint"];
           
        }

        public string Login(Mensagem objmensagem)
        {

            Entidades.SolicitaLogonRetorno.Message objSolicitaLoginRetorno = SolicitaLogon(objmensagem);

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

            return objProxy.confirmaLogonAsync(mensagem).Result;

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

        private string Serializar(string objeto)
        {
            Message objAjuizamento = new Message();
            objAjuizamento = objAjuizamento.ExtrairObjeto<Message>(objeto);
            string str = objAjuizamento.Serialize();

            IXml objXML = new Xml();
            return  objXML.AssinarXmlString(str, repositorio, certificado, "");
        }
    }
}