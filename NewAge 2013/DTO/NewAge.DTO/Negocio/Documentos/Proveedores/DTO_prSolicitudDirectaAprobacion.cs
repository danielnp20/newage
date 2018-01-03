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
    /// Models DTO_prSolicitudDirectaAprobacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSolicitudDirectaAprob
    {
        #region DTO_prSolicitudDirectaAprobacion

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prSolicitudDirectaAprob(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PeriodoDoc.Value = Convert.ToDateTime(dr["PeriodoDoc"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.UsuarioID.Value = dr["UsuarioID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prSolicitudDirectaAprob()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Aprobado = new UDT_SiNo();
            this.Rechazado = new UDT_SiNo();
            this.Observacion = new UDT_DescripTExt();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PeriodoDoc = new UDT_PeriodoID();
            this.MonedaID = new UDT_MonedaID();
            this.DocumentoID = new UDT_DocumentoID();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.ProveedorID = new UDT_ProveedorID();
            this.UsuarioID = new UDT_UsuarioID();
            this.TotalML = new UDT_Valor();
            this.TotalME = new UDT_Valor();
            this.IvaML = new UDT_Valor();
            this.IvaME = new UDT_Valor();
            this.FileUrl = "";
            this.SolicitudDirectaAprobDet = new List<DTO_prSolDirectaAprobDet>();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Aprobado { get; set; }

        [DataMember]
        public UDT_SiNo Rechazado { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoDoc { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }
        
        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_UsuarioID UsuarioID { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_Valor TotalML { get; set; }

        [DataMember]
        public UDT_Valor TotalME { get; set; }

        [DataMember]
        public UDT_Valor IvaML { get; set; }

        [DataMember]
        public UDT_Valor IvaME { get; set; }

        [DataMember]
        public string FileUrl { get; set; }

        [DataMember]
        public List<DTO_prSolDirectaAprobDet> SolicitudDirectaAprobDet { get; set; }

        #endregion
    }
    
    /// <summary>
    /// Class Models DTO_prSolDirectaAprobDet
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSolDirectaAprobDet
    {
        public DTO_prSolDirectaAprobDet(IDataReader dr)
        {
            InitDetCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["ConsecutivoDetaID"].ToString()))
                    this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                if (!string.IsNullOrWhiteSpace(dr["Documento5ID"].ToString()))
                    this.Documento5ID.Value = Convert.ToInt32(dr["Documento5ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Detalle5ID"].ToString()))
                    this.Detalle5ID.Value = Convert.ToInt32(dr["Detalle5ID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc5"].ToString()))
                    this.CantidadDoc5.Value = Convert.ToInt32(dr["CantidadDoc5"]);  
                this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorTotML"].ToString()))
                    this.ValorTotML.Value = Convert.ToDecimal(dr["ValorTotML"]);
                if (!string.IsNullOrWhiteSpace(dr["IvaTotML"].ToString()))
                    this.IvaTotML.Value = Convert.ToDecimal(dr["IvaTotML"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorTotME"].ToString()))
                 this.ValorTotME.Value = Convert.ToDecimal(dr["ValorTotME"]);
                if (!string.IsNullOrWhiteSpace(dr["IvaTotME"].ToString()))
                    this.IvaTotME.Value = Convert.ToDecimal(dr["IvaTotME"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_prSolDirectaAprobDet()
        {
            InitDetCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitDetCols()
        {
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.Documento5ID = new UDT_Consecutivo();
            this.Detalle5ID = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CantidadDoc5 = new UDT_Cantidad();    
            this.ValorUni = new UDT_Valor();
            this.ValorTotML = new UDT_Valor();
            this.ValorTotME = new UDT_Valor();
            this.IvaTotME = new UDT_Valor();
            this.IvaTotML = new UDT_Valor();
            this.SolicitudCargos = new List<DTO_prSolicitudCargos>();
        }

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento5ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle5ID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc5 { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        [DataMember]
        public UDT_Valor ValorTotML { get; set; }

        [DataMember]
        public UDT_Valor IvaTotML { get; set; }

        [DataMember]
        public UDT_Valor ValorTotME{ get; set; }

        [DataMember]
        public UDT_Valor IvaTotME { get; set; }

        [DataMember]
        public List<DTO_prSolicitudCargos> SolicitudCargos { get; set; }
        #endregion
    }
}
