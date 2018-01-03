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
    /// Models DTO_pyProyectoDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoDeta
    {
        #region pyProyectoDeta

        public DTO_pyProyectoDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TrabajoID.Value = Convert.ToString(dr["TrabajoID"]);
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                if (!string.IsNullOrEmpty(dr["FactorID"].ToString()))
                    this.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);
                if (!string.IsNullOrEmpty(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                if (!string.IsNullOrEmpty(dr["CantSolicitud"].ToString()))
                    this.CantSolicitud.Value = Convert.ToDecimal(dr["CantSolicitud"]);
                if (!string.IsNullOrEmpty(dr["Peso_Cantidad"].ToString()))
                    this.Peso_Cantidad.Value = Convert.ToDecimal(dr["Peso_Cantidad"]);
                if (!string.IsNullOrEmpty(dr["Distancia_Turnos"].ToString()))
                    this.Distancia_Turnos.Value = Convert.ToDecimal(dr["Distancia_Turnos"]);
                if (!string.IsNullOrWhiteSpace(dr["DocCompra"].ToString()))
                    this.DocCompra.Value = Convert.ToInt32(dr["DocCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["PorVariacion"].ToString()))
                    this.PorVariacion.Value = Convert.ToDecimal(dr["PorVariacion"]);
                if (!string.IsNullOrEmpty(dr["CostoLocalPRY"].ToString()))
                    this.CostoLocalPRY.Value = Convert.ToDecimal(dr["CostoLocalPRY"]);
                if (!string.IsNullOrEmpty(dr["CostoExtraPRY"].ToString()))
                    this.CostoExtraPRY.Value = Convert.ToDecimal(dr["CostoExtraPRY"]);
                if (!string.IsNullOrEmpty(dr["CostoLocalEMP"].ToString()))
                    this.CostoLocalEMP.Value = Convert.ToDecimal(dr["CostoLocalEMP"]);
                if (!string.IsNullOrEmpty(dr["CostoExtraEMP"].ToString()))
                    this.CostoExtraEMP.Value = Convert.ToDecimal(dr["CostoExtraEMP"]);
                if (!string.IsNullOrEmpty(dr["CostoLocal"].ToString()))
                    this.CostoLocal.Value = Convert.ToDecimal(dr["CostoLocal"]);
                if (!string.IsNullOrEmpty(dr["CostoExtra"].ToString()))
                    this.CostoExtra.Value = Convert.ToDecimal(dr["CostoExtra"]);
                if (!string.IsNullOrEmpty(dr["CantidadTOT"].ToString()))
                    this.CantidadTOT.Value = Convert.ToDecimal(dr["CantidadTOT"]);
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
                if (!string.IsNullOrEmpty(dr["TiempoTotal"].ToString()))
                    this.TiempoTotal.Value = Convert.ToDecimal(dr["TiempoTotal"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                if (!string.IsNullOrEmpty(dr["FechaTermina"].ToString()))
                    this.FechaTermina.Value = Convert.ToDateTime(dr["FechaTermina"]);
                if (!string.IsNullOrEmpty(dr["FechaInicioAUT"].ToString()))
                    this.FechaInicioAUT.Value = Convert.ToDateTime(dr["FechaInicioAUT"]);
                if (!string.IsNullOrEmpty(dr["FechaFijadaInd"].ToString()))
                    this.FechaFijadaInd.Value = Convert.ToBoolean(dr["FechaFijadaInd"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                //Adicionales
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                if (!string.IsNullOrEmpty(dr["TipoRecurso"].ToString()))
                    this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                this.CostoLocConPrestacion.Value = this.CostoLocal.Value;
                this.CostoLocalInicial.Value = this.CostoLocal.Value;
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        public DTO_pyProyectoDeta(IDataReader dr, bool componenteInd)
        {
            InitCols();
            try
            {
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                if (!string.IsNullOrEmpty(dr["CostoLocal"].ToString()))
                    this.CostoLocal.Value = Convert.ToDecimal(dr["CostoLocal"]);
                if (!string.IsNullOrEmpty(dr["CostoExtra"].ToString()))
                    this.CostoExtra.Value = Convert.ToDecimal(dr["CostoExtra"]); 
                this.CostoLocConPrestacion.Value = this.CostoLocal.Value;
                this.CostoLocalInicial.Value = this.CostoLocal.Value;
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
        public DTO_pyProyectoDeta()
        {
            this.InitCols();
            this.CostoLocalPRY.Value = 0;
            this.CostoExtraPRY.Value = 0;
            this.CostoLocalEMP.Value = 0;
            this.CostoExtraEMP.Value = 0;
            this.CostoLocal.Value = 0;
            this.CostoExtra.Value = 0;
            this.CostoLocalTOT.Value = 0;
            this.CostoExtraTOT.Value = 0;
            this.CantDespacho.Value = 0;
            this.CantExistencias.Value = 0;
            this.CantidadPROV.Value = 0;
            this.CantidadRecProp.Value = 0;
            this.CantidadSOLRecProp.Value = 0;
            this.CantidadStock.Value = 0;
            this.CantPreproyecto.Value = 0;
            this.VlrUnitPreproyecto.Value = 0;
            this.VlrTotPreproyecto.Value = 0;
            this.VlrUnitCLIPreproyecto.Value = 0;
            this.VlrTotCLIPreproyecto.Value = 0;
        }

        public void InitCols()
        {
            this.ConsecTarea = new UDT_Consecutivo();
            this.TrabajoID = new UDT_CodigoGrl();
            this.RecursoID = new UDT_CodigoGrl();           
            this.NumeroDoc = new UDT_Consecutivo();    
            this.FactorID = new UDT_FactorID();
            this.Cantidad = new UDT_Cantidad();
            this.CantSolicitud = new UDT_Cantidad();
            this.Peso_Cantidad = new UDT_Cantidad();
            this.Distancia_Turnos = new UDT_Cantidad();
            this.DocCompra = new UDT_Consecutivo();
            this.PorVariacion = new UDT_PorcentajeID();
            this.CostoLocalPRY = new UDT_Valor();
            this.CostoExtraPRY = new UDT_Valor();
            this.CostoLocalEMP = new UDT_Valor();
            this.CostoExtraEMP = new UDT_Valor();
            this.CostoLocal = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
            this.CantidadTOT = new UDT_Cantidad();
            this.CostoLocalTOT = new UDT_Valor();
            this.CostoExtraTOT = new UDT_Valor();
            this.AjCambioLocal = new UDT_Valor();
            this.AjCambioExtra = new UDT_Valor();
            this.VPN_ML = new UDT_Valor();
            this.VPN_ME = new UDT_Valor();
            this.TiempoTotal = new UDT_Cantidad();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.FechaTermina = new UDTSQL_smalldatetime();
            this.Observaciones = new UDT_DescripTExt();
            this.FechaFijadaInd = new UDT_SiNo();
            this.FechaInicioAUT = new UDTSQL_smalldatetime();
            //Adicionales           
            this.Version = new UDTSQL_tinyint();
            this.RecursoDesc = new UDT_DescripUnFormat();
            this.ProveedorID = new UDT_ProveedorID();
            this.TareaID = new UDT_CodigoGrl();
            this.TipoRecurso = new UDTSQL_tinyint();
            this.SelectInd = new UDT_SiNo();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.CostoLocalInicial = new UDT_Valor();
            this.CostoLocConPrestacion = new UDT_Valor();
            this.CantidadSOLRecProp = new UDT_Cantidad();
            this.CantidadRecProp = new UDT_Cantidad();
            this.CantExistencias = new UDT_Cantidad();
            this.CantDespacho = new UDT_Cantidad();
            this.CantidadPROV = new UDT_Cantidad();
            this.CantidadStock = new UDT_Cantidad();
            this.TareaCliente = new UDT_TareaID();
            this.DiasAtraso = new UDT_Cantidad();
            this.DetalleMvto = new List<DTO_pyProyectoMvto>();
            this.VlrAIUAdmin = new UDT_Valor();
            this.VlrAIUImpr = new UDT_Valor();
            this.VlrAIUUtil = new UDT_Valor();
            this.MarcaDesc = new UDT_Descriptivo();
            this.Modelo = new UDT_CodigoGrl20();
            this.CostoLocalCLI = new UDT_Valor();
            this.CostoLocalTOTCLI = new UDT_Valor();
            this.CostoLocalDiferencia = new UDT_Valor();
            this.CantPreproyecto = new UDT_Cantidad();
            this.VlrUnitPreproyecto = new UDT_Valor();
            this.VlrTotPreproyecto = new UDT_Valor();
            this.VlrUnitCLIPreproyecto = new UDT_Valor();
            this.VlrTotCLIPreproyecto = new UDT_Valor();
            //Ejecucion
            this.CantidadEjec = new UDT_Cantidad();     
            this.CostoLocalEjec = new UDT_Valor();
            this.SubTotalLocalEjec = new UDT_Valor();
            this.CostoLocalIVATOTEjec = new UDT_Valor();
            this.CostoLocalTOTEjec = new UDT_Valor();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo ConsecTarea { get; set; }

        [DataMember]
        public UDT_CodigoGrl TrabajoID { get; set; }

        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_FactorID FactorID { get; set; }
        
        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Cantidad CantSolicitud { get; set; }        

        [DataMember]
        public UDT_Cantidad Peso_Cantidad { get; set; } 

        [DataMember]
        public UDT_Cantidad Distancia_Turnos { get; set; } 

        [DataMember]
        public UDT_Consecutivo DocCompra { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorVariacion { get; set; }
        
        [DataMember]
        public UDT_Valor CostoLocalPRY { get; set; }
        
        [DataMember]
        public UDT_Valor CostoExtraPRY { get; set; }
        
        [DataMember]
        public UDT_Valor CostoLocalEMP { get; set; }
        
        [DataMember]
        public UDT_Valor CostoExtraEMP { get; set; }
        
        [DataMember]
        public UDT_Valor CostoLocal { get; set; }
        
        [DataMember]
        public UDT_Valor CostoExtra { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadTOT { get; set; }

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
        public UDT_Cantidad TiempoTotal { get; set; }
	
        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }
	
        [DataMember]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaTermina { get; set; }
	
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicioAUT { get; set; }

        [DataMember]
        public UDT_SiNo FechaFijadaInd { get; set; }	

        #endregion

        #region Campos Extras

        [DataMember]
        public UDTSQL_tinyint Version { get; set; } 

        [DataMember]
        public UDT_DescripUnFormat RecursoDesc { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoRecurso { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_SiNo SelectInd { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalInicial { get; set; }

        [DataMember]
        public UDT_Valor CostoLocConPrestacion { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadSOLRecProp { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadRecProp { get; set; }

        [DataMember]
        public UDT_Cantidad CantExistencias { get; set; }

        [DataMember]
        public UDT_Cantidad CantDespacho { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadStock { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadPROV { get; set; }

        [DataMember]
        public UDT_TareaID TareaCliente { get; set; }

        [DataMember]
        public UDT_Cantidad DiasAtraso { get; set; }

        [DataMember]
        public  List<DTO_pyProyectoMvto> DetalleMvto { get; set; }

        [DataMember]
        public UDT_Valor VlrAIUAdmin { get; set; }

        [DataMember]
        public UDT_Valor VlrAIUImpr { get; set; }

        [DataMember]
        public UDT_Valor VlrAIUUtil { get; set; }

        [DataMember]
        public UDT_Descriptivo MarcaDesc { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 Modelo { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalCLI { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalTOTCLI { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalDiferencia { get; set; }

        [DataMember]
        public UDT_Cantidad CantPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrUnitPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrTotPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrUnitCLIPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrTotCLIPreproyecto { get; set; }

        #region Campos para Ejecucion

        [DataMember]
        public UDT_Cantidad CantidadEjec { get; set; }
        
        [DataMember]
        public UDT_Valor CostoLocalEjec { get; set; }

        [DataMember]
        public UDT_Valor SubTotalLocalEjec { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalIVATOTEjec { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalTOTEjec { get; set; }

        [DataMember]
        public string NroFactura { get; set; }

        [DataMember]
        public string NroOrdCompra { get; set; }

        [DataMember]
        public string ProveedorDesc { get; set; }

        [DataMember]
        public string Estado { get; set; }

        #endregion
        
        #endregion

    }
}
