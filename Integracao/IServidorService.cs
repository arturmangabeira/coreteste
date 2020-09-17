using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace IntegradorIdea.Integracao
{
    public interface IServidorService
    {
        [OperationContract]
        public string PararServicoIntegracao(string Login, string Senha);
    }
}
