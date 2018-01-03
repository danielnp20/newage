using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NewAge.DTO.UDT
{
    //[DataContract]
    [SerializableAttribute]
    [DataContractAttribute]
    public abstract class UDT
    {
        /// <summary>
        /// trae el valor en forma de string
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();

        /// <summary>
        /// Asigna el valor de acuerod a un string
        /// </summary>
        /// <param name="valueStr"></param>
        public abstract void SetValueFromString(string valueStr);

        //public abstract 

        /// <summary>
        /// Retorna el valor por defecto de un tipo de udt
        /// </summary>
        /// <param name="udtType"></param>
        /// <returns></returns>
        public static string DefaultStringValue(Type udtType)
        {
            string res = string.Empty;
            if (udtType.Equals(typeof(UDT_Descriptivo)))
                res=string.Empty;
            if (udtType.Equals(typeof(UDT_SiNo)))
                res = bool.FalseString;
            return res;
        }

        /// <summary>
        /// Valida si un valor corresponde al formato del udt
        /// </summary>
        /// <param name="value">Valor</param>
        /// <param name="ex">Retorna la excepcion correspondiente</param>
        /// <returns>Verdadero si el fomato corresponde, Flaso si no</returns>
        public virtual bool ValidateFormat(string value, out Exception ex){
            ex = null;
            return true;
        }

        /// <summary>
        /// Devuelve la expresion regular dada un tipo de udt
        /// </summary>
        /// <param name="udtType"></param>
        /// <returns></returns>
        public static string GetRegex(Type udtType)
        {
            string res = DefaultRegex;
            if (udtType.Equals(typeof(UDTSQL_tinyint)))
                res = "[0-9]+";
            if (udtType.Equals(typeof(UDTSQL_bigint)) || udtType.Equals(typeof(UDTSQL_smallint)))
                res = "[0-9]+";
            if (udtType.Equals(typeof(UDTSQL_int)))
                res = "[0-9]+";
            if (udtType.Equals(typeof(UDT_BasicID)))
                res = "[a-z,A-Z,0-9]+";
            return res;
        }

        public static string DefaultRegex = ".*";

        /// <summary>
        /// Nombre de la columna en el idioma del usuario
        /// </summary>
        public string ColRsx = string.Empty;
    }
}
