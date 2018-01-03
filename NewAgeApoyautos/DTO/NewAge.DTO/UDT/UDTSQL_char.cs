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
    public class UDTSQL_char : UDT
    {
        public UDTSQL_char(int maxLength)
        {
            this.MaxLength = maxLength;
        }

        [DataMember]
        public int MaxLength = 50;

        [DataMember]
        private string _value = string.Empty;

        [DataMember]
        public string Value
        { 
            get
            {
                return this._value.Trim();
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                if (value.Trim().Length <= MaxLength)
                    this._value = value.Trim();
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.ColRsx))
                        throw new MentorDataParametersException(this.ColRsx, MentorDataParametersException.ExType.Lenght, MaxLength);
                    else
                        throw new MentorDataParametersException(this.GetType().Name, MentorDataParametersException.ExType.Lenght, MaxLength);
                }
            }
        }

        /// <summary>
        /// Implementacion del tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value;
        }

        /// <summary>
        /// Asigna el valor de acuerod a un string
        /// </summary>
        /// <param name="valueStr"></param>
        public override void SetValueFromString(string valueStr)
        {
            this.Value = (valueStr);
        }
    }
}
