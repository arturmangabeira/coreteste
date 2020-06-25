using System;
using System.ServiceModel;
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

        public Proxy(IConfiguration configuration)
        {
            if(this.objProxy == null){
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
                new EndpointAddress(configuration.GetValue<string>("ESAJ:UrlTJ"))
            );
           }
           this.repositorio = configuration.GetValue<string>("Certificado:RepositorioCertificado");
           this.certificado = configuration.GetValue<string>("Certificado:ThumberPrint");
            Configuration = configuration;
        }

        public string Login(IMensagem objmensagem)
        {

            Entidades.SolicitaLogonRetorno.Message objSolicitaLoginRetorno = SolicitaLogon(objmensagem);

            return ConfirmaLogon(objSolicitaLoginRetorno);

        }

        private string ConfirmaLogon(Entidades.SolicitaLogonRetorno.Message objSolicitaLoginRetorno)
        {
            IXml objXML = new Xml(this.Configuration);

            Entidades.ConfirmaLogon.Message objConfirmaLogon = new Entidades.ConfirmaLogon.Message();
            objConfirmaLogon.MessageId = objSolicitaLoginRetorno.MessageId;
            objConfirmaLogon.MessageId.ServiceId = ServiceAjuizamentoIdType.ConfirmaLogon;
            objConfirmaLogon.MessageId.MsgDesc = "Confirmação do Desafio de Login";
            objConfirmaLogon.MessageId.Date = DateTime.Now.ToString("yyyy-MM-dd");

            Entidades.ConfirmaLogon.MessageMessageBody objMensagem = new Entidades.ConfirmaLogon.MessageMessageBody();
            objMensagem.DesafioAssinado = objXML.AssinarDados(objSolicitaLoginRetorno.MessageBody.Resposta.Desafio, this.repositorio, this.certificado, "Desafio");

            objConfirmaLogon.MessageBody = objMensagem;

            string mensagem = objXML.AssinarXmlString(objConfirmaLogon.Serialize(), this.repositorio, this.certificado, "");

            return objProxy.confirmaLogonAsync(mensagem).Result;

        }

        public Entidades.SolicitaLogonRetorno.Message SolicitaLogon(IMensagem objmensagem)
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

            IXml objXML = new Xml(this.Configuration);
          
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

        private bool Autenticar(string codigo,out string retorno)
        {
            Mensagem objMensagem = new Mensagem();

            objMensagem.Codigo = codigo;
            objMensagem.Data = DateTime.Now.ToString("yyyy-MM-dd");
            objMensagem.Destino = "TJ";
            objMensagem.Origem = "PGMS";
            objMensagem.Servico = "SolicitaLogon";
            objMensagem.MsgDesc = "Solicitação do Desafio de Logon";

            retorno = Login(objMensagem);

            Entidades.SolicitaLogonRetorno.Message objConfirmaLogon = new Entidades.SolicitaLogonRetorno.Message();
            objConfirmaLogon = objConfirmaLogon.ExtrairObjeto<Entidades.SolicitaLogonRetorno.Message>(retorno);

            if (objConfirmaLogon.MessageBody.Resposta.Mensagem.Codigo.Equals("0"))
            {
                return true;
            }
            return false;
        }

        private string Serializar(string objeto)
        {
            Message objAjuizamento = new Message();
            objAjuizamento = objAjuizamento.ExtrairObjeto<Message>(objeto);
            string str = objAjuizamento.Serialize();

            IXml objXML = new Xml(this.Configuration);
            return  objXML.AssinarXmlString(str, repositorio, certificado, "");
        }
    }
}