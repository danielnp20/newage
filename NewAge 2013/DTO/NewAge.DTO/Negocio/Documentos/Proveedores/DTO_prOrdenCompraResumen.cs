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
    /// Models DTO_prOrdenCompraResumen
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prOrdenCompraResumen
    {
        #region DTO_prOrdenCompraResumen

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prOrdenCompraResumen(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["SolicitudDocuID"].ToString()))
                    this.SolicitudDocuID.Value = Convert.ToInt32(dr["SolicitudDocuID"]);
                this.PrefijoIDSol.Value = dr["PrefijoIDSol"].ToString();
                this.DocumentoNroSol.Value = Convert.ToInt32(dr["DocumentoNroSol"]);
                if (!string.IsNullOrWhiteSpace(dr["OrdCompraDocuID"].ToString()))
                    this.OrdCompraDocuID.Value = Convert.ToInt32(dr["OrdCompraDocuID"]);
                if (!string.IsNullOrWhiteSpace(dr["ContratoDocuID"].ToString()))
                    this.ContratoDocuID.Value = Convert.ToInt32(dr["ContratoDocuID"]);
                this.PrefijoIDOC.Value = dr["PrefijoIDOC"].ToString();
                this.DocumentoNroOC.Value = Convert.ToInt32(dr["DocumentoNroOC"]);
                this.FechaOC.Value = Convert.ToDateTime(dr["FechaOC"]);
                this.ConsecutivoDetaID.Value = 0;
                this.SolicitudDetaID.Value = Convert.ToInt32(dr["SolicitudDetaID"]);
                this.OrdCompraDetaID.Value = Convert.ToInt32(dr["OrdCompraDetaID"]);
                this.CodigoBSID.Value = dr["CodigoBSID"].ToString();
                this.inReferenciaID.Value = dr["inReferenciaID"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.ProveedorID.Value = dr["ProveedorID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadSol"].ToString()))
                    this.CantidadSol.Value = Convert.ToDecimal(dr["CantidadSol"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadOC"].ToString()))
                    this.CantidadOC.Value = Convert.ToDecimal(dr["CantidadOC"]);
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
                this.MonedaIDOC.Value = dr["MonedaIDOC"].ToString();
                this.MonedaPagoOC.Value = dr["MonedaPagoOC"].ToString();
                this.MonedaOrdenOC.Value = dr["MonedaOrden"].ToString();
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.EmpaqueInvID.Value = dr["EmpaqueInvID"].ToString();
                this.MarcaInvID.Value = dr["MarcaInvID"].ToString();
                this.RefProveedor.Value = dr["RefProveedor"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CantidadRec.Value = 0;
                this.UnidadEmpaque.Value = dr["UnidadEmpaque"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CantidadEmpaque"].ToString()))
                    this.CantidadxEmpaque.Value = Convert.ToInt32(dr["CantidadEmpaque"]);
                else  this.CantidadxEmpaque.Value = 1;
                if (!string.IsNullOrWhiteSpace(dr["TasaOrden"].ToString()))
                    this.TasaOrdenOC.Value = Convert.ToDecimal(dr["TasaOrden"]);
                else this.TasaOrdenOC.Value = 0;
                this.PrefDocSol = this.PrefijoIDSol.Value + "-" + this.DocumentoNroSol.Value.Value.ToString();
                this.PrefDocOC = this.PrefijoIDOC.Value + "-" + this.DocumentoNroOC.Value.Value.ToString();              
                this.SerialID.Value = string.Empty;
                if (!string.IsNullOrWhiteSpace(dr["ValorTotMLOC"].ToString()))
                    this.ValorTotMLOC.Value = Convert.ToDecimal(dr["ValorTotMLOC"]);
                else this.ValorTotMLOC.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["ValorTotMEOC"].ToString()))
                    this.ValorTotMEOC.Value = Convert.ToDecimal(dr["ValorTotMEOC"]);
                else this.ValorTotMEOC.Value = 0;
                if (!string.IsNullOrWhiteSpace(dr["ValorUni"].ToString()))
                    this.ValorUni.Value = Convert.ToDecimal(dr["ValorUni"]);
                else this.ValorUni.Value = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prOrdenCompraResumen()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.SolicitudDocuID = new UDT_Consecutivo();
            this.SolicitudDetaID = new UDT_Consecutivo();
            this.PrefijoIDSol = new UDT_PrefijoID();
            this.DocumentoNroSol = new UDT_Consecutivo();
            this.CantidadSol = new UDT_Cantidad();

            this.OrdCompraDocuID = new UDT_Consecutivo();
            this.OrdCompraDetaID = new UDT_Consecutivo();
            this.PrefijoIDOC = new UDT_PrefijoID();
            this.DocumentoNroOC = new UDT_Consecutivo();
            this.FechaOC = new UDTSQL_datetime();
            this.CantidadOC = new UDT_Cantidad();
            this.MonedaIDOC = new UDT_MonedaID();
            this.MonedaPagoOC = new UDT_MonedaID();
            this.MonedaOrdenOC = new UDT_MonedaID();
            this.ProyectoID = new UDT_ProyectoID();

            this.ContratoDocuID = new UDT_Consecutivo();
            this.ConsecutivoDetaID = new UDT_Consecutivo();  
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.Descriptivo = new UDT_DescripTExt();
            this.CantidadRec = new UDT_Cantidad();           
            this.SerialID = new UDT_SerialID();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.EmpaqueInvID = new UDT_EmpaqueInvID();
            this.MarcaInvID = new UDTSQL_char(15);
            this.RefProveedor = new UDT_CodigoGrl20();
            this.ProveedorID = new UDT_ProveedorID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
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
            this.CantidadxEmpaque = new UDT_Cantidad();
            this.CantEmpaque = new UDT_Cantidad();
            this.UnidadEmpaque = new UDT_UnidadInvID();
            this.TasaOrdenOC = new UDT_Valor();
            this.ValorTotMLOC = new UDT_Valor();
            this.ValorTotMEOC = new UDT_Valor();
            this.ValorUni = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoIDSol { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNroSol { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo OrdCompraDocuID { get; set; }

        [DataMember]
        public UDT_Consecutivo ContratoDocuID { get; set; }

        [DataMember]
        public UDT_PrefijoID PrefijoIDOC { get; set; }

        [DataMember]
        public UDT_Consecutivo DocumentoNroOC { get; set; }

        [DataMember]
        public UDTSQL_datetime FechaOC{ get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo SolicitudDetaID { get; set; }

        [DataMember]
        public UDT_Consecutivo OrdCompraDetaID { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }
        
        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public bool invSerialInd { get; set; }      

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }
      
        [DataMember]
        public UDT_Cantidad CantidadSol { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadOC { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadRec { get; set; }

        [DataMember]
        public string PrefDocSol { get; set; }

        [DataMember]
        public string PrefDocOC { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaIDOC { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaPagoOC { get; set; }

        [DataMember]
        public UDT_MonedaID MonedaOrdenOC { get; set; }

        [DataMember]
        public UDT_SerialID SerialID { get; set; }

        [DataMember]
        public TipoCodigo ClaseBS { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_EmpaqueInvID EmpaqueInvID { get; set; }

        [DataMember]
        public UDTSQL_char MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

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
        public UDT_Cantidad CantidadxEmpaque { get; set; }

        [DataMember]
        public UDT_Cantidad CantEmpaque { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadEmpaque { get; set; }

        [DataMember]
        public UDT_Valor TasaOrdenOC { get; set; }

        [DataMember]
        public UDT_Valor ValorTotMLOC { get; set; }

        [DataMember]
        public UDT_Valor ValorTotMEOC { get; set; }

        [DataMember]
        public UDT_Valor ValorUni { get; set; }

        #endregion
    }
}
