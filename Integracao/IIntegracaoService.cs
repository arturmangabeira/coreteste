﻿using Core.Api.Entidades.AssuntoClasse;
using Core.Api.Entidades.CategoriaClasse;
using Core.Api.Entidades.DocDigitalClasse;
using Core.Api.Entidades.ForoClasse;
using Core.Api.Entidades.TipoDiversasClasse;
using Core.Api.Entidades.TpParteClasse;
using Core.Api.Models;
using Core.Api.Objects;
using System.Collections.Generic;
using System.ServiceModel;

namespace Core.Api.Integracao
{
    [ServiceContract]
    public interface IIntegracaoService
    {           
        [OperationContract]
        public consultarProcessoResponse consultarProcesso(string idConsultante, string senhaConsultante, string numeroProcesso, bool movimentos, bool incluirCabecalho, bool incluirDocumentos, string[] documento);
        [OperationContract]
        [XmlSerializerFormat(SupportFaults = true,Style = OperationFormatStyle.Rpc,Use = OperationFormatUse.Encoded)]        
        public Foros getForosEVaras();
        [OperationContract]
        public Classes getClasseTpParte();        
        [OperationContract]
        public Categorias getCategoriasEClasses();
        [OperationContract]
        public Tipos getTiposDiversas();
        [OperationContract]
        public string getAreasCompetenciasEClasses(int cdForo);
        [OperationContract]
        public string obterNumeroUnificadoDoProcesso(string outroNumeroProcesso);
        [OperationContract]
        public string obterNumeroSajDoProcesso(string numeroUnificadoProcesso);
        [OperationContract]
        public Assuntos getAssuntos(int cdCompetencia, int cdClasse);
        [OperationContract]
        public List<FilaPastaDigital> consultarSituacaoDocumentosProcesso(int Cdidea, string numeroProcesso);
        [OperationContract]
        public consultarAvisosPendentesResponse consultarAvisosPendentes(ConsultarAvisosPendentes consultarAvisosPendentes);
    }
}
