using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using System.Transactions;
using NewAge.DTO.Resultados;
using System.Data.SqlClient;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.Negocio.PostSharpAspects
{
    [Serializable]
    public sealed class Transaction : OnMethodBoundaryAspect
    {
        #region Properties

        [NonSerialized]
        TransactionScope TransactionScope;
        
        public Transaction()
        {

        }
      
        #endregion

        public override void OnEntry(MethodExecutionArgs args)
        {
            this.TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        public override void OnExit(MethodExecutionArgs args)
        {            
            this.TransactionScope.Dispose();
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {            
            this.TransactionScope.Complete(); 
        }

        public override void OnException(MethodExecutionArgs args)
        {
            ModuloBase modBase = (ModuloBase)args.Instance;
            Mentor_Exception.LogException_Local(modBase.loggerConnectionStr, args.Exception, modBase.UserId.ToString(), args.Method.Name.ToString());
            throw args.Exception;              
        }        
    }
}
