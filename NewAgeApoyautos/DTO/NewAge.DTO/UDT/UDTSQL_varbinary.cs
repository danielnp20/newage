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
    public class UDTSQL_varbinary : UDT
    {
       
        public UDTSQL_varbinary(int maxLength)
        {
            this.MaxLength = maxLength;
        }

        public UDTSQL_varbinary()
            : base()
        {
        }

        [DataMember]
        private byte[] _value = new byte[0];

        [DataMember]
        public int MaxLength = 256;


        [DataMember]
        public byte[] Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (value.Length <= MaxLength)
                    this._value = value;
                else
                    throw new MentorDataParametersException(this.GetType().Name, MentorDataParametersException.ExType.Lenght, MaxLength);

            }
        }

        /// <summary>
        /// trae el valor en forma de string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Convert.ToBase64String(_value);
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
