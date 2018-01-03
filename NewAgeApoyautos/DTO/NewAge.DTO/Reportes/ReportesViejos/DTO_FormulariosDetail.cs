﻿using System;
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
    /// Clase del reporte detallado del Formulario
    /// </summary>
    public class DTO_FormulariosDetail : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_FormulariosDetail(IDataReader dr)
        {
            Declaracion = dr["Declaracion"].ToString();
            Renglon = dr["Renglon"].ToString();
            RenglonDesc = dr["RenglonDesc"].ToString();
            CuentaID = dr["CuentaID"].ToString();
            CuentaDesc = dr["CuentaDesc"].ToString();
            TerceroID = dr["TerceroID"].ToString();
            TerceroDesc = dr["TerceroDesc"].ToString();

            switch (Convert.ToInt32(dr["SaldoControl"]))
            {
                case (int)SaldoControl.Doc_Interno:
                    DocumentoID = (dr["DocumentoPrefijo"].ToString()).Trim() + "  "  +(dr["DocumentoNro"].ToString()).Trim();
                    break;
                case (int)SaldoControl.Doc_Externo:
                    DocumentoID = (dr["DocumentoTercero"].ToString()).Trim();
                    break;
                case -1:
                    DocumentoID = "";
                    break;
                default:
                    DocumentoID = " - ";
                    break;
            };

            ValorML = Convert.ToDecimal(dr["ValorML"]);
            BaseML = Convert.ToDecimal(dr["BaseML"]);
            Percent = (BaseML != 0) ? (Math.Round((ValorML / BaseML) * 100, 2)).ToString() : "*";
            ComprobanteID = dr["ComprobanteID"].ToString();
            ComprobanteNro = dr["ComprobanteNro"].ToString();
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_FormulariosDetail()
        {           
        }

        #region Propiedades
        /// <summary>
        /// Tipo de la declaracion 
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
        /// Tercero
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

        /// <summary>
        /// DocumentoCOM
        /// </summary>
        [DataMember]
        public string DocumentoID { get; set; }

        /// <summary>
        /// Valor por Tercero (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal ValorML { get; set; }

        /// <summary>
        /// Base por Tercero (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal BaseML { get; set; }

        /// <summary>
        /// (Valor/Base)*100%
        /// </summary>
        [DataMember]
        public string Percent { get; set; }

        /// <summary>
        /// ID del comprobante
        /// </summary>
        [DataMember]
        public string ComprobanteID { get; set; }

        /// <summary>
        /// Numero del comprobante
        /// </summary>
        [DataMember]
        public string ComprobanteNro { get; set; }
        #endregion

    }
}