using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Core.Api.Integracao
{
    public class Certificado
    {               
        /// <summary>
        /// Obtém o certificado digital com base no local e nome do certificado.
        /// </summary>
        /// <param name="LocalRepositorio">Local onde está armazonado o certificado digital.</param>
        /// <param name="NomeCertificado">Nome (SubjectName) do certificado digital.</param>
        public static X509Certificate2 ObterCertificado(string LocalRepositorio, string NomeCertificado, IConfiguration configuration)
        {            
            X509Store x509Store = new X509Store(LocalRepositorio, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, NomeCertificado, false);
            X509Certificate2 x509Certificate2Retorno = null;
            string certificadoNome = "";
            bool encontradoCertificado = false;
            //PGMSIntegradorTJ.dividaativa.serverDividaAtiva daService = new PGMSIntegradorTJ.dividaativa.serverDividaAtiva();
            foreach (X509Certificate2 x509Certificate2CollectionAux in x509Certificate2Collection)
            {
                certificadoNome = x509Certificate2CollectionAux.Subject;
                if (certificadoNome.IndexOf(NomeCertificado) > 0)
                {
                    //Verificar se data de NotAfter é menor que a data atual, caso seja deve enviar e-mail sobre o certificado...
                    if (x509Certificate2CollectionAux.NotAfter < DateTime.Now)
                    {
                        /*//dispara email...                        
                        string loginDA = ConfigurationManager.AppSettings["LOGIN_DA"];
                        string senhaDA = ConfigurationManager.AppSettings["SENHA_DA"];
                        string destinatario = ConfigurationManager.AppSettings["emailAddress"];
                        string assunto = "Detectado certificado com a data de expiração maior que a data atual";
                        string mensagem = "O certificado (" + x509Certificate2CollectionAux.FriendlyName + ") foi encontrado na busca de certificados da máquina (StoreLocation.CurrentUser) com a data de expiração (" + x509Certificate2CollectionAux.GetExpirationDateString() +
                             ") maior que a data atual (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "). Favor realizar a exclusão do certificado.";
                        daService.EnviarEmail(loginDA, senhaDA, destinatario, assunto, mensagem, "", "", "", "", "", "");
                        */
                    }
                    else
                    {
                        //obtém o certificado com a data válida:
                        encontradoCertificado = true;
                        x509Certificate2Retorno = x509Certificate2CollectionAux;
                        /*
                        //verifica se a data está menor ou igual a 30 dias para vencer:
                        if((x509Certificate2CollectionAux.NotAfter - DateTime.Now).TotalDays <= 30)
                        {
                            //dispara email...                        
                            string loginDA = ConfigurationManager.AppSettings["LOGIN_DA"];
                            string senhaDA = ConfigurationManager.AppSettings["SENHA_DA"];
                            string destinatario = ConfigurationManager.AppSettings["emailAddress"];
                            string assunto = "Aviso de certificado com a data de expiração menor ou igual a 30 dias";
                            string mensagem = "O certificado (" + x509Certificate2CollectionAux.FriendlyName + ") foi encontrado na busca de certificados da máquina (StoreLocation.CurrentUser) com a data de expiração (" + x509Certificate2CollectionAux.GetExpirationDateString() +
                                 ") menor ou igual a 30 dias. Favor realizar a compra do certificado.";
                            daService.EnviarEmail(loginDA, senhaDA, destinatario, assunto, mensagem, "", "", "", "", "", "");
                        }
                        */
                    }
                }
            }

            //caso nessa execução tenha encontrado o certificado então o sistema irá retornar o mesmo.
            if (encontradoCertificado) return x509Certificate2Retorno;

            //Procura como LocalMachine
            if (!encontradoCertificado)
            {
                x509Store = new X509Store(LocalRepositorio, StoreLocation.LocalMachine);
                x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, NomeCertificado, false);
                if (x509Certificate2Collection.Count == 1)
                {
                    encontradoCertificado = true;
                    return x509Certificate2Collection[0];
                }
                else
                {
                    //caso encontre mais de um certificado, será analisado a data de expiração: NotAfter
                    if (x509Certificate2Collection.Count > 1)
                    {
                        foreach (X509Certificate2 x509Certificate2CollectionAux in x509Certificate2Collection)
                        {
                            //Verificar se data de NotAfter é menor que a data atual, caso seja deve enviar e-mail sobre o certificado...
                            /*if (x509Certificate2CollectionAux.NotAfter < DateTime.Now)
                            {
                                //dispara email...                                
                                string loginDA = ConfigurationManager.AppSettings["LOGIN_DA"];
                                string senhaDA = ConfigurationManager.AppSettings["SENHA_DA"];
                                string destinatario = ConfigurationManager.AppSettings["emailAddress"];
                                string assunto = "Detectado certificado com a data de expiração maior que a data atual";
                                string mensagem = "O certificado (" + x509Certificate2CollectionAux.FriendlyName + ") foi encontrado na busca de certificados da máquina (StoreLocation.LocalMachine) com a data de expiração (" + x509Certificate2CollectionAux.GetExpirationDateString() +
                                     ") maior que a data atual (" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "). Favor realizar a exclusão do certificado.";
                                daService.EnviarEmail(loginDA, senhaDA, destinatario, assunto, mensagem, "", "", "", "", "", "");
                            }
                            else
                            {*/
                                //obtém o certificado com a data válida:
                                encontradoCertificado = true;
                                x509Certificate2Retorno = x509Certificate2CollectionAux;
                            //}
                        }
                    }
                }

                if (encontradoCertificado) return x509Certificate2Retorno;
            }

            //Procura como LocalMachine
            if (!encontradoCertificado)
            {
                string nomeCertificado02 = configuration.GetValue<string>("Certificado:NomeCertificado");
                //string nomeCertificado02 = ConfigurationManager.AppSettings.Get("CertificadoEnvio02");
                //busca por currentuser e depois
                new X509Store(LocalRepositorio, StoreLocation.CurrentUser);
                x509Store.Open(OpenFlags.ReadOnly);
                x509Certificate2Collection = x509Store.Certificates;

                foreach (X509Certificate2 x509Certificate2CollectionAux in x509Certificate2Collection)
                {
                    certificadoNome = x509Certificate2CollectionAux.Subject;
                    if (certificadoNome.IndexOf(nomeCertificado02) > 0)
                    {
                        encontradoCertificado = true;
                        return x509Certificate2CollectionAux;
                    }
                }
                //Procura como LocalMachine
                if (!encontradoCertificado)
                {
                    x509Store = new X509Store(LocalRepositorio, StoreLocation.LocalMachine);
                    x509Store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                    x509Certificate2Collection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, nomeCertificado02, false);
                    if (x509Certificate2Collection.Count == 1)
                    {
                        encontradoCertificado = true;
                        return x509Certificate2Collection[0];
                    }
                }
            }
            throw new Exception("O certificado " + NomeCertificado + " não foi encontrado nesta máquina");
        }
    }
}
