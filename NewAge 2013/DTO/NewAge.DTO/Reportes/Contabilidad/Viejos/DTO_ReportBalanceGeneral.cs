using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Negocio.Reportes;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Balance De Prueba por meses
    /// </summary>
    public class DTO_ReportBalanceGeneral : DTO_BasicReport
    {
        /// <summary>
        /// Constructor using data reader
        /// </summary>
        public DTO_ReportBalanceGeneral(IDataReader dr)
        {
             CuentaID = dr["CuentaID"].ToString();
             CuentaDesc = dr["CuentaDesc"].ToString();
             MaxLengthInd = Convert.ToInt16(dr["MaxLengthInd"]);
             FinalML_L2 = (string.IsNullOrEmpty(dr["FinalML_L2"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalML_L2"]).ToString("#,0.00");
             FinalME_L2 = (string.IsNullOrEmpty(dr["FinalME_L2"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalME_L2"]).ToString("#,0.00");
             FinalML_L3 = (string.IsNullOrEmpty(dr["FinalML_L3"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalML_L3"]).ToString("#,0.00");
             FinalME_L3 = (string.IsNullOrEmpty(dr["FinalME_L3"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalME_L3"]).ToString("#,0.00");
             FinalML_L4 = (string.IsNullOrEmpty(dr["FinalML_L4"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalML_L4"]).ToString("#,0.00");
             FinalME_L4 = (string.IsNullOrEmpty(dr["FinalME_L4"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalME_L4"]).ToString("#,0.00");
             FinalML_L5 = (string.IsNullOrEmpty(dr["FinalML_L5"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalML_L5"]).ToString("#,0.00");
             FinalME_L5 = (string.IsNullOrEmpty(dr["FinalME_L5"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalME_L5"]).ToString("#,0.00");
             FinalML_L6 = (string.IsNullOrEmpty(dr["FinalML_L6"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalML_L6"]).ToString("#,0.00");
             FinalME_L6 = (string.IsNullOrEmpty(dr["FinalME_L6"].ToString().Trim())) ? "" : Convert.ToDecimal(dr["FinalME_L6"]).ToString("#,0.00");
        }

        #region Propiedades

        /// <summary>
        /// Cuenat ID
        /// </summary>
        [DataMember]
        public string CuentaID { get; set; }

        /// <summary>
        /// Descripcion de la Cuenta
        /// </summary>
        [DataMember]
        public string CuentaDesc { get; set; }

        /// <summary>
        /// Indicador que longitud de la Cuenta es igual a Mascara de la Cuanta
        /// </summary>
        [DataMember]
        public int MaxLengthInd { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 2 (Moneda Local)
        /// </summary>
        [DataMember]
        public string FinalML_L2 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 2 (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public string FinalME_L2 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 3 (Moneda Local)
        /// </summary>
        [DataMember] 
        public string FinalML_L3 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 3 (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public string FinalME_L3 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 4 (Moneda Local)
        /// </summary>
        [DataMember]
        public string FinalML_L4 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 4 (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public string FinalME_L4 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 5 (Moneda Local)
        /// </summary>
        [DataMember]
        public string FinalML_L5 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 5 (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public string FinalME_L5 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 6  (Moneda Local)
        /// </summary>
        [DataMember]
        public string FinalML_L6 { get; set; }

        /// <summary>
        /// Saldo Final de la cuenta del nivel 6 (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public string FinalME_L6 { get; set; }
        
        #endregion
    }
}

