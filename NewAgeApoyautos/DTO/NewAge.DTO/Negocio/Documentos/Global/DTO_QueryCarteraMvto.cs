using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <Grilla General>
    /// Class Error:
    /// Models DTO_glQueryCarteraMvto
    /// </summary>
    
    [DataContract]
    [Serializable]
    public class DTO_QueryCarteraMvto
    {
        #region Asigna Valores (Constructor)

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos , convertir con el tipo de dato de la BD
        /// </summary>
        /// <param name="?"></param>
        public DTO_QueryCarteraMvto(IDataReader dr, bool byLibranza)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.NumCredito.Value = Convert.ToInt32(dr["NumCredito"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                
                if (!byLibranza)
                {                  
                    if (!string.IsNullOrWhiteSpace(dr["Fecha_Movimiento"].ToString()))
                        this.Fecha_Movimiento.Value = Convert.ToDateTime(dr["Fecha_Movimiento"]);
                    this.FechaAplicacion.Value = Convert.ToString(dr["FechaAplicacion"]);
                    this.NroCredito.Value = Convert.ToString(dr["NroCredito"]);
                    this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                    this.Nom_Cliente.Value = Convert.ToString(dr["Nom_Cliente"]);
                    this.PrefDoc.Value = Convert.ToString(dr["PrefDoc"]);
                    this.Comprobante.Value = Convert.ToString(dr["Comprobante"]);
                    this.TotalDocumento.Value = Convert.ToInt32(dr["TotalDocumento"]);
                    this.TotalCuota.Value = Convert.ToInt32(dr["TotalCuota"]);
                }
                else 
                {
                    this.PrefijoID.Value = dr["PrefijoID"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                        this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                    if (!string.IsNullOrWhiteSpace(dr["ComprobanteIDNro"].ToString()))
                        this.ComprobanteIDNro.Value = Convert.ToInt32(dr["ComprobanteIDNro"]);
                    if (!string.IsNullOrWhiteSpace(dr["FechaAplica"].ToString()))
                        this.FechaAplica.Value = Convert.ToDateTime(dr["FechaAplica"]);
                    if (!string.IsNullOrWhiteSpace(dr["FechaConsigna"].ToString()))
                        this.FechaConsigna.Value = Convert.ToDateTime(dr["FechaConsigna"]);
                    if (!string.IsNullOrWhiteSpace(dr["FechaDoc"].ToString()))
                        this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                    this.Libranza.Value = dr["Libranza"].ToString();
                    this.Descripcion.Value = dr["Descripcion"].ToString();
                    this.Observacion.Value = dr["Observacion"].ToString();
                    this.VlrCapital.Value = Convert.ToDecimal(dr["VlrCapital"]);
                    this.VlrInteres.Value = Convert.ToDecimal(dr["VlrInteres"]);
                    this.VlrSeguro.Value = Convert.ToDecimal(dr["VlrSeguro"]);
                    this.VlrOtrCuota.Value = Convert.ToDecimal(dr["VlrOtrCuota"]);
                    this.VlrMora.Value = Convert.ToDecimal(dr["VlrMora"]);
                    this.VlrPrejuridico.Value = Convert.ToDecimal(dr["VlrPrejuridico"]);
                    this.SdoFavor.Value = Convert.ToDecimal(dr["SdoFavor"]);
                    this.VlrGastos.Value = Convert.ToDecimal(dr["VlrGastos"]);
                    this.VlrExtra.Value = Convert.ToDecimal(dr["VlrExtra"]);
                    this.ClienteID.Value = Convert.ToString(dr["ClienteID"]);
                    this.Nom_Cliente.Value = Convert.ToString(dr["Nom_Cliente"]);
                    this.CompradorCarteraID.Value = Convert.ToString(dr["CompradorCarteraID"]);
                    this.NombreCompra.Value = Convert.ToString(dr["NombreCompra"]);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_QueryCarteraMvto()
        {
            InitCols();
        }

        #endregion

        #region Inicializa Variables

        /// <summary>
        /// Se Instancia las variables , se puede utilizar los alias
        /// </summary>
        
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.NumCredito = new UDT_Consecutivo();
            this.DocumentoID = new UDT_DocumentoID();
            this.Fecha_Movimiento = new UDTSQL_smalldatetime();
            this.FechaAplicacion = new UDT_DescripTExt();
            this.NroCredito = new UDT_DocTerceroID();
            this.ClienteID = new UDT_ClienteID();
            this.Nom_Cliente = new UDT_Descriptivo();
            this.PrefDoc = new UDT_DescripTBase();
            this.Comprobante = new UDT_DescripTBase();
            this.TotalDocumento = new UDT_Valor();
            this.TotalCuota = new UDT_Valor();
            this.Detalle = new List<DTO_QueryCarteraMvtoDeta>();
            this.DetalleReport = new List<DTO_QueryCarteraMvto>();

            //Consulta por Libranza
            this.CompradorCarteraID = new UDT_CompradorCarteraID();
            this.NombreCompra = new UDT_Descriptivo();
            this.EstadoCartera = new UDTSQL_tinyint();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDTSQL_int();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteIDNro = new UDTSQL_int();
            this.FechaAplica = new UDTSQL_smalldatetime();
            this.FechaConsigna = new UDTSQL_smalldatetime();
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.Libranza = new UDT_DocTerceroID();
            this.Descripcion = new UDT_DescripTBase();
            this.Observacion = new UDT_DescripTExt();
            this.VlrCapital = new UDT_Valor();
            this.VlrInteres = new UDT_Valor();
            this.VlrSeguro = new UDT_Valor();
            this.VlrOtrCuota = new UDT_Valor();
            this.VlrMora = new UDT_Valor();
            this.VlrPrejuridico = new UDT_Valor();
            this.SdoFavor = new UDT_Valor();
            this.VlrGastos = new UDT_Valor();
            this.VlrExtra = new UDT_Valor();
        } 
        #endregion
        
        #region Propiedades

        /// <summary>
        /// Se agrega el tipo de dato de la Variable, Se puede usar los alias
        /// </summary>

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo NumCredito { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha_Movimiento { get; set; }

        [DataMember]
        public UDT_DescripTExt FechaAplicacion { get; set; }

        [DataMember]
        public UDT_DocTerceroID NroCredito { get; set; }

        [DataMember]
        public UDT_ClienteID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo Nom_Cliente { get; set; }

        [DataMember]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        public UDT_DescripTBase Comprobante { get; set; }

        [DataMember]
        public UDT_Valor TotalDocumento { get; set; }

        [DataMember]
        public UDT_Valor TotalCuota { get; set; }

        [DataMember]
        public List<DTO_QueryCarteraMvtoDeta> Detalle { get; set; }

        [DataMember]
        public List<DTO_QueryCarteraMvto> DetalleReport { get; set; }

        #region Consulta Mvtos por Libranza

        [DataMember]
        public UDTSQL_tinyint EstadoCartera { get; set; }

        [DataMember]
        public UDT_CompradorCarteraID CompradorCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo NombreCompra { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDTSQL_int DocumentoNro { get; set; }

        [DataMember]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        public UDTSQL_int ComprobanteIDNro { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaConsigna { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaAplica { get; set; }

        [DataMember]
        public UDT_DocTerceroID Libranza { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public string CuotaID { get; set; }

        [DataMember]
        public UDT_Valor VlrCapital { get; set; }

        [DataMember]
        public UDT_Valor VlrInteres { get; set; }

        [DataMember]
        public UDT_Valor VlrSeguro { get; set; }

        [DataMember]
        public UDT_Valor VlrOtrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrMora { get; set; }

        [DataMember]
        public UDT_Valor VlrPrejuridico { get; set; }

        [DataMember]
        public UDT_Valor SdoFavor { get; set; }

        [DataMember]
        public UDT_Valor VlrGastos { get; set; }

        [DataMember]
        public UDT_Valor VlrExtra { get; set; } 
        #endregion

        #endregion
    }



    /// <Subgrilla>
    /// SubGrilla
    /// Models DTO_glQueryCarteraMvtoDeta
    /// </summary>
    
    [DataContract]
    [Serializable]
    public class DTO_QueryCarteraMvtoDeta
    {
        #region Asigna Valores (Constructor)

        /// <Descripcion>
        /// Constuye el DTO a partir de un resultado de base de datos , convertir con el tipo de dato de la BD
        /// </summary>
        /// <param name="?"></param>
        
        public DTO_QueryCarteraMvtoDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ComponenteCarteraID.Value = Convert.ToString(dr["ComponenteCarteraID"]);
                this.ComponenteDesc.Value = Convert.ToString(dr["ComponenteDesc"]);
                this.AbonoDocumento.Value = Convert.ToInt32(dr["AbonoDocumento"]);
                this.AbonoCuota.Value = Convert.ToInt32(dr["AbonoCuota"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <Descripcion>
        /// Constructor por defecto
        /// </summary>
        /// 
        public DTO_QueryCarteraMvtoDeta()
        {
            InitCols();
        }

        #endregion

        #region Inicializa Variables

        /// <summary>
        /// Se Instancia las variables , se puede utilizar los alias
        /// </summary>

        private void InitCols()
        {
            this.ComponenteCarteraID = new UDT_ComponenteCarteraID();
            this.ComponenteDesc = new UDT_Descriptivo();
            this.AbonoDocumento = new UDT_Valor();
            this.AbonoCuota = new UDT_Valor();
        } 
        #endregion

        #region Propiedades

        /// <summary>
        /// Se agrega el tipo de dato de la Variable, Se puede usar los alias
        /// </summary>

        [DataMember]
        public UDT_ComponenteCarteraID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComponenteDesc { get; set; }

        [DataMember]
        public UDT_Valor AbonoDocumento { get; set; }

        [DataMember]
        public UDT_Valor AbonoCuota { get; set; }
        #endregion
    }
}
