using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Core.Api.Integracao;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace coreteste.Models
{
    public class Xml : IXml
    {
        public IConfiguration Configuration { get; }

        public Xml(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Assinatura

        /// <summary>
        /// Assina o documento XML.
        /// </summary>
        /// <param name="xml">Documento XML em texto plano.</param>
        /// <param name="tagAssinatura">Tag do XML que deverá ser assinada.</param>
        /// <param name="repositorio">Local onde está armazonado o certificado digital.</param>
        /// <param name="nomeCertificado">Nome (SubjectName) do certificado digital.</param>
        /// <returns>XML Assinado.</returns>
        public string AssinarXmlString(string xml, string repositorio, string nomeCertificado,string tag)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xml);

            AssinarXmlDocument(doc,repositorio, nomeCertificado,tag);

            return doc.OuterXml;
        }

        /// <summary>
        /// Assina o documento XML.
        /// </summary>
        /// <param name="doc">Documento XML.</param>
        /// <param name="TagAssinatura">Tag do XML que deverá ser assinada.</param>
        /// <param name="LocalRepositorio">Local onde está armazonado o certificado digital.</param>
        /// <param name="NomeCertificado">Nome (SubjectName) do certificado digital.</param>
        public void AssinarXmlDocument(XmlDocument doc, string LocalRepositorio, string NomeCertificado, string tag)
        {
            X509Certificate2 certificadoX509 = Certificado.ObterCertificado(LocalRepositorio, NomeCertificado, this.Configuration);
            AssinarXml(doc, certificadoX509, tag);
        }
              
        /// <summary>
        /// Assina o Dados.
        /// </summary>
        /// <param name="dados">Dados a ser assinado</param>
        /// <param name="LocalRepositorio">Local onde está armazonado o certificado digital.</param>
        /// <param name="NomeCertificado">Nome (SubjectName) do certificado digital.</param>
        public string AssinarDados(string dados, string LocalRepositorio, string NomeCertificado, string tag)
        {
            X509Certificate2 certificadoX509 = Certificado.ObterCertificado(LocalRepositorio, NomeCertificado, this.Configuration);

            //byte[] messageToSignBytes = Encoding.GetEncoding("iso-8859-1").GetBytes(dados.Trim());
            var converter = new ASCIIEncoding();
            byte[] messageToSignBytes = converter.GetBytes(dados.Trim());

            RSA rsa = (RSA)certificadoX509.PrivateKey;
            (certificadoX509.PrivateKey as RSACng).Key.SetProperty(
                new CngProperty(
                    "Export Policy",
                    BitConverter.GetBytes((int)CngExportPolicies.AllowPlaintextExport),
                    CngPropertyOptions.Persist));

            RSAParameters RSAParameters = rsa.ExportParameters(true);

            HashAlgorithm hasher = new SHA1Managed();

            // Use the hasher to hash the message
            byte[] hash = hasher.ComputeHash(messageToSignBytes);
                        
            var signature = rsa.SignData(messageToSignBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                        
            //var cipher01 = rsa.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signature);

        }
        /// <summary>
        /// Assina o documento XML.
        /// </summary>
        /// <param name="doc">Documento XML.</param>
        /// <param name="tagAssinatura">Tag do XML que deverá ser assinada.</param>
        /// <param name="certificadoX509">Local onde está armazonado o certificado digital.</param>
        private void AssinarXml(XmlDocument doc, X509Certificate2 certificadoX509,string tag)
        {
          
            KeyInfo keyinfo = new KeyInfo();

   
            KeyInfoX509Data x509data = new KeyInfoX509Data(certificadoX509);
        
            keyinfo.AddClause(x509data);
                    
            Reference reference = new Reference();
            reference.Uri = tag;

            XmlDsigEnvelopedSignatureTransform envelopedTransformation = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(envelopedTransformation);
           
            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = certificadoX509.PrivateKey;
            signedXml.KeyInfo = keyinfo;
            
            signedXml.AddReference(reference);
            
            signedXml.ComputeSignature();

            XmlElement signedElement = signedXml.GetXml();
           
            doc.DocumentElement.AppendChild(doc.ImportNode(signedElement, true));
            
        }
              
        #endregion
    }
}