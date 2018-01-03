using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.Negocio.Reportes;
using System.Data;

namespace NewAge.DTO.Reportes
{
    [DataContract]
    [Serializable]

    /// <summary>
    /// Clase del reporte Balance De Prueba
    /// </summary>
    public class DTO_ReportBalanceDePrueba : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportBalanceDePrueba(IDataReader dr)
        {
            //MonthMax = new DateTime(Convert.ToInt32(dr["Año"]),Convert.ToInt32(dr["MesMax"]),1);
            //MonthMin = new DateTime(Convert.ToInt32(dr["Año"]),Convert.ToInt32(dr["MesMin"]),1);
            //YearID = Convert.ToInt32(dr["Año"]);
            //MonthMax = Convert.ToInt32(dr["MesMax"]);
            //MonthMin = Convert.ToInt32(dr["MesMin"]);
            CuentaID = dr["CuentaID"].ToString();
            CuentaDesc = dr["CuentaDesc"].ToString();
            MaxLengthInd = Convert.ToInt16(dr["MaxLengthInd"]);
            CentroCostoID = dr["CentroCostoID"].ToString();
            CentroCostoDesc = dr["CentroCostoDesc"].ToString();
            ProyectoID = dr["ProyectoID"].ToString();
            ProyectoDesc = dr["ProyectoDesc"].ToString();
            LineaPresupuestoID = dr["LineaPresupuestoID"].ToString();
            LineaPresupuestoDesc = dr["LineaPresupuestoDesc"].ToString();
            Signo = Convert.ToInt16(dr["Signo"]);
            DebitoML = Convert.ToDecimal(dr["DebitoML"]);
            CreditoML = Convert.ToDecimal(dr["CreditoML"]);
            DebitoME = Convert.ToDecimal(dr["DebitoME"]);
            CreditoME = Convert.ToDecimal(dr["CreditoME"]);
            InicialML = Convert.ToDecimal(dr["InicialML"]);
            FinalML = Convert.ToDecimal(dr["FinalML"]);
            InicialME = Convert.ToDecimal(dr["InicialME"]);
            FinalME = Convert.ToDecimal(dr["FinalME"]);
            MovimientoML = Convert.ToDecimal(dr["MovimientoML"]);
            MovimientoME = Convert.ToDecimal(dr["MovimientoME"]);
        }
 
        #region Propiedades
        //[DataMember]
        //public DateTime MonthMax { get; set; }

        //[DataMember]
        //public DateTime MonthMin { get; set; }

        //[DataMember]
        //public int YearID { get; set; }

        //[DataMember]
        //public int MonthMax { get; set; }

        //[DataMember]
        //public int MonthMin { get; set; }

        /// <summary>
        /// Cuenta ID 
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
        /// CentroCosto ID 
        /// </summary>
        [DataMember]
        public string CentroCostoID { get; set; }

        /// <summary>
        /// Descripcion del CentroCosto
        /// </summary>
        [DataMember]
        public string CentroCostoDesc { get; set; }

        /// <summary>
        /// Proyecto ID
        /// </summary>
        [DataMember]
        public string ProyectoID { get; set; }

        /// <summary>
        /// Descripcion del Proyecto
        /// </summary>
        [DataMember]
        public string ProyectoDesc { get; set; }

        /// <summary>
        /// LineaPresupuesto ID
        /// </summary>
        [DataMember]
        public string LineaPresupuestoID { get; set; }

        /// <summary>
        /// Descripcion del LineaPresupuesto
        /// </summary>
        [DataMember]
        public string LineaPresupuestoDesc { get; set; }

        /// <summary>
        /// Naturaleza de la cuenta
        /// </summary>
        [DataMember]
        public int Signo { get; set; }

        /// <summary>
        /// Debito (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal DebitoML { get; set; }

        /// <summary>
        /// Credito (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal CreditoML { get; set; }

        /// <summary>
        /// Debito (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal DebitoME { get; set; }

        /// <summary>
        /// Credito (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal CreditoME { get; set; }

        /// <summary>
        /// Saldo Inicial (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal InicialML { get; set; }

        /// <summary>
        /// Saldo Final (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal FinalML { get; set; }

        /// <summary>
        /// Saldo Inicial  (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal InicialME { get; set; }

        /// <summary>
        /// Saldo Final  (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal FinalME { get; set; }

        /// <summary>
        /// Movimiento (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal MovimientoML { get; set; }

        /// <summary>
        /// Movimiento (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal MovimientoME { get; set; }

        #endregion

    }
}
