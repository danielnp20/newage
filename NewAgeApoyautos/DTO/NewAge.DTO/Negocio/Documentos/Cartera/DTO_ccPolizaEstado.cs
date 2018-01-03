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
    /// Models DTO_ccPolizaEstado.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccPolizaEstado : DTO_SerializedObject
    {
        #region DTO_ccPolizaEstado

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccPolizaEstado(IDataReader dr, bool getLibranza = false)
        {
            InitCols();
            try
            {
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.Poliza.Value = dr["Poliza"].ToString();
                this.AseguradoraID.Value = dr["AseguradoraID"].ToString();
                this.SegurosAsesorID.Value = dr["SegurosAsesorID"].ToString();

                if (!string.IsNullOrWhiteSpace(dr["FinanciadaIND"].ToString()))
                    this.FinanciadaIND.Value = Convert.ToBoolean(dr["FinanciadaIND"]);
                if (!string.IsNullOrWhiteSpace(dr["AnuladaIND"].ToString()))
                    this.AnuladaIND.Value = Convert.ToBoolean(dr["AnuladaIND"]);

                this.EstadoPoliza.Value = Convert.ToByte(dr["EstadoPoliza"]);
                this.FechaLiqSeguro.Value = Convert.ToDateTime(dr["FechaLiqSeguro"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaPagoSeguro"].ToString()))
                    this.FechaPagoSeguro.Value = Convert.ToDateTime(dr["FechaPagoSeguro"]);
                this.FechaVigenciaINI.Value = Convert.ToDateTime(dr["FechaVigenciaINI"]);
                this.FechaVigenciaFIN.Value = Convert.ToDateTime(dr["FechaVigenciaFin"]);
                this.VlrPoliza.Value = Convert.ToDecimal(dr["VlrPoliza"]);
                this.NCRevoca.Value = dr["NCRevoca"].ToString();

                if (!string.IsNullOrWhiteSpace(dr["NumDocSolicitud"].ToString()))
                    this.NumDocSolicitud.Value = Convert.ToInt32(dr["NumDocSolicitud"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocCredito"].ToString()))
                    this.NumDocCredito.Value = Convert.ToInt32(dr["NumDocCredito"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocLiquida"].ToString()))
                    this.NumeroDocLiquida.Value = Convert.ToInt32(dr["NumeroDocLiquida"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorFinancia"].ToString()))
                    this.ValorFinancia.Value = Convert.ToDecimal(dr["ValorFinancia"]);
                if (!string.IsNullOrWhiteSpace(dr["PlazoFinancia"].ToString())) 
                    this.PlazoFinancia.Value = Convert.ToInt16(dr["PlazoFinancia"]);
                if (!string.IsNullOrWhiteSpace(dr["Cuota1Financia"].ToString())) 
                    this.Cuota1Financia.Value = Convert.ToInt16(dr["Cuota1Financia"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuotaFinancia"].ToString())) 
                    this.VlrCuotaFinancia.Value = Convert.ToDecimal(dr["VlrCuotaFinancia"]);
                if (!string.IsNullOrWhiteSpace(dr["TasaFinancia"].ToString())) 
                    this.TasaFinancia.Value = Convert.ToDecimal(dr["TasaFinancia"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDocPago"].ToString())) 
                    this.NumeroDocPago.Value = Convert.ToInt32(dr["NumeroDocPago"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaRevoca"].ToString()))
                    this.FechaRevoca.Value = Convert.ToDateTime(dr["FechaRevoca"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorRevoca"].ToString())) 
                    this.ValorRevoca.Value = Convert.ToDecimal(dr["ValorRevoca"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocPagoRevoca"].ToString()))
                    this.NumDocPagoRevoca.Value = Convert.ToInt32(dr["NumDocPagoRevoca"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocEstCuenta"].ToString()))
                    this.NumDocEstCuenta.Value = Convert.ToInt32(dr["NumDocEstCuenta"]);
                if (!string.IsNullOrWhiteSpace(dr["NumDocRevoca"].ToString()))
                    this.NumDocRevoca.Value = Convert.ToInt32(dr["NumDocRevoca"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                if (!string.IsNullOrWhiteSpace(dr["ColectivaInd"].ToString()))
                    this.ColectivaInd.Value = Convert.ToBoolean(dr["ColectivaInd"]);
                //Trae la info de la libranza
                if(getLibranza)
                {
                    if (!string.IsNullOrWhiteSpace(dr["LibranzaCred"].ToString()))
                        this.Libranza.Value = Convert.ToInt32(dr["LibranzaCred"]);
                    if (!string.IsNullOrWhiteSpace(dr["LibranzaSol"].ToString()))
                        this.Solicitud.Value = Convert.ToInt32(dr["LibranzaSol"]);
                }
                if (!string.IsNullOrWhiteSpace(dr["TipoEspecial"].ToString()))
                    this.TipoEspecial.Value = Convert.ToByte(dr["TipoEspecial"]);


            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccPolizaEstado()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.EmpresaID = new UDT_BasicID();
            this.TerceroID = new UDT_TerceroID();
            this.Poliza = new UDTSQL_char(20);
            this.AseguradoraID = new UDT_CodigoGrl10();
            this.SegurosAsesorID = new UDT_CodigoGrl10();
            this.FinanciadaIND = new UDT_SiNo();
            this.AnuladaIND = new UDT_SiNo();
            this.EstadoPoliza = new UDTSQL_tinyint();
            this.FechaLiqSeguro = new UDTSQL_smalldatetime();
            this.FechaPagoSeguro = new UDTSQL_smalldatetime();
            this.FechaVigenciaINI = new UDTSQL_smalldatetime();
            this.FechaVigenciaFIN = new UDTSQL_smalldatetime();
            this.NumDocSolicitud = new UDT_Consecutivo();
            this.NumDocCredito = new UDT_Consecutivo();
            this.NumeroDocLiquida = new UDT_Consecutivo();
            this.VlrPoliza = new UDT_Valor();
            this.ValorFinancia = new UDT_Valor();
            this.PlazoFinancia = new UDTSQL_smallint();
            this.Cuota1Financia = new UDTSQL_smallint();
            this.VlrCuotaFinancia = new UDT_Valor();
            this.TasaFinancia = new UDT_PorcentajeCarteraID();
            this.NumeroDocPago = new UDT_Consecutivo();
            this.NCRevoca = new UDTSQL_char(20);
            this.FechaRevoca = new UDTSQL_smalldatetime();
            this.ValorRevoca = new UDT_Valor();
            this.NumDocPagoRevoca = new UDT_Consecutivo();
            this.NumDocRevoca = new UDT_Consecutivo();
            this.NumDocEstCuenta = new UDT_Consecutivo();
            this.Consecutivo = new UDT_Consecutivo();
            this.ColectivaInd = new UDT_SiNo();
            //campos Extra
            this.PagarInd = new UDT_SiNo();
            this.Solicitud = new UDT_Consecutivo();
            this.Libranza = new UDT_Consecutivo();
            this.Nombre = new UDT_Descriptivo();
            this.FechaMvto = new UDTSQL_smalldatetime();
            this.TipoEspecial = new UDTSQL_tinyint();
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_BasicID EmpresaID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDTSQL_char Poliza { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 AseguradoraID { get; set; }

        [DataMember]
        public UDT_CodigoGrl10 SegurosAsesorID { get; set; }

        [DataMember]
        public UDT_SiNo FinanciadaIND { get; set; }

        [DataMember]
        public UDT_SiNo AnuladaIND { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoPoliza { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaLiqSeguro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaPagoSeguro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVigenciaINI { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVigenciaFIN { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocSolicitud { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocCredito { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocLiquida { get; set; }

        [DataMember]
        public UDT_Valor VlrPoliza { get; set; }

        [DataMember]
        public UDT_Valor ValorFinancia { get; set; }

        [DataMember]
        public UDTSQL_smallint PlazoFinancia { get; set; }

        [DataMember]
        public UDTSQL_smallint Cuota1Financia { get; set; }

        [DataMember]
        public UDT_Valor VlrCuotaFinancia { get; set; }

        [DataMember]
        public UDT_PorcentajeCarteraID TasaFinancia { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocPago { get; set; }

        [DataMember]
        public UDTSQL_char NCRevoca { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaRevoca { get; set; }

        [DataMember]
        public UDT_Valor ValorRevoca { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocPagoRevoca { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocEstCuenta { get; set; }

        [DataMember]
        public UDT_Consecutivo NumDocRevoca { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_SiNo ColectivaInd { get; set; }

        //Campos Extra

        [DataMember]
        public UDT_SiNo PagarInd { get; set; }

        [DataMember]
        public UDT_Consecutivo Solicitud { get; set; }

        [DataMember]
        public UDT_Consecutivo Libranza { get; set; }

        [DataMember]
        public UDT_Descriptivo Nombre { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaMvto { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEspecial { get; set; }

        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }
        

        #endregion
    }
}
