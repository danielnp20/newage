using System;
using System.Reflection;
using PostSharp.Aspects;
using PostSharp.Extensibility;
using NewAge.Librerias.ExceptionHandler;
using System.Configuration;
using NewAge.Librerias.Project;

namespace NewAge.ADO
{
    [Serializable]
    public sealed class ADOExceptionManager : OnExceptionAspect
    {
        public override void OnException(MethodExecutionArgs args)
        {
            DAL_Base dalBase = (DAL_Base)args.Instance;
            Mentor_Exception.LogException_Local(dalBase.loggerConnectionStr, args.Exception, dalBase.UserId.ToString(), args.Method.Name.ToString());
            var message = new Exception(DictionaryMessages.Err_AccesoDatos + "&&" + args.Method.Name.ToString(), args.Exception);
            throw message;
        }
    }
}
