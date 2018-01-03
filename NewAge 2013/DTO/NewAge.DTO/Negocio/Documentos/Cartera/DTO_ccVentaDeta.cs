using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccVentaDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccVentaDeta
    {
        #region DTO_ccVentaDeta

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccVentaDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocSustituye"].ToString()))
                    this.NumDocSustituye.Value = Convert.ToInt32(dr["NumDocSustituye"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocRecompra"].ToString()))
                    this.NumDocRecompra.Value = Convert.ToInt32(dr["NumDocRecompra"]);
                this.Portafolio.Value = dr["Portafolio"].ToString();
                this.CuotaID.Value = Convert.ToInt32(dr["CuotaID"]);
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.CuotasVend.Value = Convert.ToInt32(dr["CuotasVend"]);
                this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                this.VlrVenta.Value = Convert.ToDecimal(dr["VlrVenta"]);
                this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSustLibranza"].ToString()))
                    this.VlrSustLibranza.Value = Convert.ToDecimal(dr["VlrSustLibranza"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrSustRecompra"].ToString()))
                    this.VlrSustRecompra.Value = Convert.ToDecimal(dr["VlrSustRecompra"]);
                this.VlrNeto.Value = Convert.ToInt32(dr["VlrNeto"]);
                if (!string.IsNullOrWhiteSpace(dr["CompradorFinal"].ToString()))
                    this.CompradorFinal.Value = dr["CompradorFinal"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VlrTotalDerechos"].ToString()))
                    this.VlrTotalDerechos.Value = Convert.ToInt32(dr["VlrTotalDerechos"]);
                else
                    this.VlrTotalDerechos.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["VlrProvGeneral"].ToString()))
                    this.VlrProvGeneral.Value = Convert.ToInt32(dr["VlrProvGeneral"]);
                else
                    this.VlrProvGeneral.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["VlrProvComprador"].ToString()))
                    this.VlrProvComprador.Value = Convert.ToInt32(dr["VlrProvComprador"]);
                else
                    this.VlrProvComprador.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["VlrSdoCapital"].ToString()))
                    this.VlrSdoCapital.Value = Convert.ToInt32(dr["VlrSdoCapital"]);
                else
                    this.VlrSdoCapital.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["VlrSdoAsistencias"].ToString()))
                    this.VlrSdoAsistencias.Value = Convert.ToInt32(dr["VlrSdoAsistencias"]);
                else
                    this.VlrSdoAsistencias.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["VlrSdoOtros"].ToString()))
                    this.VlrSdoOtros.Value = Convert.ToInt32(dr["VlrSdoOtros"]); 
                else
                    this.VlrSdoOtros.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccVentaDeta()
        {
            this.InitCols();            
            this.VlrTotalDerechos.Value = 0;
            this.VlrProvGeneral.Value = 0;
            this.VlrProvComprador.Value = 0;
            this.VlrSdoCapital.Value = 0;
            this.VlrSdoAsistencias.Value = 0;
            this.VlrSdoOtros.Value = 0;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumDocSustituye = new UDT_Consecutivo();
            this.NumDocRecompra = new UDT_Consecutivo();
            this.Portafolio = new UDTSQL_char(5);
            this.CuotaID = new UDT_CuotaID();
            this.VlrCuota = new UDT_Valor();
            this.CuotasVend = new UDTSQL_int();
            this.VlrLibranza = new UDT_Valor();
            this.VlrVenta = new UDT_Valor();
            this.FactorCesion = new UDT_TasaID();
            this.VlrSustLibranza = new UDT_Valor();
            this.VlrSustRecompra = new UDT_Valor();
            this.VlrNeto = new UDT_Valor();
            this.CompradorFinal = new UDT_CompradorCarteraID();
            this.VlrTotalDerechos = new UDT_Valor();
            this.VlrProvGeneral = new UDT_Valor();
            this.VlrProvComprador = new UDT_Valor();
            this.VlrSdoCapital = new UDT_Valor();
            this.VlrSdoAsistencias = new UDT_Valor();
            this.VlrSdoOtros = new UDT_Valor();
            //Campos Adicionales
            this.Aprobado = new UDT_SiNo();
            this.AsignaCarteraInd = new UDT_SiNo();
            this.ClienteID = new UDT_ClienteID();
            this.Libranza = new UDT_LibranzaID();
            this.FechaCuota1 = new UDTSQL_smalldatetime();
            this.FechaFondeoCartera = new UDTSQL_smalldatetime();
            this.Nombre = new UDT_DescripTBase();
            this.Observacion = new UDT_Descriptivo();
            this.Rechazado = new UDT_SiNo();
            this.VlrRecompra = new UDT_Valor();
            this.VlrUtilidad = new UDT_Valor();
            this.IsPreventa = new UDT_SiNo();

        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_DescripTBase Nombre { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorFinal { get; set; }

        [DataMember]
        public UDTSQL_int CuotasVend { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_Valor VlrVenta { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCuota1 { get; set; }

        #region Campos Propios
        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocSustituye { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumDocRecompra { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_char Portafolio { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CuotaID CuotaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_TasaID FactorCesion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrTotalDerechos { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrSustLibranza { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrSustRecompra { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrNeto { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrProvGeneral { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrProvComprador { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrSdoCapital { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrSdoAsistencias { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrSdoOtros { get; set; }

        #endregion

        #region Campos Adicionales

        [DataMember]
        [NotImportable]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo AsignaCarteraInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaFondeoCartera { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo Observacion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrRecompra { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrUtilidad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo IsPreventa { get; set; } 
        #endregion

       #endregion
    }
}
