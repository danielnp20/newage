using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;
using System.Reflection;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Models DTO_prRecibidoAprob
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prRecibidoAprob
    {
        #region DTO_prRecibidoAprob

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prRecibidoAprob(IDataReader dr)
        {
            InitCols();
            try
            {
               this.EmpresaID.Value = dr["EmpresaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoID"].ToString()))
                    this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                if (!string.IsNullOrWhiteSpace(dr["DocumentoNro"].ToString()))
                    this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.PrefDoc = this.PrefijoID.Value + " - " + this.DocumentoNro.Value.Value.ToString();
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
               this.ProveedorID.Value = dr["ProveedorID"].ToString();
               this.ProveedorNombre.Value = dr["ProveedorNombre"].ToString(); 
               this.MonedaID.Value = dr["MonedaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TasaCambioDOCU"].ToString()))
                    this.TasaCambioDOCU.Value = Convert.ToDecimal(dr["TasaCambioDOCU"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["CostoIvaML"].ToString()))
                    this.CostoIvaML.Value = Convert.ToDecimal(dr["CostoIvaML"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoIvaME"].ToString()))
                    this.CostoIvaME.Value = Convert.ToDecimal(dr["CostoIvaME"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoML"].ToString()))
                    this.CostoML.Value = Convert.ToDecimal(dr["CostoML"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoME"].ToString()))
                    this.CostoME.Value = Convert.ToDecimal(dr["CostoME"]);
               this.UsuarioID.Value = dr["UsuarioID"].ToString();
               this.Observacion.Value = dr["ObservacionDesc"].ToString();
                this.Seleccionar.Value = false;                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prRecibidoAprob()
        {
            this.InitCols();
            this.CostoMLRecib.Value = 0;
            this.CostoMERecib.Value = 0;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.Fecha = new UDTSQL_datetime();
            this.DocumentoID = new UDT_DocumentoID();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.ProveedorNombre = new UDT_DescripTBase();
            this.MonedaID = new UDT_MonedaID();
            this.TasaCambioDOCU = new UDT_Valor();
            this.CostoMLRecib = new UDT_Valor();
            this.CostoMERecib = new UDT_Valor();
            this.CostoML = new UDT_Valor();
            this.CostoME = new UDT_Valor();
            this.CostoIvaML = new UDT_Valor();
            this.CostoIvaME = new UDT_Valor();
            this.UsuarioID = new UDT_UsuarioID();
            this.Observacion = new UDT_DescripTExt();
            this.ConformidadInd = new UDT_SiNo();
            this.Calificacion = new UDTSQL_tinyint();
            this.FileUrl = "";
            this.Detalle = new List<DTO_prRecibidoAprobDet>();
            this.Seleccionar = new UDT_SiNo();
        }
        #endregion

        #region Propiedades
        [DataMember]
        [AllowNull]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        [AllowNull]
        public string PrefDoc { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTBase ProveedorNombre { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor TasaCambioDOCU { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CostoMLRecib { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CostoMERecib { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CostoML { get; set; }

        [DataMember]
        public UDT_Valor CostoME { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CostoIvaML { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor CostoIvaME { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo ConformidadInd { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint Calificacion { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        [AllowNull]
        public string FileUrl { get; set; }

        [DataMember]
        [AllowNull]
        public List<DTO_prRecibidoAprobDet> Detalle { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SiNo Seleccionar { get; set; }

        #endregion
    }

    /// <summary>
    /// Class Models DTO_prRecibidoAprobDet
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prRecibidoAprobDet
    {
        public DTO_prRecibidoAprobDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["ConsecutivoDetaID"].ToString()))
                    this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                if (!string.IsNullOrWhiteSpace(dr["RecibidoDocuID"].ToString()))
                    this.RecibidoDocuID.Value = Convert.ToInt32(dr["RecibidoDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["RecibidoDetaID"].ToString()))
                    this.RecibidoDetaID.Value = Convert.ToInt32(dr["RecibidoDetaID"]);
                if (!string.IsNullOrWhiteSpace(dr["OrdCompraDocuID"].ToString()))
                   this.OrdCompraDocuID.Value = Convert.ToInt32(dr["OrdCompraDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["OrdCompraDetaID"].ToString()))
                   this.OrdCompraDetaID.Value = Convert.ToInt32(dr["OrdCompraDetaID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();              
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.SerialID.Value = dr["SerialID"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadRec"].ToString()))
                    this.CantidadRec.Value = Convert.ToDecimal(dr["CantidadRec"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDocuID"].ToString()))
                    this.SolicitudDocuID.Value = Convert.ToInt32(dr["SolicitudDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDetaID"].ToString()))
                    this.SolicitudDetaID.Value = Convert.ToInt32(dr["SolicitudDetaID"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorUni"].ToString()))
                    this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                if (!string.IsNullOrWhiteSpace(dr["IVAUni"].ToString()))
                   this.IVAUni.Value = Convert.ToDecimal(dr["IVAUni"]);
                this.ValorTotMLRecibDet.Value = 0;
                this.ValorTotMERecibDet.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_prRecibidoAprobDet()
        {
            InitDetCols();
            this.ValorUni.Value =  0;
            this.ValorMLDet.Value = 0;
            this.ValorMEDet.Value = 0;
            this.IVAUni.Value =  0;
            this.PorcIVA = 0;
            this.ValorTotMLRecibDet.Value = 0;
            this.ValorTotMERecibDet.Value = 0;
            this.TasaOC.Value = 0;
            this.Margen.Value = 0;
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.RecibidoDocuID = new UDT_Consecutivo();
            this.RecibidoDetaID = new UDT_Consecutivo();
            this.OrdCompraDocuID = new UDT_Consecutivo();
            this.OrdCompraDetaID = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.SerialID = new UDT_SerialID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CantidadRec = new UDT_Cantidad();
            this.SolicitudDocuID = new UDT_Consecutivo();
            this.SolicitudDetaID = new UDT_Consecutivo();
            this.MonedaOC = new UDT_MonedaID();
            this.TasaOC = new UDT_Valor();
            this.Margen = new UDT_Cantidad();
            this.ValorTotMLRecibDet = new UDT_Valor();
            this.ValorTotMERecibDet = new UDT_Valor();
            this.OrigenMonetario = new UDTSQL_tinyint();
            this.ValorUni = new UDT_Valor();
            this.ValorMLDet = new UDT_Valor();
            this.ValorMEDet = new UDT_Valor();
            this.ValorIvaMLDet = new UDT_Valor();
            this.ValorIvaMEDet = new UDT_Valor();
            this.IVAUni = new UDT_Valor();
            this.FileUrlDet = "";
        }

        #region Propiedades
        
        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo RecibidoDocuID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo RecibidoDetaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo OrdCompraDocuID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo OrdCompraDetaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_SerialID SerialID { get; set; }
               
        [DataMember]
        [AllowNull]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad CantidadRec { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo SolicitudDocuID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo SolicitudDetaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_MonedaID MonedaOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor TasaOC { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Cantidad Margen { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorTotMLRecibDet { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorTotMERecibDet { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint OrigenMonetario { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor IVAUni { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorMLDet { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorMEDet { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorIvaMLDet { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor ValorIvaMEDet { get; set; }

        [DataMember]
        [AllowNull]
        public decimal PorcIVA { get; set; }

        [DataMember]
        public string PrefDoc { get; set; }

        [DataMember]
        [AllowNull]
        public string FileUrlDet { get; set; }
        #endregion
    }
}
