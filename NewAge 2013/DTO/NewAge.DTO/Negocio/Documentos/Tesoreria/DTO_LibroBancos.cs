using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using System.Data;

namespace NewAge.DTO.Negocio
{

    /// <summary>
    /// Class Pago Facturas:
    /// Models DTO_LibroBancos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_LibroBancos
    {
        #region DTO_LibroBancos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_LibroBancos(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["BancoCuentaId"].ToString()))
                    this.BancoCuentaId.Value = (dr["BancoCuentaId"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = (dr["Descriptivo"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["CuentaId"].ToString()))
                    this.CuentaId.Value = (dr["CuentaId"]).ToString();
                if (!string.IsNullOrWhiteSpace(dr["SaldoIniLoc"].ToString()))
                    this.SaldoIniLoc.Value = Convert.ToDecimal(dr["SaldoIniLoc"]);
                if (!string.IsNullOrWhiteSpace(dr["SaldoIniExt"].ToString()))
                    this.SaldoIniExt.Value = Convert.ToDecimal(dr["SaldoIniExt"]);
                if (!string.IsNullOrWhiteSpace(dr["IdentificadorTr"].ToString()))
                    this.IdentificadorTr.Value = Convert.ToInt32(dr["IdentificadorTr"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_LibroBancos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
           this.BancoCuentaId = new UDT_BancoCuentaID();
            this.Descriptivo = new UDT_Descriptivo();
            this.CuentaId = new UDT_CuentaID();
            this.SaldoIniLoc = new UDT_Valor();
            this.SaldoIniExt = new UDT_Valor();
            this.IdentificadorTr = new UDT_Consecutivo();

            this.FechaIni = new UDTSQL_datetime();
            this.FechaFin = new UDTSQL_datetime();
        }

        #endregion

        #region Propiedades

       
        [DataMember]
        public UDT_BancoCuentaID BancoCuentaId { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaId { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniLoc { get; set; }

        [DataMember]
        public UDT_Valor SaldoIniExt { get; set; }

        [DataMember]
        public UDT_Consecutivo IdentificadorTr { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaIni { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaFin { get; set; }

        [DataMember]
        public List<DTO_LibroBancosDeta> Detalle { get; set; }

        #endregion
    }
}
