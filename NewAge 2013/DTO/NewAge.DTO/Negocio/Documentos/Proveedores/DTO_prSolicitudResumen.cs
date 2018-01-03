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
    /// Models DTO_prSolicitudResumen
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prSolicitudResumen
    {
        #region DTO_prSolicitudResumen

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prSolicitudResumen(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDocuID"].ToString()))
                    this.SolicitudDocuID.Value = Convert.ToInt32(dr["SolicitudDocuID"]); ;
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.DocumentoID.Value = Convert.ToInt32(dr["DocumentoID"]);
                this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.DocumentoNro.Value = Convert.ToInt32(dr["DocumentoNro"]);
                this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Parametro1.Value = dr["Parametro1"].ToString();
                this.Parametro2.Value = dr["Parametro2"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.EmpaqueInvID.Value = dr["EmpaqueInvID"].ToString();
                this.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                this.RefProveedor.Value = dr["RefProveedor"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadSol"].ToString()))
                    this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadSolTOT"].ToString()))
                    this.CantidadSolTOT.Value = Convert.ToDecimal(dr["CantidadSolTOT"]);
                if (!string.IsNullOrWhiteSpace(dr["Documento1ID"].ToString()))
                    this.Documento1ID.Value = Convert.ToInt32(dr["Documento1ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Documento2ID"].ToString()))
                    this.Documento2ID.Value = Convert.ToInt32(dr["Documento2ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Documento3ID"].ToString()))
                    this.Documento3ID.Value = Convert.ToInt32(dr["Documento3ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Documento4ID"].ToString()))
                    this.Documento4ID.Value = Convert.ToInt32(dr["Documento4ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Documento5ID"].ToString()))
                    this.Documento5ID.Value = Convert.ToInt32(dr["Documento5ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Detalle1ID"].ToString()))
                    this.Detalle1ID.Value = Convert.ToInt32(dr["Detalle1ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Detalle2ID"].ToString()))
                    this.Detalle2ID.Value = Convert.ToInt32(dr["Detalle2ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Detalle3ID"].ToString()))
                    this.Detalle3ID.Value = Convert.ToInt32(dr["Detalle3ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Detalle4ID"].ToString()))
                    this.Detalle4ID.Value = Convert.ToInt32(dr["Detalle4ID"]);
                if (!string.IsNullOrWhiteSpace(dr["Detalle5ID"].ToString()))
                    this.Detalle5ID.Value = Convert.ToInt32(dr["Detalle5ID"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc1"].ToString()))
                    this.CantidadDoc1.Value = Convert.ToInt32(dr["CantidadDoc1"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc2"].ToString()))
                    this.CantidadDoc2.Value = Convert.ToInt32(dr["CantidadDoc2"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc3"].ToString()))
                    this.CantidadDoc3.Value = Convert.ToInt32(dr["CantidadDoc3"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc4"].ToString()))
                    this.CantidadDoc4.Value = Convert.ToInt32(dr["CantidadDoc4"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadDoc5"].ToString()))
                    this.CantidadDoc5.Value = Convert.ToInt32(dr["CantidadDoc5"]);
                if (!string.IsNullOrWhiteSpace(dr["OrigenMonetario"].ToString()))
                    this.OrigenMonetario.Value = Convert.ToByte(dr["OrigenMonetario"]);
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                this.DatoAdd5.Value = dr["DatoAdd5"].ToString();
            
                this.CantidadOrdenComp.Value = 0;
                this.PrefDoc = this.PrefijoID.Value + "-" + this.DocumentoNro.Value.Value.ToString(); 
                this.ValorUni.Value = 0;
                this.Selected.Value = false;
                this.UnidadEmpaque.Value = dr["UnidadEmpaque"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadEmpaque"].ToString()))
                    this.CantidadxEmpaque.Value = Convert.ToInt32(dr["CantidadEmpaque"]);
                else
                    this.CantidadxEmpaque.Value = 1;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prSolicitudResumen()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.Selected = new UDT_SiNo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.SolicitudDocuID = new UDT_Consecutivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.Fecha = new UDTSQL_datetime();
            this.DocumentoID = new UDT_DocumentoID();
            this.PrefijoID = new UDT_PrefijoID();
            this.DocumentoNro = new UDT_Consecutivo();
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.Descriptivo = new UDT_DescripTExt();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.Parametro1 = new UDT_ParametroInvID();
            this.Parametro2 = new UDT_ParametroInvID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.EmpaqueInvID = new UDT_EmpaqueInvID();
            this.MarcaInvID = new UDTSQL_char(15);
            this.RefProveedor = new UDT_CodigoGrl20();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.CantidadSol = new UDT_Cantidad();
            this.CantidadOrdenComp = new UDT_Cantidad();
            this.CantidadSolTOT = new UDT_Cantidad();
            this.ValorUni = new UDT_Valor();
            this.Documento1ID = new UDT_Consecutivo();
            this.Documento2ID = new UDT_Consecutivo();
            this.Documento3ID = new UDT_Consecutivo();
            this.Documento4ID = new UDT_Consecutivo();
            this.Documento5ID = new UDT_Consecutivo();
            this.Detalle1ID = new UDT_Consecutivo();
            this.Detalle2ID = new UDT_Consecutivo();
            this.Detalle3ID = new UDT_Consecutivo();
            this.Detalle4ID = new UDT_Consecutivo();
            this.Detalle5ID = new UDT_Consecutivo();
            this.CantidadDoc1 = new UDT_Cantidad();
            this.CantidadDoc2 = new UDT_Cantidad();
            this.CantidadDoc3 = new UDT_Cantidad();
            this.CantidadDoc4 = new UDT_Cantidad();
            this.CantidadDoc5 = new UDT_Cantidad();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.ProyectoID = new UDT_ProyectoID();
            this.OrigenMonetario = new UDTSQL_tinyint();
            this.NumeroDocOC = new UDT_Consecutivo();
            this.SolicitudCargos = new List<DTO_prSolicitudCargos>();
            this.CantidadxEmpaque = new UDT_Cantidad();
            this.UnidadEmpaque = new UDT_UnidadInvID();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_SiNo Selected { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDTSQL_datetime Fecha { get; set; }

        [DataMember]
        public UDT_DocumentoID DocumentoID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoID { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNro { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro1 { get; set; }

        [DataMember]
        public UDT_ParametroInvID Parametro2 { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        public UDTSQL_char MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadOrdenComp { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSolTOT { get; set; }

        [DataMember]
        public string PrefDoc { get; set; }
    
        [DataMember]
        public UDT_Valor ValorUni { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Documento1ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento2ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento3ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento4ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento5ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle1ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle2ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle3ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle4ID { get; set; }

        [DataMember]
        public UDT_Consecutivo Detalle5ID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc1 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc2 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc3 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc4 { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDoc5 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDTSQL_tinyint OrigenMonetario { get; set; }       

        [DataMember]
        public List<DTO_prSolicitudCargos> SolicitudCargos { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDocOC { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadxEmpaque { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadEmpaque { get; set; }

        #endregion
    }
}
