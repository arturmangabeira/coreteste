
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace IntegradorIdea.Integracao
{
    public class Assinar
    {
        private string pdforigem;
        private string pdfdestino;
        private MemoryStream msOrigem;
        private MemoryStream msDestino;             
        private int pagina;
        private string texto;
        private float posX;
        private float posY;
        private float altura;
        private float largura;
        private string proposito;
        private bool visivel;
        private string contato;
        private string localizacao;
        private bool multipagina;
        private bool cryptografado;        
        private string repositorio;
        private string nomecertificado;
        private X509Certificate2 card;
        private string SerialNumber;
        public IConfiguration Configuration { get; }
        public MemoryStream MsOrigem
        {
            get
            {
                return this.msOrigem;
            }
            set
            {
                this.msOrigem = value;
            }
        }
        public MemoryStream MsDestino
        {
            get
            {
                return this.msDestino;
            }
            set
            {
                this.msDestino = value;
            }
        }
        public string Repositorio
        {
            get
            {
                return this.repositorio;
            }
            set
            {
                this.repositorio = value;
            }
        }
        public string Nomecertificado
        {
            get
            {
                return this.nomecertificado;
            }
            set
            {
                this.nomecertificado = value;
            }
        }        
        public bool Cryptografado
        {
            get
            {
                return this.cryptografado;
            }
            set
            {
                this.cryptografado = value;
            }
        }
        public string Proposito
        {
            get
            {
                return this.proposito;
            }
            set
            {
                this.proposito = value;
            }
        }
        public string Contato
        {
            get
            {
                return this.contato;
            }
            set
            {
                this.contato = value;
            }
        }
        public string Localizacao
        {
            get
            {
                return this.localizacao;
            }
            set
            {
                this.localizacao = value;
            }
        }
        public bool Visivel
        {
            get
            {
                return this.visivel;
            }
            set
            {
                this.visivel = value;
            }
        }
        public bool Multipagina
        {
            get
            {
                return this.multipagina;
            }
            set
            {
                this.multipagina = value;
            }
        }
        public float Largura
        {
            get
            {
                return this.largura;
            }
            set
            {
                this.largura = value;
            }
        }
        public float PosX
        {
            get
            {
                return this.posX;
            }
            set
            {
                this.posX = value;
            }
        }
        public float PosY
        {
            get
            {
                return this.posY;
            }
            set
            {
                this.posY = value;
            }
        }
        public float Altura
        {
            get
            {
                return this.altura;
            }
            set
            {
                this.altura = value;
            }
        }
        public string Texto
        {
            get
            {
                return this.texto;
            }
            set
            {
                this.texto = value;
            }
        }
        public int Pagina
        {
            get
            {
                return this.pagina;
            }
            set
            {
                this.pagina = value;
            }
        }
       
        public string Pdforigem
        {
            get
            {
                return this.pdforigem;
            }
            set
            {
                this.pdforigem = value;
            }
        }
        public string Pdfdestino
        {
            get
            {
                return this.pdfdestino;
            }
            set
            {
                this.pdfdestino = value;
            }
        }
        public Assinar()
        {
            Configuration = ConfigurationManager.ConfigurationManager.AppSettings;
        }
        
        public byte[] AssinarPdfStreamCert(ref byte[] arquivo, string Repositorio, string Certificado)
        {
            MemoryStream arquivoOrigem = new MemoryStream(arquivo);
            X509Certificate2 cert = Integracao.Certificado.ObterCertificado(Repositorio, Certificado, this.Configuration);
            return this.Assinar2(arquivoOrigem, cert).ToArray();
        }

        public byte[] AssinarPdfStreamCert(ref byte[] arquivo, string Repositorio, string Certificado, ref byte[] pkc7)
        {
            MemoryStream arquivoOrigem = new MemoryStream(arquivo);
            X509Certificate2 cert = Integracao.Certificado.ObterCertificado(Repositorio, Certificado, this.Configuration);
            return this.Assinar2(arquivoOrigem, cert, ref pkc7).ToArray();
        }

        public byte[] AssinarPdfStreamCert(ref byte[] arquivo, string Certificado)
        {
            MemoryStream arquivoOrigem = new MemoryStream(arquivo);
            X509Certificate2 cert = Integracao.Certificado.ObterCertificado("", Certificado, this.Configuration);
            return this.Assinar2(arquivoOrigem, cert).ToArray();
        }
        
        private static byte[] SignSHA1withRSA(X509Certificate2 certificate, byte[] input)
        {
            IntPtr zero = IntPtr.Zero;
            bool flag = false;
            IntPtr zero2 = IntPtr.Zero;
            byte[] array = null;
            try
            {
                IntPtr handle = certificate.Handle;
                int dwKeySpec = 2;
                if (!Crypto.CryptAcquireCertificatePrivateKey(handle, 6, IntPtr.Zero, ref zero, ref dwKeySpec, ref flag))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }
                if (!Crypto.CryptCreateHash(zero, 32772, IntPtr.Zero, 0, ref zero2))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }
                MemoryStream memoryStream = new MemoryStream(input);
                byte[] array2 = new byte[4096];
                while (true)
                {
                    int num = memoryStream.Read(array2, 0, array2.Length);
                    if (num == 0)
                    {
                        break;
                    }
                    if (!Crypto.CryptHashData(zero2, array2, num, 0))
                    {
                        goto Block_6;
                    }
                }
                int num2 = 0;
                if (!Crypto.CryptSignHash(zero2, dwKeySpec, null, 0, null, ref num2))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }
                array = new byte[num2];
                if (!Crypto.CryptSignHash(zero2, dwKeySpec, null, 0, array, ref num2))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }
                Array.Reverse(array);
                return array;
            Block_6:
                throw new CryptographicException(Marshal.GetLastWin32Error());
            }
            finally
            {
                if (zero2 != IntPtr.Zero)
                {
                    if (!Crypto.CryptDestroyHash(zero2))
                    {
                        throw new CryptographicException(Marshal.GetLastWin32Error());
                    }
                }
                if (flag && zero != IntPtr.Zero)
                {
                    if (!Crypto.CryptReleaseContext(zero, 0))
                    {
                        throw new CryptographicException(Marshal.GetLastWin32Error());
                    }
                }
            }
            return array;
        }

        private MemoryStream Assinar2(MemoryStream ArquivoOrigem, X509Certificate2 cert)
        {
            this.card = cert;
            X509CertificateParser x509CertificateParser = new X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] array = new Org.BouncyCastle.X509.X509Certificate[]
            {
                x509CertificateParser.ReadCertificate(this.card.RawData)
            };
            PdfReader reader = new PdfReader(ArquivoOrigem);
            MemoryStream memoryStream = new MemoryStream();
            PdfStamper pdfStamper = PdfStamper.CreateSignature(reader, memoryStream, '\0', null, true);
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
            signatureAppearance.SetCrypto(null, array, null, PdfSignatureAppearance.SELF_SIGNED);
            signatureAppearance.Reason = this.proposito;
            signatureAppearance.Contact = this.contato;
            signatureAppearance.Location = this.localizacao;
            signatureAppearance.CryptoDictionary = new PdfSignature(PdfName.ADOBE_PPKLITE, new PdfName("adbe.pkcs7.detached"))
            {
                Reason = signatureAppearance.Reason,
                Location = signatureAppearance.Location,
                Contact = signatureAppearance.Contact,
                Date = new PdfDate(signatureAppearance.SignDate)
            };
            int num = 15000;
            Dictionary<PdfName, int> dictionary = new Dictionary<PdfName, int>();
            dictionary[PdfName.CONTENTS] = num * 2 + 2;
            signatureAppearance.PreClose(dictionary);
            PdfPKCS7 pdfPKCS = new PdfPKCS7(null, array, null, "SHA1", false);
            IDigest digest = DigestUtilities.GetDigest("SHA1");
            Stream rangeStream = signatureAppearance.GetRangeStream();
            byte[] array2 = new byte[8192];
            int length;
            while ((length = rangeStream.Read(array2, 0, array2.Length)) > 0)
            {
                digest.BlockUpdate(array2, 0, length);
            }
            byte[] array3 = new byte[digest.GetDigestSize()];
            digest.DoFinal(array3, 0);
            DateTime now = DateTime.Now;
            byte[] ocsp = null;
            if (array.Length >= 2)
            {
                string oCSPURL = PdfPKCS7.GetOCSPURL(array[0]);
                if (oCSPURL != null && oCSPURL.Length > 0)
                {
                    ocsp = new OcspClientBouncyCastle().GetEncoded(array[0], array[1], oCSPURL);
                }
            }
            byte[] authenticatedAttributeBytes = pdfPKCS.GetAuthenticatedAttributeBytes(array3, now, ocsp);
            byte[] digest2 = Assinar.SignSHA1withRSA(this.card, authenticatedAttributeBytes);
            pdfPKCS.SetExternalDigest(digest2, array3, "RSA");
            byte[] array4 = new byte[num];
            byte[] encodedPKCS = pdfPKCS.GetEncodedPKCS7(array3, now, null, ocsp);
            Array.Copy(encodedPKCS, 0, array4, 0, encodedPKCS.Length);
            if (num + 2 < encodedPKCS.Length)
            {
                throw new ApplicationException("Não há espaço suficiente para assinatura.");
            }
            PdfDictionary pdfDictionary = new PdfDictionary();
            pdfDictionary.Put(PdfName.CONTENTS, new PdfString(array4).SetHexWriting(true));
            signatureAppearance.Close(pdfDictionary);
            return memoryStream;
        }

        private MemoryStream Assinar2(MemoryStream ArquivoOrigem, X509Certificate2 cert, ref byte[] pkcs7)
        {
            this.card = cert;
            X509CertificateParser x509CertificateParser = new X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] array = new Org.BouncyCastle.X509.X509Certificate[]
            {
                x509CertificateParser.ReadCertificate(this.card.RawData)
            };
            PdfReader reader = new PdfReader(ArquivoOrigem);
            MemoryStream memoryStream = new MemoryStream();
            PdfStamper pdfStamper = PdfStamper.CreateSignature(reader, memoryStream, '\0', null, true);
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
            signatureAppearance.SetCrypto(null, array, null, PdfSignatureAppearance.SELF_SIGNED);
            signatureAppearance.Reason = this.proposito;
            signatureAppearance.Contact = this.contato;
            signatureAppearance.Location = this.localizacao;
            signatureAppearance.CryptoDictionary = new PdfSignature(PdfName.ADOBE_PPKLITE, new PdfName("adbe.pkcs7.detached"))
            {
                Reason = signatureAppearance.Reason,
                Location = signatureAppearance.Location,
                Contact = signatureAppearance.Contact,
                Date = new PdfDate(signatureAppearance.SignDate)
            };
            int num = 15000;
            Dictionary<PdfName, int> dictionary = new Dictionary<PdfName, int>();
            dictionary[PdfName.CONTENTS] = num * 2 + 2;
            signatureAppearance.PreClose(dictionary);
            //PdfPKCS7 pdfPKCS = new PdfPKCS7(null, array, null, "SHA1", false);
            PdfPKCS7 pdfPKCS = new PdfPKCS7(null, array, null, "MD5", false);
            IDigest digest = DigestUtilities.GetDigest("MD5");
            Stream rangeStream = signatureAppearance.GetRangeStream();
            byte[] array2 = new byte[8192];
            int length;
            while ((length = rangeStream.Read(array2, 0, array2.Length)) > 0)
            {
                digest.BlockUpdate(array2, 0, length);
            }
            byte[] array3 = new byte[digest.GetDigestSize()];
            digest.DoFinal(array3, 0);
            DateTime now = DateTime.Now;
            byte[] ocsp = null;
            if (array.Length >= 2)
            {
                string oCSPURL = PdfPKCS7.GetOCSPURL(array[0]);
                if (oCSPURL != null && oCSPURL.Length > 0)
                {
                    ocsp = new OcspClientBouncyCastle().GetEncoded(array[0], array[1], oCSPURL);
                }
            }
            byte[] authenticatedAttributeBytes = pdfPKCS.GetAuthenticatedAttributeBytes(array3, now, ocsp);
            byte[] digest2 = Assinar.SignSHA1withRSA(this.card, authenticatedAttributeBytes);
            pdfPKCS.SetExternalDigest(digest2, array3, "RSA");
            byte[] array4 = new byte[num];
            byte[] encodedPKCS = pdfPKCS.GetEncodedPKCS7(array3, now, null, ocsp);
            pkcs7 = encodedPKCS;
            Array.Copy(encodedPKCS, 0, array4, 0, encodedPKCS.Length);
            if (num + 2 < encodedPKCS.Length)
            {
                throw new ApplicationException("Não há espaço suficiente para assinatura.");
            }
            PdfDictionary pdfDictionary = new PdfDictionary();
            pdfDictionary.Put(PdfName.CONTENTS, new PdfString(array4).SetHexWriting(true));
            signatureAppearance.Close(pdfDictionary);
            //pdfStamper.
            return memoryStream;
        }

        public void LimparCertificado()
        {
            this.SerialNumber = null;
        }
        public bool VerificarAssinatura(byte[] pdf)
        {
            PdfReader pdfReader = new PdfReader(pdf);
            AcroFields acroFields = pdfReader.AcroFields;
            IList<string> signatureNames = acroFields.GetSignatureNames();
            bool flag = false;
            foreach (string current in signatureNames)
            {
                PdfPKCS7 pdfPKCS = acroFields.VerifySignature(current);
                DateTime signDate = pdfPKCS.SignDate;
                flag = pdfPKCS.Verify();
                if (!flag)
                {
                    break;
                }
            }
            return flag;
        }
        
        public bool VerificarNomeCertificado(string Repositorio, string NomeCertificado)
        {
            X509Store x509Store = new X509Store(Repositorio, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates;
            string certificadoNome = "";
            foreach (X509Certificate2 x509Certificate2CollectionAux in x509Certificate2Collection)
            {
                certificadoNome = x509Certificate2CollectionAux.Subject;
                if (certificadoNome.IndexOf(NomeCertificado) > 0)
                {
                    return true; ;
                }
            }

            return false;
        }

        public X509Certificate2 ObterCertificado(string NomeCertificado)
        {
            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates;
            string certificadoNome = "";
            bool encontradoCertificado = false;
            foreach (X509Certificate2 x509Certificate2CollectionAux in x509Certificate2Collection)
            {
                certificadoNome = x509Certificate2CollectionAux.Subject;
                if (certificadoNome.IndexOf(NomeCertificado) > 0)
                {
                    encontradoCertificado = true;
                    return x509Certificate2CollectionAux;
                }
            }
            if (x509Certificate2Collection.Count == 1)
            {
                return x509Certificate2Collection[0];
            }

            //Procura como LocalMachine
            if (!encontradoCertificado)
            {
                x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, NomeCertificado, false);
                if (x509Certificate2Collection.Count == 1)
                {
                    return x509Certificate2Collection[0];
                }
            }
            throw new Exception("O certificado " + NomeCertificado + " não foi encontrado nesta máquina");

        }

        public X509Certificate2 ObterCertificadoComChavePrivada(string NomeCertificado)
        {
            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            X509Certificate2 X509Certificate2 = null;
            x509Store.Open(OpenFlags.ReadOnly);
            try
            {
                foreach (var c in x509Store.Certificates)
                {
                    if (c.Subject.IndexOf(NomeCertificado) > 0 && c.HasPrivateKey)
                    {
                        X509Certificate2 = c;
                        break;
                    }
                }
            }
            finally
            {
                x509Store.Close();
            }

            return X509Certificate2;
        }
        
        public X509Certificate2 ObterCertificado()
        {
            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            //x509Store.Open(OpenFlags.ReadOnly);
            x509Store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
            X509Certificate2Collection certificates = x509Store.Certificates;
            X509Certificate2 x509Certificate = null;
            if (this.SerialNumber == null)
            {
                X509Certificate2Collection x509Certificate2Collection = X509Certificate2UI.SelectFromCollection(certificates, "Certificados Disponíveis", "Selecione o Certificado para ser enviado ao recurso que está tentando acessar.", X509SelectionFlag.SingleSelection);
                if (x509Certificate2Collection.Count > 0)
                {
                    X509Certificate2Enumerator enumerator = x509Certificate2Collection.GetEnumerator();
                    enumerator.MoveNext();
                    x509Certificate = enumerator.Current;
                    this.SerialNumber = x509Certificate.GetSerialNumberString();
                }
            }
            else
            {
                X509Certificate2Collection x509Certificate2Collection2 = x509Store.Certificates.Find(X509FindType.FindBySerialNumber, this.SerialNumber, false);
                if (x509Certificate2Collection2.Count != 1)
                {
                    throw new Exception("O certificado " + this.SerialNumber + " não foi encontrado nesta máquina");
                }
                x509Certificate = x509Certificate2Collection2[0];
            }
            x509Store.Close();
            return x509Certificate;
        }
    }
}
