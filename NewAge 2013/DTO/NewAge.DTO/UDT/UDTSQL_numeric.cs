using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT.ExcepcionesFormatos;

namespace NewAge.DTO.UDT
{
    [DataContract]
    [Serializable]
    public class UDTSQL_numeric : UDT
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
                valueStr = valueStr.Replace(",", ".");
                this.Value = Convert.ToDecimal(valueStr, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch (Exception e)
            {
                this.Value = null;
            }
        }

        /// <summary>
        /// Valida si un valor corresponde al formato del udt
        /// </summary>
        /// <param name="value">Valor</param>
        /// <param name="ex">Retorna la excepcion correspondiente</param>
        /// <returns>Verdadero si el fomato corresponde, Flaso si no</returns>
        public override bool ValidateFormat(string valueStr, out Exception ex)
        {
            try
            {
                Convert.ToDecimal(valueStr);
                ex = null;
                return true;
            }
            catch (Exception e)
            {
                ex = new InvaliddecimalFormatException();
                return false;
            }
        }

        [DataMember]
        public decimal? Value { get; set; }
    }
}
