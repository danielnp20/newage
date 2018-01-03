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
    /// Class Programación Pagos:
    /// Models DTO_ProgramacionPagos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ProgramacionPagos
    {
        #region DTO_ProgramacionPagos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="dr"></param>
        public DTO_ProgramacionPagos(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoTercero.Value = dr["DocumentoTercero"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.SaldoML.Value = Convert.ToDecimal(dr["SaldoML"]);
                this.SaldoME.Value = Convert.ToDecimal(dr["SaldoME"]);
                this.PagoInd.Value = dr["PagoInd"] != DBNull.Value ? Convert.ToBoolean(dr["PagoInd"]) : false;
                if (!string.IsNullOrWhiteSpace(dr["PagoIndInicial"].ToString()))
                    this.PagoIndInicial.Value = Convert.ToBoolean(dr["PagoIndInicial"]);
                else
                    this.PagoIndInicial.Value = false;
                if (!string.IsNullOrWhiteSpace(dr["BancoCuentaID"].ToString()))
                    this.BancoCuentaID.Value = dr["BancoCuentaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ValorPago"].ToString()))
                    this.ValorPago.Value = Convert.ToDecimal(dr["ValorPago"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteIdNro"].ToString()))
                    this.ComprobanteIdNro.Value = Convert.ToInt32(dr["ComprobanteIdNro"]);
                this.Observacion.Value = dr["Observacion"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ProgramacionPagos()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.TerceroID = new UDT_TerceroID();
            this.Descriptivo = new UDT_DescripTBase();
            this.MonedaID = new UDT_MonedaID();
            this.SaldoML = new UDT_Valor();
            this.SaldoME = new UDT_Valor();
            this.PagoInd = new UDT_SiNo();
            this.PagoIndInicial = new UDT_SiNo();
            this.BancoCuentaID = new UDT_BancoCuentaID();
            this.ValorPago = new UDT_Valor();
            this.Fecha = new UDTSQL_smalldatetime();
            this.ComprobanteIdNro = new UDTSQL_int();
            this.Observacion = new UDT_DescripTExt();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_Valor SaldoML { get; set; }

        [DataMember]
        public UDT_Valor SaldoME { get; set; }

        [DataMember]
        public UDT_SiNo PagoInd { get; set; }

        [DataMember]
        public UDT_SiNo PagoIndInicial { get; set; }

        [DataMember]
        public UDT_BancoCuentaID BancoCuentaID { get; set; }

        [DataMember]
        public UDT_Valor ValorPago { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDTSQL_int ComprobanteIdNro { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        #endregion
    }
}
