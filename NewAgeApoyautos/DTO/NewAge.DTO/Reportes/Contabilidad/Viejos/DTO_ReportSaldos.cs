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
    /// Clase del Reporte Saldos
    /// </summary>
    public class DTO_ReportSaldos1 : DTO_BasicReport
    {
        /// <summary>
        /// Constructor con DataReader
        /// </summary>
        public DTO_ReportSaldos1(IDataReader dr)
        {
            PeriodoID = Convert.ToDateTime(dr["PeriodoID"]);
            CuentaID = dr["CuentaID"].ToString();
            CuentaDesc = dr["CuentaDesc"].ToString();
            TerceroID = dr["TerceroID"].ToString();
            TerceroDesc = dr["TerceroDesc"].ToString();
            ProyectoID = dr["ProyectoID"].ToString();
            ProyectoDesc = dr["ProyectoDesc"].ToString();
            CentroCostoID = dr["CentroCostoID"].ToString();
            CentroCostoDesc = dr["CentroCostoDesc"].ToString();
            LineaPresupuestoID = dr["LineaPresupuestoID"].ToString();
            LineaPresupuestoDesc = dr["LineaPresupuestoDesc"].ToString();
            Signo = Convert.ToInt16(dr["Signo"]);
            switch (Convert.ToInt32(dr["SaldoControl"]))
            {
                case (int)SaldoControl.Doc_Interno:
                    DocumentoID = (dr["DocumentoPrefijo"].ToString()).Trim() + (dr["DocumentoNumero"].ToString()).Trim();
                    break;
                case (int)SaldoControl.Doc_Externo:
                    DocumentoID = (dr["DocumentoTercero"].ToString()).Trim();
                    break;
                default:
                    DocumentoID = " - ";
                    break;
            };
            DocumentoDesc = "";
            BalanceTipoID = dr["BalanceTipoID"].ToString();
            DebitoML = Convert.ToDecimal(dr["DebitoML"]);
            CreditoML = Convert.ToDecimal(dr["CreditoML"]);
            DebitoME = Convert.ToDecimal(dr["DebitoME"]);
            CreditoME = Convert.ToDecimal(dr["CreditoME"]);
            InicialML = Convert.ToDecimal(dr["InicialML"]);
            InicialME = Convert.ToDecimal(dr["InicialME"]);
            FinalML = Convert.ToDecimal(dr["FinalML"]);
            FinalME = Convert.ToDecimal(dr["FinalME"]);
        }

        #region Propiedades
        /// <summary>
        /// Periodo del reporte
        /// </summary>
        [DataMember]
        public DateTime PeriodoID { get; set; }

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
        /// Tercero ID
        /// </summary>
        [DataMember]
        public string TerceroID { get; set; }

        /// <summary>
        /// Descripcion del Tercero
        /// </summary>
        [DataMember]
        public string TerceroDesc { get; set; }

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
        /// LineaPresupuesto ID
        /// </summary>
        [DataMember]
        public string LineaPresupuestoID { get; set; }

        /// <summary>
        /// Descripcion de la Linea Presupuesto
        /// </summary>
        [DataMember]
        public string LineaPresupuestoDesc { get; set; }

        /// <summary>
        /// Naturaleza de la cuenta
        /// </summary>
        [DataMember]
        public int Signo { get; set; }

        /// <summary>
        /// Documento ID
        /// </summary>
        [DataMember]
        public string DocumentoID { get; set; }

        /// <summary>
        /// Descripcion del Documento
        /// </summary>
        [DataMember]
        public string DocumentoDesc { get; set; } 

        /// <summary>
        /// Tipo del balance
        /// </summary>
        [DataMember]
        public string BalanceTipoID { get; set; }

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
        /// Saldo Inicial (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal InicialME { get; set; }

        /// <summary>
        /// Saldo Final (Moneda Local)
        /// </summary>
        [DataMember]
        public decimal FinalML { get; set; }

        /// <summary>
        /// Saldo Final (Moneda Extranjera)
        /// </summary>
        [DataMember]
        public decimal FinalME { get; set; }
        #endregion

    }
}