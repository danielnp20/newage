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
    /// Clase del reporte Balance De Prueba Comparativo
    /// </summary>
    public class DTO_ReportBalanceDePruebaComparativo : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportBalanceDePruebaComparativo(IDataReader dr)
        {
             CuentaID = dr["CuentaID"].ToString();
             CuentaDesc = dr["CuentaDesc"].ToString();
             MaxLengthInd = Convert.ToInt16(dr["MaxLengthInd"]);

             MovimientoML_curr = Convert.ToDecimal(dr["MovimientoML_curr"]);
             MovimientoML_prev = Convert.ToDecimal(dr["MovimientoML_prev"]);
             Dif_MovML = (MovimientoML_prev == 0) ? "*" : (Math.Round((MovimientoML_curr - MovimientoML_prev) / MovimientoML_prev * 100)).ToString();

             MovimientoME_curr = Convert.ToDecimal(dr["MovimientoME_curr"]);
             MovimientoME_prev = Convert.ToDecimal(dr["MovimientoME_prev"]);
             Dif_MovME = (MovimientoME_prev == 0) ? "*" : (Math.Round((MovimientoME_curr - MovimientoME_prev) / MovimientoME_prev * 100)).ToString();

             FinalML_curr = Convert.ToDecimal(dr["FinalML_curr"]);
             FinalML_prev = Convert.ToDecimal(dr["FinalML_prev"]);
             Dif_FinalML = (FinalML_prev == 0) ? "*" : (Math.Round((FinalML_curr - FinalML_prev) / FinalML_prev * 100)).ToString();

             FinalME_curr = Convert.ToDecimal(dr["FinalME_curr"]);
             FinalME_prev = Convert.ToDecimal(dr["FinalME_prev"]);
             Dif_FinalME = (FinalME_prev == 0) ? "*" : (Math.Round((FinalME_curr - FinalME_prev) / FinalME_prev * 100)).ToString();
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
        /// Movimiento durante el periodo del año corriente (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal MovimientoML_curr { get; set; }

        /// <summary>
        /// Movimiento durante el periodo del año pasado (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal MovimientoML_prev { get; set; }

        /// <summary>
        /// Diferencia entre movimientos de los periodos de los años corriente y pasado (Moneda Local)
        /// </summary>
        [DataMember]
        public string  Dif_MovML { get; set; }

        /// <summary>
        /// Movimiento durante el periodo del año corriente (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal MovimientoME_curr { get; set; }

        /// <summary>
        /// Movimiento durante el periodo del año pasado (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal MovimientoME_prev { get; set; }

        /// <summary>
        /// Diferencia entre movimientos de los periodos de los años corriente y pasado (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public string Dif_MovME { get; set; }

        /// <summary>
        /// Saldo final del periodo del año corriente (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal FinalML_curr { get; set; }

        /// <summary>
        /// Saldo final del periodo del año pasado (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal FinalML_prev { get; set; }

        /// <summary>
        /// Diferencia entre Saldos finales de los periodos de los años corriente y pasado (Moneda Local)
        /// </summary>
        [DataMember]
        public string Dif_FinalML { get; set; }

        /// <summary>
        /// Saldo final del periodo del año corriente (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal FinalME_curr { get; set; }

        /// <summary>
        /// Saldo final del periodo del año pasado (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal FinalME_prev { get; set; }

        /// <summary>
        /// Diferencia entre Saldos finales de los periodos de los años corriente y pasado (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public string Dif_FinalME { get; set; }

        #endregion
    }
}

