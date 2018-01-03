using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class comprobante para aprobacion:
    /// Models DTO_ccSolicitudDocu
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_SolicitudAprobacionCartera
    {
        #region DTO_SolicitudAprobacionCartera

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_SolicitudAprobacionCartera(IDataReader dr, bool CodEmpleado)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["NombrePri"].ToString()))
                    this.NombreCliente.Value += dr["NombrePri"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NombreSdo"].ToString()))
                    this.NombreCliente.Value += " " + dr["NombreSdo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ApellidoPri"].ToString()))
                    this.NombreCliente.Value += " " + dr["ApellidoPri"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ApellidoSdo"].ToString()))
                    this.NombreCliente.Value += " " + dr["ApellidoSdo"].ToString();
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"].ToString());
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.DescripcionAsesor.Value = dr["DescripcionAsesor"].ToString();
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                this.DescripcionPagaduria.Value = dr["DescripcionPagaduria"].ToString();
                this.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value += " " + dr["Observacion"].ToString();
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
                this.VlrCompra.Value = Convert.ToDecimal(dr["VlrCompra"]);
                this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.VlrCupoDisponible.Value = Convert.ToDecimal(dr["VlrCupoDisponible"]);
                this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.VlrAdicional.Value = Convert.ToDecimal(dr["VlrAdicional"]);
                this.VlrDescuento.Value = Convert.ToDecimal(dr["VlrDescuento"]);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_SolicitudAprobacionCartera(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["NombrePri"].ToString()))
                    this.NombreCliente.Value += dr["NombrePri"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NombreSdo"].ToString()))
                    this.NombreCliente.Value += " " + dr["NombreSdo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ApellidoPri"].ToString()))
                    this.NombreCliente.Value += " " + dr["ApellidoPri"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ApellidoSdo"].ToString()))
                    this.NombreCliente.Value += " " + dr["ApellidoSdo"].ToString();
                this.Libranza.Value = Convert.ToInt32(dr["Libranza"].ToString());
                this.AsesorID.Value = dr["AsesorID"].ToString();
                this.DescripcionAsesor.Value = dr["DescripcionAsesor"].ToString();
                this.PagaduriaID.Value = dr["PagaduriaID"].ToString();
                this.DescripcionPagaduria.Value = dr["DescripcionPagaduria"].ToString();
                this.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value += " " + dr["Observacion"].ToString();
                this.ClienteID.Value = dr["ClienteID"].ToString();
                this.VlrGiro.Value = Convert.ToDecimal(dr["VlrGiro"]);
                this.VlrCompra.Value = Convert.ToDecimal(dr["VlrCompra"]);
                this.VlrLibranza.Value = Convert.ToDecimal(dr["VlrLibranza"]);
                this.LineaCreditoID.Value = dr["LineaCreditoID"].ToString();
                this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                this.VlrCupoDisponible.Value = Convert.ToDecimal(dr["VlrCupoDisponible"]);
                this.Plazo.Value = Convert.ToInt16(dr["Plazo"]);
                this.VlrSolicitado.Value = Convert.ToDecimal(dr["VlrSolicitado"]);
                this.VlrAdicional.Value = Convert.ToDecimal(dr["VlrAdicional"]);
                this.VlrDescuento.Value = Convert.ToDecimal(dr["VlrDescuento"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_SolicitudAprobacionCartera()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NombreCliente = new UDT_DescripTBase();
            this.Libranza = new UDT_LibranzaID();
            this.AsesorID = new UDT_AsesorID();
            this.DescripcionAsesor = new UDT_Descriptivo();
            this.PagaduriaID = new UDT_PagaduriaID();
            this.DescripcionPagaduria = new UDT_Descriptivo();
            this.VlrPrestamo = new UDT_Valor();
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.ClienteID = new UDTSQL_char(20);
            this.VlrGiro = new UDT_Valor();
            this.VlrCompra = new UDT_Valor();
            this.VlrLibranza = new UDT_Valor();
            this.LineaCreditoID = new UDT_LineaCreditoID();
            this.VlrCuota = new UDT_Valor();
            this.VlrCupoDisponible = new UDT_Valor();
            this.Plazo = new UDTSQL_smallint();
            this.VlrSolicitado = new UDT_Valor();
            this.VlrAdicional = new UDT_Valor();
            this.VlrDescuento = new UDT_Valor();
            this.EmpleadoCodigo = new UDTSQL_char(20);

            //Campo Extra
            this.ActividadFlujoDesc = string.Empty;
            this.ActividadFlujoReversion = new UDT_ActividadFlujoID();
            this.Acierta = new UDTSQL_char(10);
            this.AciertaCifin = new UDTSQL_char(10);
            this.FechaAprobacion = new UDTSQL_smalldatetime();
        }

        #endregion

        [DataMember]
        public UDT_LibranzaID Libranza { get; set; }

        [DataMember]
        public UDT_DescripTBase NombreCliente { get; set; }

        [DataMember]
        public UDT_AsesorID AsesorID { get; set; }

        [DataMember]
        public UDT_Descriptivo DescripcionAsesor { get; set; }

        [DataMember]
        public UDT_PagaduriaID PagaduriaID { get; set; }

        [DataMember]
        public UDT_Descriptivo DescripcionPagaduria { get; set; }

        [DataMember]
        public UDT_Valor VlrPrestamo { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_char ClienteID { get; set; }

        [DataMember]
        public UDT_Valor VlrGiro { get; set; }

        [DataMember]
        public UDT_Valor VlrCompra { get; set; }

        [DataMember]
        public UDT_Valor VlrLibranza { get; set; }

        [DataMember]
        public UDT_LineaCreditoID LineaCreditoID { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrCupoDisponible { get; set; }

        [DataMember]
        public UDTSQL_smallint Plazo { get; set; }

        [DataMember]
        public UDT_Valor VlrSolicitado { get; set; }

        [DataMember]
        public UDT_Valor VlrAdicional { get; set; }

        [DataMember]
        public UDT_Valor VlrDescuento { get; set; }

        //Campo Extra
        [DataMember]
        public string ActividadFlujoDesc { get; set; }

        [DataMember]
        public UDTSQL_char EmpleadoCodigo { get; set; }

        [DataMember]
        public UDT_ActividadFlujoID ActividadFlujoReversion { get; set; }

        [DataMember]
        public UDTSQL_char Acierta { get; set; }

        [DataMember]
        public UDTSQL_char AciertaCifin { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAprobacion { get; set; }

    }
}
