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
    public class UDT_Imagen : UDT
    {
        protected byte[] _value = new byte[0];

        public UDT_Imagen()
            : base()
        {
        }

        [DataMember]
        public byte[] Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }

        /// <summary>
        /// trae el valor en forma de string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Convert.ToBase64String(this._value);
        }

        /// <summary>
        /// Asigna el valor de acuerod a un string
        /// </summary>
        /// <param name="valueStr"></param>
        public override void SetValueFromString(string valueStr)
        {
            return;
        }
    }
}
