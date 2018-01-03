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
    public class DTO_ReportLiquidacionCredito
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportLiquidacionCredito(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"]);
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.nombreCliente.Value = dr["nombreCliente"].ToString();
                this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrEmpty(dr["VlrCuotaSeguro"].ToString()))
                    this.VlrCuotaSeguro.Value = Convert.ToDecimal(dr["VlrCuotaSeguro"]);
                else
                    this.VlrCuotaSeguro.Value = 0;
                this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                this.PorSeguro.Value = Convert.ToDecimal(dr["PorSeguro"]);
                this.PorInteres.Value = Convert.ToDecimal(dr["PorInteres"]);
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.PagaDesc.Value = dr["PagaDesc"].ToString();
                this.zonaDesc.Value = dr["zonaDesc"].ToString();
                this.FechaLiquida.Value = Convert.ToDateTime(dr["FechaLiquida"]);
                this.FechaLiquidaMora.Value = Convert.ToDateTime(dr["FechaLiquidaMora"]);
                this.NumCuota.Value = dr["NumCuota"].ToString();
                this.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapital"]);
                this.VlrInteres.Value = Convert.ToDecimal(dr["VlrInteres"]);
                this.VlrSeguro.Value = Convert.ToDecimal(dr["VlrSeguro"]);
                if (!string.IsNullOrEmpty(dr["VlrOtro1"].ToString()))
                    this.VlrOtro1.Value = Convert.ToDecimal(dr["VlrOtro1"]);
                if (!string.IsNullOrEmpty(dr["VlrOtro2"].ToString()))
                    this.VlrOtro2.Value = Convert.ToDecimal(dr["VlrOtro2"]);
                if (!string.IsNullOrEmpty(dr["VlrOtro3"].ToString()))
                    this.VlrOtro3.Value = Convert.ToDecimal(dr["VlrOtro3"]);
                this.VlrOtrosfijos.Value = Convert.ToDecimal(dr["VlrOtrosfijos"]);
                this.VlrSaldoCapital.Value = Convert.ToDecimal(dr["VlrSaldoCapital"]);
                this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.VlrAdicional.Value = Convert.ToDecimal(dr["VlrAdicional"]);
                this.asesor.Value = dr["asesor"].ToString();
                this.Pagare.Value = dr["Pagare"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.ConcesionarioID.Value = dr["ConcesionarioID"].ToString();
                this.ConcesionarioDesc.Value = dr["ConcesionarioDesc"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ReportLiquidacionCredito()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.Libranza = new UDT_LibranzaID();
            this.ClienteID = new UDT_ClienteID();
            this.nombreCliente = new UDT_Descriptivo();
            this.Plazo = new UDTSQL_smallint();
            this.VlrCuota = new UDT_Valor();
            this.VlrCuotaTotal = new UDT_Valor();
            this.VlrCuotaSeguro = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.PorSeguro = new UDT_PorcentajeID();
            this.PorInteres = new UDT_PorcentajeID();
            this.AsesorID = new UDT_AsesorID();
            this.PagaDesc = new UDT_Descriptivo();
            this.zonaDesc = new UDT_Descriptivo();
            this.FechaLiquida = new UDTSQL_smalldatetime();
            this.FechaLiquidaMora = new UDTSQL_smalldatetime();
            this.NumCuota = new UDTSQL_char(15);
            this.VlrCapital = new UDT_Valor();
            this.VlrInteres = new UDT_Valor();
            this.VlrSeguro = new UDT_Valor();
            this.VlrOtro1 = new UDT_Valor();
            this.VlrOtro2 = new UDT_Valor();
            this.VlrOtro3 = new UDT_Valor();
            this.VlrOtrosfijos = new UDT_Valor();
            this.VlrSaldoCapital = new UDT_Valor();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrAdicional = new UDT_Valor();
            this.asesor = new UDT_Descriptivo();
            this.Pagare = new UDTSQL_char(15);
            this.CentroCostoID = new UDT_CentroCostoID();
            this.ConcesionarioID = new UDT_CodigoGrl10();
            this.ConcesionarioDesc = new UDT_Descriptivo();

            //Componentes
            #region Componentes
            this.vlrComponete1 = new UDT_Valor();
            this.nombreCompo1 = new UDT_Descriptivo();
            this.vlrComponente2 = new UDT_Valor();
            this.nombreCompo2 = new UDT_Descriptivo();
            this.vlrComponente3 = new UDT_Valor();
            this.nombreCompo3 = new UDT_Descriptivo();
            this.vlrComponente4 = new UDT_Valor();
            this.nombreCompo4 = new UDT_Descriptivo();
            this.vlrComponente5 = new UDT_Valor();
            this.nombreCompo5 = new UDT_Descriptivo();
            this.vlrComponent6 = new UDT_Valor();
            this.nombreCompo6 = new UDT_Descriptivo();
            this.vlrComponente7 = new UDT_Valor();
            this.nombreCompo7 = new UDT_Descriptivo();
            this.vlrComponente8 = new UDT_Valor();
            this.nombreCompo8 = new UDT_Descriptivo();
            this.vlrComponente9 = new UDT_Valor();
            this.nombreCompo9 = new UDT_Descriptivo();
            this.vlrComponente10 = new UDT_Valor();
            this.nombreCompo10 = new UDT_Descriptivo();
            this.vlrComponente11 = new UDT_Valor();
            this.nombreCompo11 = new UDT_Descriptivo();
            this.vlrComponente12 = new UDT_Valor();
            this.nombreCompo12 = new UDT_Descriptivo();
            this.vlrComponente13 = new UDT_Valor();
            this.nombreCompo13 = new UDT_Descriptivo();
            this.vlrComponente14 = new UDT_Valor();
            this.nombreCompo14 = new UDT_Descriptivo();
            #endregion
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCliente { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrCuotaTotal { get; set; }

        [DataMember]
        public UDT_Valor VlrCuotaSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorSeguro { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorInteres { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_Descriptivo PagaDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo zonaDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquida { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiquidaMora { get; set; }

        [DataMember]
        public UDTSQL_char NumCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrInteres { get; set; }

        [DataMember]
        public UDT_Valor VlrSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrOtro1 { get; set; }

        [DataMember]
        public UDT_Valor VlrOtro2 { get; set; }

        [DataMember]
        public UDT_Valor VlrOtro3 { get; set; }

        [DataMember]
        public UDT_Valor VlrOtrosfijos { get; set; }

        [DataMember]
        public UDT_Valor VlrSaldoCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrAdicional { get; set; }

        [DataMember]
        public UDT_Descriptivo asesor { get; set; }

        [DataMember]
        public UDTSQL_char Pagare { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 ConcesionarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConcesionarioDesc { get; set; }

         [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        #region Componentes

        [DataMember]
        public UDT_Valor vlrComponete1 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo1 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente2 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo2 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente3 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo3 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente4 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo4 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente5 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo5 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponent6 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo6 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente7 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo7 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente8 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo8 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente9 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo9 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente10 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo10 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente11 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo11 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente12 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo12 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente13 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo13 { get; set; }

        [DataMember]
        public UDT_Valor vlrComponente14 { get; set; }

        [DataMember]
        public UDT_Descriptivo nombreCompo14 { get; set; }

        #endregion


        #endregion

    }
}
