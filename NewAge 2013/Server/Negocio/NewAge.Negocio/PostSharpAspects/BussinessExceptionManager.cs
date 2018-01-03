using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using NewAge.Librerias.ExceptionHandler;
using System.Configuration;

namespace NewAge.Negocio.PostSharpAspects
{
    [Serializable]
    public sealed class BussinessExceptionManager : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            ModuloBase modBase =  (ModuloBase)args.Instance;            
            Mentor_Exception.LogException_Local(modBase.loggerConnectionStr, args.Exception, modBase.UserId.ToString(), args.Method.Name.ToString());
            throw args.Exception;            
        }
    }
}
