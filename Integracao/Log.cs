using Core.Api.Data;
using Core.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Integracao
{
    public class Log
    {
        public DataContext DataContext { get; }

        public Log(DataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public void RegistrarLOGOperacao(TLogOperacao logOperacao)
        {
            try
            {
                this.DataContext.TLogOperacao.Add(logOperacao);
                this.DataContext.SaveChanges();
            }
            catch
            {

            }
        }
    }
}
