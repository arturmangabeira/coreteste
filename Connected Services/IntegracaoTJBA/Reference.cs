﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntegracaoTJBA
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="ServicoPJ2", ConfigurationName="IntegracaoTJBA.ServicoPJ2PortType")]
    public interface ServicoPJ2PortType
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> solicitaLogonAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> confirmaLogonAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoCitacaoAutoConfirmadaAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getClasseTpParteAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoIntimacaoAtoAsync(string in0, string in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> peticionarIntermediariaAsync(string in0, string in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> ConsultaProcessoSGCRAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getComarcasAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> ConsultaProcessoSGAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> obterDocumentoAnexoAoAtoAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getDadosProcessoAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> obterNumeroUnificadoDoProcessoAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitaLogonAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoQtdIntimacoesAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getCategoriasEClassesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> peticionarIntermediariaDiversaAsync(string in0, string in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getTodosAssuntosAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<bool> logonAsync(string in0, string in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoDadosDistribuicaoAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> solicitacaoDocCienciaAtoAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getTiposDocDigitalAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitaListaIntimacoesAguardandoCienciaAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getAssuntosAsync(System.Nullable<int> in0, System.Nullable<int> in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getTiposDiversasAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitaListaCitacoesAguardandoCienciaAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoCitacaoAtoAsync(string in0, string in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoIntimacaoCienciaAsync(string in0, string in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> ConsultaPeticaoAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getAreasCompetenciasClassesEAssuntosAsync(System.Nullable<int> in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getAreasCompetenciasEClassesAsync(System.Nullable<int> in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> getForosEVarasAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoIntimacaoAutoConfirmadaAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> obterNumeroSajDoProcessoAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> SolicitacaoCitacaoCienciaAsync(string in0, string in1);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> ConfirmaLogonAsync(string in0);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        [return: System.ServiceModel.MessageParameterAttribute(Name="out")]
        System.Threading.Tasks.Task<string> ajuizarAsync(string in0, string in1);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface ServicoPJ2PortTypeChannel : IntegracaoTJBA.ServicoPJ2PortType, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class ServicoPJ2PortTypeClient : System.ServiceModel.ClientBase<IntegracaoTJBA.ServicoPJ2PortType>, IntegracaoTJBA.ServicoPJ2PortType
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ServicoPJ2PortTypeClient() : 
                base(ServicoPJ2PortTypeClient.GetDefaultBinding(), ServicoPJ2PortTypeClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.ServicoPJ2HttpPort.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServicoPJ2PortTypeClient(EndpointConfiguration endpointConfiguration) : 
                base(ServicoPJ2PortTypeClient.GetBindingForEndpoint(endpointConfiguration), ServicoPJ2PortTypeClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServicoPJ2PortTypeClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ServicoPJ2PortTypeClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServicoPJ2PortTypeClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ServicoPJ2PortTypeClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ServicoPJ2PortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<string> solicitaLogonAsync(string in0)
        {
            return base.Channel.solicitaLogonAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> confirmaLogonAsync(string in0)
        {
            return base.Channel.confirmaLogonAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoCitacaoAutoConfirmadaAsync(string in0)
        {
            return base.Channel.SolicitacaoCitacaoAutoConfirmadaAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getClasseTpParteAsync()
        {
            return base.Channel.getClasseTpParteAsync();
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoIntimacaoAtoAsync(string in0, string in1)
        {
            return base.Channel.SolicitacaoIntimacaoAtoAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> peticionarIntermediariaAsync(string in0, string in1)
        {
            return base.Channel.peticionarIntermediariaAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> ConsultaProcessoSGCRAsync(string in0)
        {
            return base.Channel.ConsultaProcessoSGCRAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getComarcasAsync()
        {
            return base.Channel.getComarcasAsync();
        }
        
        public System.Threading.Tasks.Task<string> ConsultaProcessoSGAsync(string in0)
        {
            return base.Channel.ConsultaProcessoSGAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> obterDocumentoAnexoAoAtoAsync(string in0)
        {
            return base.Channel.obterDocumentoAnexoAoAtoAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getDadosProcessoAsync(string in0)
        {
            return base.Channel.getDadosProcessoAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> obterNumeroUnificadoDoProcessoAsync(string in0)
        {
            return base.Channel.obterNumeroUnificadoDoProcessoAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> SolicitaLogonAsync(string in0)
        {
            return base.Channel.SolicitaLogonAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoQtdIntimacoesAsync(string in0)
        {
            return base.Channel.SolicitacaoQtdIntimacoesAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getCategoriasEClassesAsync()
        {
            return base.Channel.getCategoriasEClassesAsync();
        }
        
        public System.Threading.Tasks.Task<string> peticionarIntermediariaDiversaAsync(string in0, string in1)
        {
            return base.Channel.peticionarIntermediariaDiversaAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> getTodosAssuntosAsync()
        {
            return base.Channel.getTodosAssuntosAsync();
        }
        
        public System.Threading.Tasks.Task<bool> logonAsync(string in0, string in1)
        {
            return base.Channel.logonAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoDadosDistribuicaoAsync(string in0)
        {
            return base.Channel.SolicitacaoDadosDistribuicaoAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> solicitacaoDocCienciaAtoAsync(string in0)
        {
            return base.Channel.solicitacaoDocCienciaAtoAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getTiposDocDigitalAsync()
        {
            return base.Channel.getTiposDocDigitalAsync();
        }
        
        public System.Threading.Tasks.Task<string> SolicitaListaIntimacoesAguardandoCienciaAsync(string in0)
        {
            return base.Channel.SolicitaListaIntimacoesAguardandoCienciaAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getAssuntosAsync(System.Nullable<int> in0, System.Nullable<int> in1)
        {
            return base.Channel.getAssuntosAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> getTiposDiversasAsync()
        {
            return base.Channel.getTiposDiversasAsync();
        }
        
        public System.Threading.Tasks.Task<string> SolicitaListaCitacoesAguardandoCienciaAsync(string in0)
        {
            return base.Channel.SolicitaListaCitacoesAguardandoCienciaAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoCitacaoAtoAsync(string in0, string in1)
        {
            return base.Channel.SolicitacaoCitacaoAtoAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoIntimacaoCienciaAsync(string in0, string in1)
        {
            return base.Channel.SolicitacaoIntimacaoCienciaAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> ConsultaPeticaoAsync(string in0)
        {
            return base.Channel.ConsultaPeticaoAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getAreasCompetenciasClassesEAssuntosAsync(System.Nullable<int> in0)
        {
            return base.Channel.getAreasCompetenciasClassesEAssuntosAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getAreasCompetenciasEClassesAsync(System.Nullable<int> in0)
        {
            return base.Channel.getAreasCompetenciasEClassesAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> getForosEVarasAsync()
        {
            return base.Channel.getForosEVarasAsync();
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoIntimacaoAutoConfirmadaAsync(string in0)
        {
            return base.Channel.SolicitacaoIntimacaoAutoConfirmadaAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> obterNumeroSajDoProcessoAsync(string in0)
        {
            return base.Channel.obterNumeroSajDoProcessoAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> SolicitacaoCitacaoCienciaAsync(string in0, string in1)
        {
            return base.Channel.SolicitacaoCitacaoCienciaAsync(in0, in1);
        }
        
        public System.Threading.Tasks.Task<string> ConfirmaLogonAsync(string in0)
        {
            return base.Channel.ConfirmaLogonAsync(in0);
        }
        
        public System.Threading.Tasks.Task<string> ajuizarAsync(string in0, string in1)
        {
            return base.Channel.ajuizarAsync(in0, in1);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ServicoPJ2HttpPort))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ServicoPJ2HttpPort))
            {
                return new System.ServiceModel.EndpointAddress("http://esaj.tjba.jus.br/tjws2/services/ServicoPJ2");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return ServicoPJ2PortTypeClient.GetBindingForEndpoint(EndpointConfiguration.ServicoPJ2HttpPort);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return ServicoPJ2PortTypeClient.GetEndpointAddress(EndpointConfiguration.ServicoPJ2HttpPort);
        }
        
        public enum EndpointConfiguration
        {
            
            ServicoPJ2HttpPort,
        }
    }
}
