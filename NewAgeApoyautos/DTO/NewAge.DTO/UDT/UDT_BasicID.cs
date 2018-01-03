using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.Librerias.ExceptionHandler;

namespace NewAge.DTO.UDT
{
    /// <summary>
    /// Clase con el udt de Área Básica
    /// </summary>
    [DataContract]
    [Serializable]
    public class UDT_BasicID : UDT
    {
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public UDT_BasicID()
        {
            this.IsInt = false;
        }

        /// <summary>
        /// Constructor indicando si el campo es entero
        /// </summary>
        /// <param name="isInt"></param>
        public UDT_BasicID(bool isInt)
        {
            this.IsInt = isInt;
        }

        /// <summary>
        /// Sobrecarga el metodo tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Asigna el valor de acuerod a un string
        /// </summary>
        /// <param name="valueStr"></param>
        public override void SetValueFromString(string valueStr)
        {
            this.Value = (valueStr);
        }

        [DataMember]
        public bool IsInt;

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
                    this._value = value.Trim().ToUpper();
                else
                {
                    if (!string.IsNullOrWhiteSpace(this.ColRsx))
                        throw new MentorDataParametersException(this.ColRsx, MentorDataParametersException.ExType.Lenght, MaxLength);
                    else
                        throw new MentorDataParametersException("ID", MentorDataParametersException.ExType.Lenght, MaxLength);
                }
            }
        }
    }
}
