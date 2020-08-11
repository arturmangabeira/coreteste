using IntegradorIdea.Entidades.AssuntoClasse;
using IntegradorIdea.Entidades.CategoriaClasse;
using IntegradorIdea.Entidades.ForoClasse;
using IntegradorIdea.Entidades.TipoDiversasClasse;
using IntegradorIdea.Entidades.TpParteClasse;
using IntegradorIdea.Objects;
using System.Collections.Generic;
using System.ServiceModel;

namespace IntegradorIdea.Integracao
{
    [ServiceContract]
    public interface IIntegracaoService
    {           
        [OperationContract]
        public consultarProcessoResponse consultarProcesso(ConsultarProcesso consultarProcesso);
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
