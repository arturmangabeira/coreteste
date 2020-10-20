using IntegradorIdea.Entidades.AssuntoClasse;
using IntegradorIdea.Entidades.CategoriaClasse;
using IntegradorIdea.Entidades.DocDigitalClasse;
using IntegradorIdea.Entidades.ForoClasse;
using IntegradorIdea.Entidades.TipoDiversasClasse;
using IntegradorIdea.Entidades.TpParteClasse;
using IntegradorIdea.Objects;
using IntegradorIdea.Objects.Response;
using System.Collections.Generic;
using System.ServiceModel;

namespace IntegradorIdea.Integracao
{
    [ServiceContract(Namespace = "http://www.cnj.jus.br/intercomunicacao-2.2.2")]
    public interface IIntegracaoService
    {        

        [OperationContract]
        public IntegradorIdea.Objects.Response.GetTiposDocDigitalResponse getTiposDocDigital();

        [OperationContract]
        public IntegradorIdea.Objects.Response.GetForosEVarasResponse getForosEVaras();

        // @TODO ajustar retorno
        [OperationContract]
        public IntegradorIdea.Objects.Response.GetAreasCompetenciasEClassesResponse.GetAreasCompetenciasEClassesResponse getAreasCompetenciasEClasses(int cdForo);

        [OperationContract]
        public IntegradorIdea.Objects.Response.getClasseTpParteResponse getClasseTpParte();

        [OperationContract]
        public IntegradorIdea.Objects.Response.GetCategoriasEClassesResponse getCategoriasEClasses();

        [OperationContract]
        public IntegradorIdea.Objects.Response.GetTiposDiversasResponse getTiposDiversas();

        [OperationContract]
        public IntegradorIdea.Objects.Response.GetAssuntosResponse getAssuntos(int cdCompetencia, int cdClasse);

        [OperationContract]
        public List<FilaPastaDigital> consultarSituacaoDocumentosProcesso(int Cdidea, string numeroProcesso);

        
        //[OperationContract]
        //public Objects.ConsultarProcessoESAJ.consultarProcessoResposta consultarProcesso02(int idConsultante, string numeroProcesso, bool movimentos, bool incluirCabecalho, bool incluirDocumentos, string[] documento);                 
        [OperationContract]
        public consultarProcessoResponse consultarProcesso(int idConsultante, string numeroProcesso, bool movimentos, bool incluirCabecalho, bool incluirDocumentos, string[] documento);        
        /*        
        [OperationContract]
        public string obterNumeroUnificadoDoProcesso(string outroNumeroProcesso);
        [OperationContract]
        public string obterNumeroSajDoProcesso(string numeroUnificadoProcesso);
        
        [OperationContract]
        public consultarAvisosPendentesResponse consultarAvisosPendentes(string idRepresentado, int idConsultante, string senhaConsultante, string dataReferencia);
        [OperationContract]
        public consultarTeorComunicacaoResponse consultarTeorComunicacao( string idConsultante, string senhaConsultante, string numeroProcesso, string identificadorAviso );
        [OperationContract]
        public entregarManifestacaoProcessualResponse entregarManifestacaoProcessual(string idManifestante, string senhaManifestante, string numeroProcesso, tipoCabecalhoProcesso dadosBasicos, Documento documento, string dataEnvio, tipoParametro[] parametros);
        */

    }
}
