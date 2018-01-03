﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NewAge.DTO.UDT
{
    /// <summary>
    /// Clase con el udt de acciones
    /// </summary>
    [DataContract]
    [Serializable]
    public class UDT_AccionID : UDT
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
                this.Value = Convert.ToInt16(valueStr);
            }
            catch (Exception e)
            {
                this.Value = null;
            }
        }

        [DataMember]
        public Int16? Value { get; set; }
    }
}
