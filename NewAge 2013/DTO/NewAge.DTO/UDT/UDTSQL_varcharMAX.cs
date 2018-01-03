using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.DTO.UDT
{
    /// <summary>
    /// Clase con el UDT de Descripción
    /// </summary>
    [DataContract]
    [Serializable]
    public class UDTSQL_varcharMAX : UDTSQL_varchar
    {
        public static int RichTextMaxLength=300000;

        public UDTSQL_varcharMAX():base(RichTextMaxLength)
        {
        }
    }
}
