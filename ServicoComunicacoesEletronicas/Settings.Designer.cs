﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServicoComunicacoesEletronicas {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("30")]
        public string FREQUENCIA_EXECUCAO_ROTINA {
            get {
                return ((string)(this["FREQUENCIA_EXECUCAO_ROTINA"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://integradoridea.dsv.sistemas.intranet.mpba.mp.br/api/intimacaocitacao/Obte" +
            "rListaIntimacaoCitacao")]
        public string URL_OBTER_LISTA_INTIMACOES {
            get {
                return ((string)(this["URL_OBTER_LISTA_INTIMACOES"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://integradoridea.dsv.sistemas.intranet.mpba.mp.br/api/intimacaocitacao/Obte" +
            "rDocCiencia")]
        public string URL_OBTER_DOC_CIENCIA {
            get {
                return ((string)(this["URL_OBTER_DOC_CIENCIA"]));
            }
            set {
                this["URL_OBTER_DOC_CIENCIA"] = value;
            }
        }
    }
}
