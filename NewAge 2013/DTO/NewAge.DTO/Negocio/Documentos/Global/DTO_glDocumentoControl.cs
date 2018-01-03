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
    /// <summary>
    /// Class Error:
    /// Models DTO_glDocumentoControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDocumentoControl
    {
        #region Constructor

		/// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocumentoControl(IDataReader dr)
        {
            InitCols();
            try
            {
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.DocumentoTipo.Value = Convert.ToByte(dr["DocumentoTipo"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.PeriodoDoc.Value = Convert.ToDateTime(dr["PeriodoDoc"]);
                this.PeriodoUltMov.Value = Convert.ToDateTime(dr["PeriodoUltMov"]);
                this.AreaFuncionalID.Value = Convert.ToString(dr["AreaFuncionalID"]);
                this.PrefijoID.Value = Convert.ToString(dr["PrefijoID"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                    this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                if (!string.IsNullOrWhiteSpace(dr["MonedaID"].ToString()))
                    this.MonedaID.Value = Convert.ToString(dr["MonedaID"]);
                this.TasaCambioDOCU.Value = Convert.ToDecimal(dr["TasaCambioDOCU"]);
                this.TasaCambioCONT.Value = Convert.ToDecimal(dr["TasaCambioCONT"]);
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = Convert.ToString(dr["ComprobanteID"]);
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteIDNro"].ToString()))
                    this.ComprobanteIDNro.Value = Convert.ToInt32(dr["ComprobanteIDNro"]);
                if (!string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = Convert.ToString(dr["TerceroID"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoTercero"].ToString()))
                    this.DocumentoTercero.Value = Convert.ToString(dr["DocumentoTercero"]);
                if (!string.IsNullOrWhiteSpace(dr["CuentaID"].ToString()))
                    this.CuentaID.Value = Convert.ToString(dr["CuentaID"]);
                if (!string.IsNullOrWhiteSpace(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = Convert.ToString(dr["ProyectoID"]);
                if (!string.IsNullOrWhiteSpace(dr["CentroCostoID"].ToString()))
                    this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                if (!string.IsNullOrWhiteSpace(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                if (!string.IsNullOrWhiteSpace(dr["LugarGeograficoID"].ToString()))
                    this.LugarGeograficoID.Value = Convert.ToString(dr["LugarGeograficoID"]);
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                this.Estado.Value = Convert.ToInt16(dr["Estado"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoAnula"].ToString()))
                    this.DocumentoAnula.Value = Convert.ToInt32(dr["DocumentoAnula"]);
                if (!string.IsNullOrWhiteSpace(dr["PeriodoAnula"].ToString()))
                    this.PeriodoAnula.Value = Convert.ToDateTime(dr["PeriodoAnula"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsSaldo"].ToString()))
                    this.ConsSaldo.Value = Convert.ToInt32(dr["ConsSaldo"]);
                this.seUsuarioID.Value = Convert.ToInt32(dr["seUsuarioID"]);
                try { this.UsuarioIDDesc.Value = dr["UsuarioIDDesc"].ToString(); } catch (Exception) { }
                if (!string.IsNullOrWhiteSpace(dr["FechaDoc"].ToString()))
                    this.FechaDoc.Value = Convert.ToDateTime(dr["FechaDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Iva"].ToString()))
                    this.Iva.Value = Convert.ToDecimal(dr["Iva"]);
                if (!string.IsNullOrWhiteSpace(dr["Descripcion"].ToString()))
                    this.Descripcion.Value = Convert.ToString(dr["Descripcion"]);
                if (!string.IsNullOrWhiteSpace(dr["Revelacion"].ToString()))
                    this.Revelacion.Value = Convert.ToString(dr["Revelacion"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoPadre"].ToString()))
                    this.DocumentoPadre.Value = Convert.ToInt32(dr["DocumentoPadre"]);
                this.Marca.Value = false;
                try { this.PrefDoc.Value = this.PrefijoID.Value + "-"+ this.DocumentoNro.Value.ToString(); } catch (Exception) { }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDocumentoControl()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpresaID = new UDT_EmpresaID();
            this.DocumentoID = new UDT_DocumentoID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.DocumentoTipo = new UDTSQL_tinyint();
            this.Fecha = new UDTSQL_smalldatetime();
            this.PeriodoDoc = new UDT_PeriodoID();
            this.PeriodoUltMov = new UDT_PeriodoID();
            this.AreaFuncionalID = new UDT_AreaFuncionalID();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.MonedaID = new UDT_MonedaID();
            this.TasaCambioCONT = new UDT_TasaID();
            this.TasaCambioDOCU = new UDT_TasaID();
            this.ComprobanteID = new UDT_ComprobanteID();
            this.ComprobanteIDNro = new UDTSQL_int();
            this.TerceroID = new UDT_TerceroID();
            this.DocumentoTercero = new UDTSQL_char(20);
            this.CuentaID = new UDT_CuentaID();
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.LugarGeograficoID = new UDT_LugarGeograficoID();
            this.Observacion = new UDTSQL_varcharMAX();
            this.Estado = new UDTSQL_smallint();
            this.DocumentoAnula = new UDT_Consecutivo();
            this.PeriodoAnula = new UDT_PeriodoID();
            this.ConsSaldo = new UDT_Consecutivo();
            this.seUsuarioID = new UDT_seUsuarioID();         
            this.FechaDoc = new UDTSQL_smalldatetime();
            this.Valor = new UDT_Valor();
            this.Iva = new UDT_Valor();
            this.Descripcion = new UDT_DescripTBase();
            this.Revelacion = new UDT_DescripTExt();
            this.DocumentoPadre = new UDT_Consecutivo();
            this.UsuarioIDDesc = new UDT_UsuarioID();
            this.DocMask = new UDT_DescripTBase();
            this.Marca = new UDT_SiNo();
            this.FechaInicial = new UDTSQL_smalldatetime();
            this.FechaFinal = new UDTSQL_smalldatetime();
            this.TerceroDesc = new UDT_DescripTBase();
            this.PrefDoc = new UDT_DescripTBase();
            this.Comprobante = new UDT_DescripTBase();
            this.Cantidad = new UDT_Cantidad();
            this.ProyectoDesc = new UDT_DescripTBase();
        }

	    #endregion
        
        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint DocumentoTipo { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PeriodoID PeriodoDoc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PeriodoID PeriodoUltMov { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_AreaFuncionalID AreaFuncionalID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }
    
        [DataMember]
        [NotImportable]
        public UDT_TasaID TasaCambioDOCU { get; set; }

        [DataMember]
        public UDT_TasaID TasaCambioCONT { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_ComprobanteID ComprobanteID { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_int ComprobanteIDNro { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDTSQL_char DocumentoTercero { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_LugarGeograficoID LugarGeograficoID { get; set; }

        [DataMember]
        public UDTSQL_varcharMAX Observacion { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint Estado { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo DocumentoAnula { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_PeriodoID PeriodoAnula { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo ConsSaldo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_seUsuarioID seUsuarioID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor Iva { get; set; }

        [DataMember]
        public UDT_DescripTBase Descripcion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt Revelacion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo DocumentoPadre { get; set; }

        #region Campos Adicionales
        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_UsuarioID UsuarioIDDesc { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_SiNo Marca { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase DocMask { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_smalldatetime FechaInicial { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDTSQL_smalldatetime FechaFinal { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase TerceroDesc { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase PrefDoc { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase Comprobante { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        [AllowNull]
        [NotImportable]
        public UDT_DescripTBase ProyectoDesc { get; set; }
        #endregion

        #endregion
    }
}
