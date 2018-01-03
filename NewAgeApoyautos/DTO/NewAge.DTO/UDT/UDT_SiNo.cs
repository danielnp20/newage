using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT.ExcepcionesFormatos;

namespace NewAge.DTO.UDT
{
    /// <summary>
    /// Clase con el udt del booleano SiNo
    /// </summary>
    [DataContract]
    [Serializable]
    public class UDT_SiNo : UDT
    {
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
            try
            {
                this.Value = Convert.ToBoolean(valueStr);
                if (this.Value == null)
                {
                    //Validar mediante "X" y ""
                    if (valueStr.Equals("X") || valueStr.Equals("x"))
                    {
                        this.Value = true;
                    }
                    if (valueStr.Equals(""))
                        this.Value = false;
                }
            }
            catch (Exception e)
            {
                this.Value = null;
                if (valueStr.Equals("X") || valueStr.Equals("x"))
                {
                    this.Value = true;
                }
                if (valueStr.Equals(""))
                    this.Value = false;
            }
        }

        /// <summary>
        /// Valida si un valor corresponde al formato del udt
        /// </summary>
        /// <param name="value">Valor</param>
        /// <param name="ex">Retorna la excepcion correspondiente</param>
        /// <returns>Verdadero si el fomato corresponde, Flaso si no</returns>
        public override bool ValidateFormat(string value, out Exception ex)
        {
            if (value.Equals(true.ToString()) || value.Equals(false.ToString()) || value.Equals("X") || value.Equals("x") || value.Equals(""))
            {
                ex = null;
                return true;
            }
            else
            {
                ex = new InvalidBoolFormatException();
                return false;
            }
        }

        [DataMember]
        public bool? Value { get; set; }
    }
}
