using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NewAge.DTO.UDT
{
    [DataContract]
    [Serializable]
    public class UDT_PeriodoCert : UDT
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
        /// Asigna el valor de acuerdo a un string
        /// </summary>
        /// <param name="valueStr"></param>
        public override void SetValueFromString(string valueStr)
        {
            try
            {
                this.Value = Convert.ToByte(valueStr);
            }
            catch (Exception e)
            { ; }
        }

        [DataMember]
        public Byte? Value { get; set; }
    }
}
