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
    /// 
    /// Models DTO_pyProyectoMvto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoMvto
    {
        #region DTO_pyProyectoMvto

        public DTO_pyProyectoMvto(IDataReader dr)
        {
            InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.ConsecDeta.Value = Convert.ToInt32(dr["ConsecDeta"]);
                this.EmpresaID.Value = Convert.ToString(dr["EmpresaID"]);
                if (!string.IsNullOrEmpty(dr["TipoMvto"].ToString()))
                    this.TipoMvto.Value = Convert.ToByte(dr["TipoMvto"]);
                if (!string.IsNullOrEmpty(dr["NumProyecto"].ToString()))
                    this.NumProyecto.Value = Convert.ToInt32(dr["NumProyecto"]);
                if (!string.IsNullOrEmpty(dr["CostoLocal"].ToString()))
                    this.CostoLocal.Value = Convert.ToDecimal(dr["CostoLocal"]);
                if (!string.IsNullOrEmpty(dr["CostoExtra"].ToString()))
                    this.CostoExtra.Value = Convert.ToDecimal(dr["CostoExtra"]);
                if (!string.IsNullOrEmpty(dr["CantidadTOT"].ToString()))
                    this.CantidadTOT.Value = Convert.ToDecimal(dr["CantidadTOT"]);
                if (!string.IsNullOrEmpty(dr["CantidadSOL"].ToString()))
                    this.CantidadSOL.Value = Convert.ToDecimal(dr["CantidadSOL"]);
                if (!string.IsNullOrEmpty(dr["CantidadPROV"].ToString()))
                    this.CantidadPROV.Value = Convert.ToDecimal(dr["CantidadPROV"]);
                if (!string.IsNullOrEmpty(dr["CantidadNOM"].ToString()))
                    this.CantidadNOM.Value = Convert.ToDecimal(dr["CantidadNOM"]);
                if (!string.IsNullOrEmpty(dr["CantidadACT"].ToString()))
                    this.CantidadACT.Value = Convert.ToDecimal(dr["CantidadACT"]);
                if (!string.IsNullOrEmpty(dr["CantidadINV"].ToString()))
                    this.CantidadINV.Value = Convert.ToDecimal(dr["CantidadINV"]);
                if (!string.IsNullOrEmpty(dr["CantidadBOD"].ToString()))
                    this.CantidadBOD.Value = Convert.ToDecimal(dr["CantidadBOD"]);
                if (!string.IsNullOrEmpty(dr["CantidadREC"].ToString()))
                    this.CantidadREC.Value = Convert.ToDecimal(dr["CantidadREC"]);
                if (!string.IsNullOrEmpty(dr["CostoLocalTOT"].ToString()))
                    this.CostoLocalTOT.Value = Convert.ToDecimal(dr["CostoLocalTOT"]);
                if (!string.IsNullOrEmpty(dr["CostoExtraTOT"].ToString()))
                    this.CostoExtraTOT.Value = Convert.ToDecimal(dr["CostoExtraTOT"]);
                if (!string.IsNullOrEmpty(dr["AjCambioLocal"].ToString()))
                    this.AjCambioLocal.Value = Convert.ToDecimal(dr["AjCambioLocal"]);
                if (!string.IsNullOrEmpty(dr["AjCambioExtra"].ToString()))
                    this.AjCambioExtra.Value = Convert.ToDecimal(dr["AjCambioExtra"]);
                if (!string.IsNullOrEmpty(dr["VPN_ML"].ToString()))
                    this.VPN_ML.Value = Convert.ToDecimal(dr["VPN_ML"]);
                if (!string.IsNullOrEmpty(dr["VPN_ME"].ToString()))
                    this.VPN_ME.Value = Convert.ToDecimal(dr["VPN_ME"]);
                if (!string.IsNullOrEmpty(dr["FechaInicioTarea"].ToString()))
                    this.FechaInicioTarea.Value = Convert.ToDateTime(dr["FechaInicioTarea"]);
                if (!string.IsNullOrEmpty(dr["FechaOrdCompra"].ToString()))
                    this.FechaOrdCompra.Value = Convert.ToDateTime(dr["FechaOrdCompra"]);
                if (!string.IsNullOrEmpty(dr["FechaRecibido"].ToString()))
                    this.FechaRecibido.Value = Convert.ToDateTime(dr["FechaRecibido"]);
                if (!string.IsNullOrEmpty(dr["FechaPago"].ToString()))
                    this.FechaPago.Value = Convert.ToDateTime(dr["FechaPago"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                if (!string.IsNullOrEmpty(dr["NumDocPresupuesto"].ToString()))
                    this.NumDocPresupuesto.Value = Convert.ToInt32(dr["NumDocPresupuesto"]);
                if (!string.IsNullOrEmpty(dr["Version"].ToString()))
                    this.Version.Value = Convert.ToByte(dr["Version"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
               
                //Adicionales
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);             
                this.TareaCliente.Value = Convert.ToString(dr["TareaCliente"]);
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);   
                if (!string.IsNullOrEmpty(dr["FactorID"].ToString()))
                    this.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);
                this.RecursoDesc.Value = dr["RecursoDesc"].ToString();
                this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                this.CantidadTarea.Value = Convert.ToDecimal(dr["CantidadTarea"]);
                this.TareaDesc.Value = Convert.ToString(dr["TareaDesc"]);   
                this.SelectInd.Value = false;
                this.FijarFechaOCInd.Value = false;
                this.CantidadPend.Value = 0;
                this.CantidadSUM.Value = 0;
                this.CantidadEjec.Value = 0;
                this.CantidadPresup.Value = 0;
                this.CantidadDespacho.Value = 0;
                this.CantidadRecProp.Value = 0;
                this.SolicitadoInd.Value = false;
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
        public DTO_pyProyectoMvto()
        {
            this.InitCols();
            this.FactorID.Value = 0;
            this.CantidadTarea.Value = 0;
            this.CantidadSOL.Value = 0;
            this.CantidadPROV.Value = 0;
            this.CantidadNOM.Value = 0;
            this.CantidadACT.Value = 0;
            this.CantidadINV.Value = 0;           
            this.CantidadBOD.Value = 0;
            this.CantidadREC.Value = 0;            
            this.CostoTotalML.Value = 0;
            this.CantidadPend.Value = 0;
            this.CantidadSUM.Value = 0;
            this.CantidadEjec.Value = 0;
            this.CantidadPresup.Value = 0;
            this.CantidadDespacho.Value = 0;
            this.CantidadRecProp.Value = 0;
            this.VlrPresupuestado.Value = 0;
            this.VlrEjecutado.Value = 0;
            this.VlrPendiente.Value = 0;
            this.PorcentajeEjec.Value = 0;
            this.FijarFechaOCInd.Value = false;
            this.SolicitadoInd.Value = false;
        }

        public void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConsecDeta = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.TipoMvto = new UDTSQL_tinyint();
            this.NumProyecto = new UDT_Consecutivo();
            this.CostoLocal = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();
            this.CantidadTOT = new UDT_Cantidad();
            this.CantidadSOL = new UDT_Cantidad();
            this.CantidadPROV = new UDT_Cantidad();
            this.CantidadNOM = new UDT_Cantidad();
            this.CantidadACT = new UDT_Cantidad();
            this.CantidadINV = new UDT_Cantidad();
            this.CantidadBOD = new UDT_Cantidad();
            this.CantidadREC = new UDT_Cantidad();
            this.Consecutivo = new UDT_Consecutivo();
            this.CostoLocalTOT = new UDT_Valor();
            this.CostoExtraTOT = new UDT_Valor();
            this.AjCambioLocal = new UDT_Valor();
            this.AjCambioExtra = new UDT_Valor();
            this.VPN_ML = new UDT_Valor();
            this.VPN_ME = new UDT_Valor();
            this.FechaInicioTarea = new UDTSQL_smalldatetime();
            this.FechaOrdCompra = new UDTSQL_smalldatetime();
            this.FechaRecibido = new UDTSQL_smalldatetime();
            this.FechaPago = new UDTSQL_smalldatetime();
            this.CodigoBSID = new UDT_CodigoBSID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.NumDocPresupuesto = new UDT_Consecutivo();
            this.Consecutivo = new UDT_Consecutivo();
            this.Observaciones = new UDT_DescripTExt();
            this.Version = new UDTSQL_tinyint();

            //Adicionales
            this.FactorID = new UDT_FactorID();
            this.RecursoID = new UDT_CodigoGrl();
            this.RecursoDesc = new UDT_DescripTExt();
            this.CodigoBSDesc = new UDT_Descriptivo();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.TareaCliente = new UDT_CodigoGrl20();
            this.TareaID = new UDT_CodigoGrl();
            this.TareaDesc = new UDT_DescripTExt();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CantidadTarea = new UDT_Cantidad();
            this.CostoTotalML = new UDT_Valor();
            this.CantidadSUM = new UDT_Cantidad();
            this.CantidadDespacho = new UDT_Cantidad();
            #region Consulta Ejecucion
            this.CantidadPend = new UDT_Cantidad();
            this.CantidadEjec = new UDT_Cantidad();
            this.CantidadPresup = new UDT_Cantidad();
            this.VlrPresupuestado = new UDT_Valor();
            this.VlrEjecutado = new UDT_Valor();
            this.VlrPendiente = new UDT_Valor();
            this.PorcentajeEjec = new UDT_PorcentajeID(); 
            #endregion
            this.TipoRecurso = new UDTSQL_tinyint();
            this.ProveedorID = new UDT_ProveedorID();
            this.FijarFechaOCInd = new UDT_SiNo();
            this.SelectInd = new UDT_SiNo();
            this.SolicitadoInd = new UDT_SiNo();
            this.CantidadRecProp = new UDT_Cantidad();
            this.CantidadSOLRecProp = new UDT_Cantidad();
            this.DetalleTareas = new List<DTO_pyProyectoMvto>();
            this.MarcaInvID = new UDT_Descriptivo();
            this.RefProveedor = new UDT_CodigoGrl20();
            this.ProyectoID = new UDT_ProyectoID();
            this.ConsecTarea = new UDT_Consecutivo();
        }
        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecDeta { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }
                
        [DataMember]
        public UDTSQL_tinyint TipoMvto { get; set; }

        [DataMember]
        public UDT_Consecutivo NumProyecto { get; set; }
        
        [DataMember]
        public UDT_Valor CostoLocal { get; set; }
        
        [DataMember]
        public UDT_Valor CostoExtra { get; set; }
        
        [DataMember]
        public UDT_Cantidad CantidadTOT { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSOL { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadPROV { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadNOM { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadACT { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadINV { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadBOD { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadREC { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalTOT { get; set; }

        [DataMember]
        public UDT_Valor CostoExtraTOT { get; set; }

        [DataMember]
        public UDT_Valor AjCambioLocal { get; set; }

        [DataMember]
        public UDT_Valor AjCambioExtra { get; set; }

        [DataMember]
        public UDT_Valor VPN_ML { get; set; }

        [DataMember]
        public UDT_Valor VPN_ME { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicioTarea { get; set; }	
	
        [DataMember]
        public UDTSQL_smalldatetime FechaOrdCompra { get; set; }	

        [DataMember]
        public UDTSQL_smalldatetime FechaRecibido { get; set; }	

        [DataMember]
        public UDTSQL_smalldatetime FechaPago { get; set; }	

        [DataMember]
        public UDT_Consecutivo NumDocPresupuesto { get; set; }

        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDTSQL_tinyint Version { get; set; }

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        //Adicionales

        [DataMember]
        public UDT_FactorID FactorID { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }

        [DataMember]
        public UDT_DescripTExt RecursoDesc { get; set; }

        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 TareaCliente { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDT_DescripTExt TareaDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo CodigoBSDesc { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadTarea { get; set; }

        [DataMember]
        public UDT_Valor CostoTotalML { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSUM { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadDespacho { get; set; }

        #region Consulta Ejecucion
        [DataMember]
        public UDT_Cantidad CantidadPend { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadEjec { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadPresup { get; set; }

        [DataMember]
        public UDT_Valor VlrPresupuestado { get; set; }

        [DataMember]
        public UDT_Valor VlrEjecutado { get; set; }

        [DataMember]
        public UDT_Valor VlrPendiente { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcentajeEjec { get; set; } 
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoRecurso { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_SiNo FijarFechaOCInd { get; set; }

        [DataMember]
        public UDT_SiNo SelectInd { get; set; }

        [DataMember]
        public string PrefDoc { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadRecProp { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSOLRecProp { get; set; }

        [DataMember]
        public List<DTO_pyProyectoMvto> DetalleTareas { get; set; }

        [DataMember]
        public UDT_SiNo SolicitadoInd { get; set; }

        [DataMember]
        public UDT_Descriptivo MarcaInvID { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecTarea { get; set; }
        #endregion
    }
}
