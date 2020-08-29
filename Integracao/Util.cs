using System;
using IntegradorIdea.Entidades;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text.pdf.parser;
using System.Net;
using System.Net.Sockets;

namespace IntegradorIdea.Integracao
{
    public class Util
    {
        private static Random random = new Random();
        private static Regex numbersOnly = new Regex(@"[^\d]");
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Serializar(string objeto, string repositorio, string certificado)
        {
            Message objAjuizamento = new Message();
            objAjuizamento = objAjuizamento.ExtrairObjeto<Message>(objeto);
            string str = objAjuizamento.Serialize();

            IXml objXML = new Xml();
            return objXML.AssinarXmlString(str, repositorio, certificado, "");
        }

        public static string Serializar(Object obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(obj.GetType());            
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, obj);
                    xml = sww.ToString(); 
                }
            }

            return xml;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string OnlyNumbers(string document)
        {
            return numbersOnly.Replace(document.Trim(), "");
        }

        public static string ExtrairTextoPDF(byte[] pdf)
        {
            PdfReader leitor = new PdfReader(pdf);

            StringBuilder texto = new StringBuilder();
            for (int i = 1; i <= leitor.NumberOfPages; i++)
            {
                texto.Append(PdfTextExtractor.GetTextFromPage(leitor, i));
            }

            return texto.ToString();
        }

        public static string GetIpOrigem()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            return host
               .AddressList
               .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork).ToString();
        }

        public static string GetUUIID()
        {
            return System.Guid.NewGuid().ToString();
        }

        public static byte[] AssinarPDF(ref byte[] dadosArquivo)
        {            
            var configuration = ConfigurationManager.ConfigurationManager.AppSettings;
            Assinar objAssinar = new Assinar();

            return objAssinar.AssinarPdfStreamCert(ref dadosArquivo, configuration["Certificado:RepositorioCertificado"], configuration["Certificado:ThumberPrint"]);
            
        }
    }
}
