using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT.ExcepcionesFormatos;
using NewAge.Librerias.Project;

namespace NewAge.DTO.UDT
{
    [DataContract]
    [Serializable]
    public class UDT_PeriodoID : UDT
    {
        /// <summary>
        /// Sobrecarga el metodo tostring
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string ret = string.Empty;
            if (this.Value.HasValue)
                ret = this.Value.Value.ToString(FormatString.Date);

            return ret;
        }

        /// <summary>
        /// Asigna el valor de acuerdo a un string
        /// </summary>
        /// <param name="valueStr"></param>
        public override void SetValueFromString(string valueStr)
        {
            try
            {
                if (valueStr.Length == 9) valueStr = "0" + valueStr; //Valida que la fecha este completa
                this.Value = DateTime.ParseExact(valueStr, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
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
        public override bool ValidateFormat(string value, out Exception ex)
        {
            try
            {
                if (value.Length == 9) value = "0" + value; //Valida que la fecha este completa
                DateTime.ParseExact(value, FormatString.Date, System.Globalization.CultureInfo.InvariantCulture);
                ex = null;
                return true;
            }
            catch (Exception e)
            {
                ex = new InvalidDateFormatException();
                return false;
            }
        }

        [DataMember]
        public DateTime? Value { get; set; }
    }
}
