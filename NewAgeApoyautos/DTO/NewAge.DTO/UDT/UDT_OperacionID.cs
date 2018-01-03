using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.DTO.UDT
{
    /// <summary>
    /// Clase con el UDT de operacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class UDT_OperacionID : UDT
    {
        [DataMember]
        public static int MaxLength = 5;

        [DataMember]
        public static string Name = "OperacionID";

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
                    this._value = value.Trim().ToUpper();
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.ColRsx))
                        throw new MentorDataParametersException(this.ColRsx, MentorDataParametersException.ExType.Lenght, MaxLength);
                    else
                        throw new MentorDataParametersException(Name, MentorDataParametersException.ExType.Lenght, MaxLength);
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
