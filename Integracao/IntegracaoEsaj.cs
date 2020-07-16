using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Api.Entidades;

namespace Core.Api.Integracao
{
    public class IntegracaoEsaj
    {
        public Proxy ObjProxy { get; }
        public IntegracaoEsaj(Proxy proxy) 
        {
            this.ObjProxy = proxy;
        }

        public Entidades.ConsultaProcessoResposta.Message ObterDadosProcesso(string numeroProcesso,out string retorno)
        {
            Entidades.ConsultaProcessoResposta.Message objDadosProcessoRetorno = null;
            string xmlDadosProcessoRetorno = String.Empty;

            try
            {
                Entidades.ConsultaProcesso.Message Message = new Entidades.ConsultaProcesso.Message();
                Entidades.ConsultaProcesso.MessageMessageId MessageMessageId = new Entidades.ConsultaProcesso.MessageMessageId();
                Entidades.ConsultaProcesso.MessageMessageBody MessageMessageBody = new Entidades.ConsultaProcesso.MessageMessageBody();

                MessageMessageId.Code = "201220001662";
                MessageMessageId.Date = DateTime.Now.ToString("yyyy-MM-dd"); ;
                MessageMessageId.FromAddress = "PGMS";
                MessageMessageId.ToAddress = "TJ";
                MessageMessageId.Version = Entidades.ConsultaProcesso.VersionType.Item10;
                MessageMessageId.MsgDesc = "Consulta Processo";
                MessageMessageId.ServiceId = Entidades.ConsultaProcesso.ServiceIdType.ConsultaProcesso;
                Message.MessageId = MessageMessageId;

                Entidades.ConsultaProcesso.MessageMessageBodyProcesso MessageBodyProcesso = new Entidades.ConsultaProcesso.MessageMessageBodyProcesso();
                MessageBodyProcesso.Numero = numeroProcesso;
                MessageMessageBody.Processo = MessageBodyProcesso;
                Message.MessageBody = MessageMessageBody;

                string xml = Message.Serialize();
                
                xmlDadosProcessoRetorno = this.ObjProxy.ObterDadosProcesso(xml, "201220001662", out retorno);
                objDadosProcessoRetorno = new Entidades.ConsultaProcessoResposta.Message();
                objDadosProcessoRetorno = objDadosProcessoRetorno.ExtrairObjeto<Entidades.ConsultaProcessoResposta.Message>(xmlDadosProcessoRetorno);
            }
            catch (Exception ex)
            {
                //Não envia e-mail caso a string seja diferente de vazio!
                if (xmlDadosProcessoRetorno != String.Empty)
                {
                    //INSERE LOG CONTENDO ERRO.
                    //this.EnviarEmailErroConsultarDadosDoProcesso(new StringBuilder().Append("<![CDATA[ " + xmlDadosProcessoRetorno + "]]>").Append(" <br /> Erro na comunicação com o e-SAJ TJ-BA : " + ex.Message));
                }
                throw new Exception(" Erro na comunicação com o e-SAJ TJ-BA : " + ex.Message);
            }

            return objDadosProcessoRetorno;
        }
    }
}
