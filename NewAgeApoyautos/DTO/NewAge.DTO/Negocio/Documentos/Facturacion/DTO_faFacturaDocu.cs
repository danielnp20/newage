using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Attributes;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// DTO Tabla DTO_faFacturaDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faFacturaDocu
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faFacturaDocu(IDataReader dr)
        {
            InitCols();
            try
            {
                this.Seleccionar.Value = false;
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.AsesorID.Value = Convert.ToString(dr["AsesorID"]);
                this.DocumentoREL.Value = Convert.ToInt32(dr["DocumentoREL"]);
                if (!string.IsNullOrWhiteSpace(dr["NotaEnvioREL"].ToString()))
                    this.NotaEnvioREL.Value = Convert.ToInt32(dr["NotaEnvioREL"]);
                this.FacturaREL.Value = Convert.ToInt32(dr["FacturaREL"]);
                this.MonedaPago.Value = Convert.ToString(dr["MonedaPago"]);
                this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                if (!string.IsNullOrWhiteSpace(dr["ContratoID"].ToString()))
                    this.ContratoID.Value = Convert.ToString(dr["ContratoID"]);
                this.ListaPrecioID.Value = Convert.ToString(dr["ListaPrecioID"]);
                this.ZonaID.Value = Convert.ToString(dr["ZonaID"]);
                this.FacturaTipoID.Value = Convert.ToString(dr["FacturaTipoID"]);
                this.TasaPago.Value = Convert.ToByte(dr["TasaPago"]);
                this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
                this.ObservacionENC.Value = Convert.ToString(dr["ObservacionENC"]);
                this.ObservacionPIE.Value = Convert.ToString(dr["ObservacionPIE"]);
                this.FormaPago.Value = Convert.ToString(dr["FormaPago"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.Iva.Value = Convert.ToDecimal(dr["Iva"]);
                this.Bruto.Value = Convert.ToDecimal(dr["Bruto"]);
                if (!string.IsNullOrWhiteSpace(dr["Porcentaje1"].ToString()))
                    this.Porcentaje1.Value = Convert.ToDecimal(dr["Porcentaje1"]);
                if (!string.IsNullOrWhiteSpace(dr["Porcentaje2"].ToString()))
                     this.Porcentaje2.Value = Convert.ToDecimal(dr["Porcentaje2"]);
                if (!string.IsNullOrWhiteSpace(dr["PorcPtoPago"].ToString()))
                     this.PorcPtoPago.Value = Convert.ToDecimal(dr["PorcPtoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["PorcRteGarantia"].ToString()))
                    this.PorcRteGarantia.Value = Convert.ToDecimal(dr["PorcRteGarantia"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPtoPago"].ToString()))
                     this.FechaPtoPago.Value = Convert.ToDateTime(dr["FechaPtoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorPtoPago"].ToString()))
                     this.ValorPtoPago.Value = Convert.ToDecimal(dr["ValorPtoPago"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion1"].ToString()))
                     this.Retencion1.Value = Convert.ToDecimal(dr["Retencion1"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion2"].ToString()))
                     this.Retencion2.Value = Convert.ToDecimal(dr["Retencion3"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion3"].ToString()))
                     this.Retencion3.Value = Convert.ToDecimal(dr["Retencion3"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion4"].ToString()))
                     this.Retencion4.Value = Convert.ToDecimal(dr["Retencion4"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion5"].ToString()))
                     this.Retencion5.Value = Convert.ToDecimal(dr["Retencion5"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion6"].ToString()))
                     this.Retencion6.Value = Convert.ToDecimal(dr["Retencion6"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion7"].ToString()))
                     this.Retencion7.Value = Convert.ToDecimal(dr["Retencion7"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion8"].ToString()))
                     this.Retencion8.Value = Convert.ToDecimal(dr["Retencion8"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion9"].ToString()))
                     this.Retencion9.Value = Convert.ToDecimal(dr["Retencion9"]);
                if (!string.IsNullOrWhiteSpace(dr["Retencion10"].ToString()))
                     this.Retencion10.Value = Convert.ToDecimal(dr["Retencion10"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd1"].ToString()))
                     this.DatoAdd1.Value = Convert.ToString(dr["DatoAdd1"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd2"].ToString()))
                     this.DatoAdd2.Value = Convert.ToString(dr["DatoAdd2"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd3"].ToString()))
                     this.DatoAdd3.Value = Convert.ToString(dr["DatoAdd3"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd4"].ToString()))
                     this.DatoAdd4.Value = Convert.ToString(dr["DatoAdd4"]);
                if (!string.IsNullOrWhiteSpace(dr["DatoAdd5"].ToString()))
                     this.DatoAdd5.Value = Convert.ToString(dr["DatoAdd5"]);
                this.FacturaFijaInd.Value = Convert.ToBoolean(dr["FacturaFijaInd"]);
                this.RteGarantiaIvaInd.Value = Convert.ToBoolean(dr["RteGarantiaIvaInd"]);
                if (!string.IsNullOrWhiteSpace(dr["PorcAnticipo"].ToString()))
                    this.PorcAnticipo.Value = Convert.ToDecimal(dr["PorcAnticipo"]);
                else
                    this.PorcAnticipo.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["IncluyeIVAInd"].ToString()))
                    this.IncluyeIVAInd.Value = Convert.ToBoolean(dr["IncluyeIVAInd"]);
                else
                    this.IncluyeIVAInd.Value = false;
                
            }
            catch (Exception e)
            { 
                throw e; 
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faFacturaDocu()
        {
            InitCols();
            this.ValorAnticipo = 0;
            this.ValorRteGarantia = 0;
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Seleccionar = new UDT_SiNo();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.AsesorID = new UDT_AsesorID();
            //this.MvtoTipoCarID = new UDT_MvtoTipoID();
            this.DocumentoREL = new UDT_Consecutivo();
            this.NotaEnvioREL = new UDT_Consecutivo();
            this.FacturaREL = new UDT_Consecutivo();
            this.ClienteID = new UDT_ClienteID();
            this.ContratoID = new UDT_ContratoID();
            this.ListaPrecioID = new UDT_ListaPrecioID();
            this.ZonaID = new UDT_ZonaID();
            this.FacturaTipoID = new UDT_FacturaTipoID();
            this.Porcentaje1 = new UDT_PorcentajeID();
            this.Porcentaje2 = new UDT_PorcentajeID();
            this.MonedaPago = new UDT_MonedaID();
            this.TasaPago = new UDTSQL_tinyint();
            this.FechaVto = new UDTSQL_smalldatetime();
            this.ObservacionENC = new UDT_DescripTExt();
            this.ObservacionPIE = new UDT_DescripTExt();
            this.FormaPago = new UDT_DescripTBase();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.Bruto = new UDT_Valor();
            this.PorcPtoPago = new UDT_PorcentajeID();
            this.PorcRteGarantia = new UDT_PorcentajeID();
            this.FechaPtoPago = new UDTSQL_smalldatetime();
            this.ValorPtoPago = new UDT_Valor();
            this.Retencion1 = new UDT_Valor();
            this.Retencion2 = new UDT_Valor();
            this.Retencion3 = new UDT_Valor();
            this.Retencion4 = new UDT_Valor();
            this.Retencion5 = new UDT_Valor();
            this.Retencion6 = new UDT_Valor();
            this.Retencion7 = new UDT_Valor();
            this.Retencion8 = new UDT_Valor();
            this.Retencion9 = new UDT_Valor();
            this.Retencion10 = new UDT_Valor();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.FacturaFijaInd = new UDT_SiNo();
            this.RteGarantiaIvaInd = new UDT_SiNo();
            this.PorcAnticipo = new UDT_Valor();
            this.IncluyeIVAInd = new UDT_SiNo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_SiNo Seleccionar { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoREL { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Consecutivo NotaEnvioREL { get; set; }
                
        [DataMember]
        public UDT_Consecutivo FacturaREL { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_ContratoID ContratoID { get; set; }

        [DataMember]
        public UDT_ListaPrecioID ListaPrecioID { get; set; }

        [DataMember]
        public UDT_ZonaID ZonaID { get; set; }

        [DataMember]
        public UDT_FacturaTipoID FacturaTipoID { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_PorcentajeID Porcentaje1 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_PorcentajeID Porcentaje2 { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaPago { get; set; }

        [DataMember]
        public UDTSQL_tinyint TasaPago { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVto { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt ObservacionENC { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt ObservacionPIE { get; set; }

        [DataMember]
        public UDT_DescripTBase FormaPago { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_Valor Bruto { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_PorcentajeID PorcPtoPago { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_PorcentajeID PorcRteGarantia { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_smalldatetime FechaPtoPago { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor ValorPtoPago { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion1 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion2 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion3 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion4 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion5 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion6 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion7 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion8 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion9 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor Retencion10 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo FacturaFijaInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo RteGarantiaIvaInd { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Valor PorcAnticipo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo IncluyeIVAInd { get; set; }

        /// <summary>
        /// Campos extra
        /// </summary>
        [DataMember]
        [NotImportable]
        public decimal VlrBrutoTotal { get; set; }

        [DataMember]
        [NotImportable]
        public decimal VlrIvaTotal { get; set; }

        [DataMember]
        [NotImportable]
        public decimal VlrNetoTotal { get; set; }

        [DataMember]
        [NotImportable]
        public decimal ValorAnticipo { get; set; }

        [DataMember]
        [NotImportable]
        public decimal ValorRteGarantia { get; set; }

        [DataMember]
        [NotImportable]
        public string CuentaRteICA { get; set; }

        #endregion
    }
}
