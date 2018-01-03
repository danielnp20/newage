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
    public class DTO_ReportBalanceDePruebaPorMeses : DTO_BasicReport
    {
        /// <summary>
        /// Constructor using data reader
        /// </summary>
        public DTO_ReportBalanceDePruebaPorMeses(IDataReader dr)
        {
             YearID = new DateTime(Convert.ToInt32(dr["Año"]), 1, 1);
             CuentaID = dr["CuentaID"].ToString();
             CuentaDesc = dr["CuentaDesc"].ToString();
             MaxLengthInd = Convert.ToInt16(dr["MaxLengthInd"]);
             InicialML = Convert.ToDecimal(dr["InicialML"]);
             InicialME = Convert.ToDecimal(dr["InicialME"]);
             MovML_01 = Convert.ToDecimal(dr["MovML_01"]);
             MovME_01 = Convert.ToDecimal(dr["MovME_01"]);
             MovML_02 = Convert.ToDecimal(dr["MovML_02"]);
             MovME_02 = Convert.ToDecimal(dr["MovME_02"]);
             MovML_03 = Convert.ToDecimal(dr["MovML_03"]);
             MovME_03 = Convert.ToDecimal(dr["MovME_03"]);
             MovML_04 = Convert.ToDecimal(dr["MovML_04"]);
             MovME_04 = Convert.ToDecimal(dr["MovME_04"]);
             MovML_05 = Convert.ToDecimal(dr["MovML_05"]);
             MovME_05 = Convert.ToDecimal(dr["MovME_05"]);
             MovML_06 = Convert.ToDecimal(dr["MovML_06"]);
             MovME_06 = Convert.ToDecimal(dr["MovME_06"]);
             MovML_07 = Convert.ToDecimal(dr["MovML_07"]);
             MovME_07 = Convert.ToDecimal(dr["MovME_07"]);
             MovML_08 = Convert.ToDecimal(dr["MovML_08"]);
             MovME_08 = Convert.ToDecimal(dr["MovME_08"]);
             MovML_09 = Convert.ToDecimal(dr["MovML_09"]);
             MovME_09 = Convert.ToDecimal(dr["MovME_09"]);
             MovML_10 = Convert.ToDecimal(dr["MovML_10"]);
             MovME_10 = Convert.ToDecimal(dr["MovME_10"]);
             MovML_11 = Convert.ToDecimal(dr["MovML_11"]);
             MovME_11 = Convert.ToDecimal(dr["MovME_11"]);
             MovML_12 = Convert.ToDecimal(dr["MovML_12"]);
             MovME_12 = Convert.ToDecimal(dr["MovME_12"]);
             TotalML_Year = Convert.ToDecimal(dr["Y_ML"]);
             TotalME_Year = Convert.ToDecimal(dr["Y_ME"]);
        }

        #region Propiedades

        /// <summary>
        /// Año del reporte
        /// </summary>
        [DataMember]
        public DateTime YearID { get; set; }
        
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
        /// Saldo Inicial (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal InicialML { get; set; }

        /// <summary>
        /// Saldo Inicial (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal InicialME { get; set; }

        /// <summary>
        /// Movimiento en Enero (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_01 { get; set; }

        /// <summary>
        /// Movimiento en Enero (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_01 { get; set; }

        /// <summary>
        /// Movimiento en Febrero (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_02 { get; set; }

        /// <summary>
        /// Movimiento en Febrero (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_02 { get; set; }

        /// <summary>
        /// Movimiento en Marzo (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_03 { get; set; }

        /// <summary>
        /// Movimiento en Marzo (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_03 { get; set; }

        /// <summary>
        /// Movimiento en Abril (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_04 { get; set; }

        /// <summary>
        /// Movimiento en Abril (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_04 { get; set; }

        /// <summary>
        /// Movimiento en Mayo (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_05 { get; set; }

        /// <summary>
        /// Movimiento en Mayo (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_05 { get; set; }

        /// <summary>
        /// Movimiento en Junio (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_06 { get; set; }

        /// <summary>
        /// Movimiento en Junio (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_06 { get; set; }

        /// <summary>
        /// Movimiento en Julio (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_07 { get; set; }

        /// <summary>
        /// Movimiento en Julio (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_07 { get; set; }

        /// <summary>
        /// Movimiento en Agosto (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_08 { get; set; }

        /// <summary>
        /// Movimiento en Agosto (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_08 { get; set; }

        /// <summary>
        /// Movimiento en Septiembre (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_09 { get; set; }

        /// <summary>
        /// Movimiento en Septiembre (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_09 { get; set; }

        /// <summary>
        /// Movimiento en Octubre (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_10 { get; set; }

        /// <summary>
        /// Movimiento en Octubre (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_10 { get; set; }

        /// <summary>
        /// Movimiento en Noviebmre (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_11 { get; set; }

        /// <summary>
        /// Movimiento en Noviebmre (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_11 { get; set; }

        /// <summary>
        /// Movimiento en Diciembre (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal  MovML_12 { get; set; }

        /// <summary>
        /// Movimiento en Diciembre (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal  MovME_12 { get; set; }

        /// <summary>
        /// Total Movimiento en el año (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal TotalML_Year { get; set; }

        /// <summary>
        ///  Total Movimiento en el año (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal TotalME_Year { get; set; }
        
        #endregion
    }
}
