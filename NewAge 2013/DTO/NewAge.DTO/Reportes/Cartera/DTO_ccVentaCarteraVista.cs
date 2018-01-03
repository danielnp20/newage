using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_ccVentaCarteraVista
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccVentaCarteraVista(IDataReader dr)
        {
            this.InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["CuotaID"].ToString()))
                    this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                if (!string.IsNullOrEmpty(dr["Libranza"].ToString()))
                    this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                if (!string.IsNullOrEmpty(dr["Plazo"].ToString()))
                    this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                if (!string.IsNullOrEmpty(dr["CuotasFlujo"].ToString()))
                    this.CuotasFlujo.Value = Convert.ToInt32(dr["CuotasFlujo"]);
                if (!string.IsNullOrEmpty(dr["AlturaFlujo"].ToString()))
                    this.AlturaFlujo.Value = Convert.ToInt32(dr["AlturaFlujo"]);
                if (!string.IsNullOrEmpty(dr["CuotasVEN"].ToString()))
                    this.CuotasVEN.Value = Convert.ToInt32(dr["CuotasVEN"]);
                if (!string.IsNullOrEmpty(dr["CapitalVEN"].ToString()))
                    this.CapitalVEN.Value = Convert.ToDecimal(dr["CapitalVEN"]);
                if (!string.IsNullOrEmpty(dr["InteresVEN"].ToString()))
                    this.InteresVEN.Value = Convert.ToDecimal(dr["InteresVEN"]);
                if (!string.IsNullOrEmpty(dr["CapitalSDO"].ToString()))
                    this.CapitalSDO.Value = Convert.ToDecimal(dr["CapitalSDO"]);
                if (!string.IsNullOrEmpty(dr["InteresSDO"].ToString()))
                    this.InteresSDO.Value = Convert.ToDecimal(dr["InteresSDO"]);
                if (!string.IsNullOrEmpty(dr["NumeroDocPrepago"].ToString()))
                    this.NumeroDocPrepago.Value = Convert.ToInt32(dr["NumeroDocPrepago"]);
                if (!string.IsNullOrEmpty(dr["Categoria"].ToString()))
                    this.Categoria.Value = dr["Categoria"].ToString();
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrEmpty(dr["EC_Fecha"].ToString()))
                    this.EC_Fecha.Value = Convert.ToDateTime(dr["EC_Fecha"]);
                if (!string.IsNullOrEmpty(dr["SaldoFlujo"].ToString()))
                    this.SaldoFlujo.Value = Convert.ToDecimal(dr["SaldoFlujo"]);
                if (!string.IsNullOrEmpty(dr["SaldoFlujoCAP"].ToString()))
                    this.SaldoFlujoCAP.Value = Convert.ToDecimal(dr["SaldoFlujoCAP"]);
                if (!string.IsNullOrEmpty(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrEmpty(dr["CuotasSDO"].ToString()))
                    this.CuotasSDO.Value = Convert.ToInt32(dr["CuotasSDO"]);
                if (!string.IsNullOrEmpty(dr["VlrVenta"].ToString()))
                    this.VlrVenta.Value = Convert.ToDecimal(dr["VlrVenta"]);
                if (!string.IsNullOrEmpty(dr["FactorCesion"].ToString()))
                    this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"]);
                if (!string.IsNullOrEmpty(dr["CuotasVENDeta"].ToString()))
                    this.CuotasVENDeta.Value = Convert.ToInt32(dr["CuotasVENDeta"]);
                if (!string.IsNullOrEmpty(dr["ValorRecompra"].ToString()))
                    this.ValorRecompra.Value = Convert.ToDecimal(dr["ValorRecompra"]);
                if (!string.IsNullOrEmpty(dr["VlrLibranza"].ToString()))
                    this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                if (!string.IsNullOrEmpty(dr["FechaDocRecompra"].ToString()))
                    this.FechaDocRecompra.Value = Convert.ToDateTime(dr["FechaDocRecompra"]);
                if (!string.IsNullOrEmpty(dr["FechaDocPrepago"].ToString()))
                    this.FechaDocPrepago.Value = Convert.ToDateTime(dr["FechaDocPrepago"]);

                //Otros Valores
                if (!string.IsNullOrEmpty(dr["InteresVEN"].ToString()) && !string.IsNullOrEmpty(dr["InteresVEN"].ToString()))
                    this.SdoMora.Value = CapitalVEN.Value + InteresVEN.Value;
                else
                    this.SdoMora.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccVentaCarteraVista()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuotaID = new UDT_CuotaID();
            this.Libranza = new UDT_LibranzaID();
            this.Plazo = new UDTSQL_smallint();
            this.CuotasFlujo = new UDT_CuotaID();
            this.AlturaFlujo = new UDT_CuotaID();
            this.CuotasVEN = new UDT_CuotaID();
            this.CapitalSDO = new UDT_Valor();
            this.InteresSDO = new UDT_Valor();
            this.InteresVEN = new UDT_Valor();
            this.CapitalVEN = new UDT_Valor();
            this.NumeroDocPrepago = new UDT_Consecutivo();
            this.Categoria = new UDTSQL_char(2);
            this.ClienteID = new UDT_ClienteID();
            this.Descriptivo = new UDT_Descriptivo();
            this.EC_Fecha = new UDTSQL_smalldatetime();
            this.SaldoFlujo = new UDT_Valor();
            this.SaldoFlujoCAP = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.CuotasSDO = new UDT_CuotaID();
            this.VlrVenta = new UDT_Valor();
            this.FactorCesion = new UDT_TasaID();
            this.CuotasVENDeta = new UDT_CuotaID();
            this.ValorRecompra = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.FechaDocRecompra = new UDTSQL_smalldatetime();
            this.FechaDocPrepago = new UDTSQL_smalldatetime();

            //Otros Valores
            this.SdoMora = new UDT_Valor();
        }
        #endregion

        #region Propiedades
        [DataMember]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasFlujo { get; set; }

        [DataMember]
        public UDT_CuotaID AlturaFlujo { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasVEN { get; set; }

        [DataMember]
        public UDT_Valor CapitalSDO { get; set; }

        [DataMember]
        public UDT_Valor InteresSDO { get; set; }

        [DataMember]
        public UDT_Valor InteresVEN { get; set; }

        [DataMember]
        public UDT_Valor CapitalVEN { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocPrepago { get; set; }

        [DataMember]
        public UDTSQL_char Categoria { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime EC_Fecha { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujo { get; set; }

        [DataMember]
        public UDT_Valor SaldoFlujoCAP { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasSDO { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDT_TasaID FactorCesion { get; set; }

        [DataMember]
        public UDT_CuotaID CuotasVENDeta { get; set; }

        [DataMember]
        public UDT_Valor ValorRecompra { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDocRecompra { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDocPrepago { get; set; }
        #endregion

        #region Otros Valores

        [DataMember]
        public UDT_Valor SdoMora { get; set; }

        #endregion
    }
}
