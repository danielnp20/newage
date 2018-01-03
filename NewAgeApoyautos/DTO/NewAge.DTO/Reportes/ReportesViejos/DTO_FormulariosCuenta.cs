using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;
using NewAge.DTO.Negocio;

namespace NewAge.DTO.Reportes 
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte del Formulario resumido por cuenta
    /// </summary>
    public class DTO_FormulariosCuenta : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_FormulariosCuenta(IDataReader dr)
        {
            Declaracion = dr["Declaracion"].ToString();
            Renglon = dr["Renglon"].ToString();
            RenglonDesc = dr["RenglonDesc"].ToString();
            CuentaID = dr["CuentaID"].ToString();
            CuentaDesc = dr["CuentaDesc"].ToString();           
            ValorML = Convert.ToDecimal(dr["ValorML"]);
        }

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public DTO_FormulariosCuenta()
        {
        }

        #region Propiedades
        /// <summary>
        /// Tipo de la declaracion (IVA,ICA,Retefuent etc.)
        /// </summary>
        [DataMember]
        public string Declaracion { get; set; }

        /// <summary>
        /// Renglon del formulario
        /// </summary>
        [DataMember]
        public string Renglon { get; set; }

        /// <summary>
        /// Descripcion del renglon
        /// </summary>
        [DataMember]
        public string RenglonDesc { get; set; }

        /// <summary>
        /// ID de la Cuenta 
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        /// <summary>
        /// Descripcion de la Cuenta 
        /// </summary>
        [DataMember]
        public string CuentaDesc { get; set; }

        /// <summary>
        /// Valor por Cuenta (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal ValorML { get; set; }

        #endregion

    }
}
