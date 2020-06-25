namespace coreteste.Models
{
    public interface IXml
    {
         // <summary>
        /// Assina o documento XML.
        /// </summary>
        /// <param name="xml">Documento XML em texto plano.</param>
        /// <param name="tagAssinatura">Tag do XML que deverá ser assinada.</param>
        /// <param name="repositorio">Local onde está armazonado o certificado digital.</param>
        /// <param name="nomeCertificado">Nome (SubjectName) do certificado digital.</param>
        /// <returns>XML Assinado.</returns>
        string AssinarXmlString(string xml, string repositorio, string nomeCertificado,string tag);

        ///// <summary>
        ///// Assina o Dados.
        ///// </summary>
        ///// <param name="dados">Dados a ser assinado</param>
        ///// <param name="LocalRepositorio">Local onde está armazonado o certificado digital.</param>
        ///// <param name="NomeCertificado">Nome (SubjectName) do certificado digital.</param>
        string AssinarDados(string dados, string LocalRepositorio, string NomeCertificado, string tag);
    }
}