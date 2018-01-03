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
    /// 
    /// Models DTO_pyProyectoTarea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyProyectoTarea
    {
        #region DTO_pyProyectoTarea
        public DTO_pyProyectoTarea(IDataReader dr)
        {
            InitCols();
            try
            {    
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
                this.TareaID.Value = Convert.ToString(dr["TareaID"]);
                this.TareaCliente.Value = Convert.ToString(dr["TareaCliente"]);
                this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                if (!string.IsNullOrEmpty(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                this.CentroCostoID.Value = Convert.ToString(dr["CentroCostoID"]);
                if (!string.IsNullOrEmpty(dr["CostoLocalCLI"].ToString()))
                    this.CostoLocalCLI.Value = Convert.ToDecimal(dr["CostoLocalCLI"]);
                if (!string.IsNullOrEmpty(dr["CostoExtraCLI"].ToString()))
                    this.CostoExtraCLI.Value = Convert.ToDecimal(dr["CostoExtraCLI"]);
                if (!string.IsNullOrEmpty(dr["CostoLocalUnitCLI"].ToString()))
                    this.CostoLocalUnitCLI.Value = Convert.ToDecimal(dr["CostoLocalUnitCLI"]);
                if (!string.IsNullOrEmpty(dr["CostoExtraUnitCLI"].ToString()))
                    this.CostoExtraUnitCLI.Value = Convert.ToDecimal(dr["CostoExtraUnitCLI"]);
                if (!string.IsNullOrEmpty(dr["CostoTotalUnitML"].ToString()))
                    this.CostoTotalUnitML.Value = Convert.ToDecimal(dr["CostoTotalUnitML"]);
                if (!string.IsNullOrEmpty(dr["CostoTotalUnitME"].ToString()))
                    this.CostoTotalUnitME.Value = Convert.ToDecimal(dr["CostoTotalUnitME"]);
                if (!string.IsNullOrEmpty(dr["CostoTotalML"].ToString()))
                    this.CostoTotalML.Value = Convert.ToDecimal(dr["CostoTotalML"]);
                if (!string.IsNullOrEmpty(dr["CostoTotalME"].ToString()))
                    this.CostoTotalME.Value = Convert.ToDecimal(dr["CostoTotalME"]);
                if (!string.IsNullOrEmpty(dr["AjCambioLocal"].ToString()))
                    this.AjCambioLocal.Value = Convert.ToDecimal(dr["AjCambioLocal"]);
                if (!string.IsNullOrEmpty(dr["AjCambioExtra"].ToString()))
                    this.AjCambioExtra.Value = Convert.ToDecimal(dr["AjCambioExtra"]);
                if (!string.IsNullOrEmpty(dr["VPN_ML"].ToString()))
                    this.VPN_ML.Value = Convert.ToDecimal(dr["VPN_ML"]);
                if (!string.IsNullOrEmpty(dr["VPN_ME"].ToString()))
                    this.VPN_ME.Value = Convert.ToDecimal(dr["VPN_ME"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                if (!string.IsNullOrEmpty(dr["FechaTermina"].ToString()))
                    this.FechaTermina.Value = Convert.ToDateTime(dr["FechaTermina"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrEmpty(dr["Nivel"].ToString()))
                    this.Nivel.Value = Convert.ToByte(dr["Nivel"]);
                if (!string.IsNullOrEmpty(dr["DetalleInd"].ToString()))
                    this.DetalleInd.Value = Convert.ToBoolean(dr["DetalleInd"]);
                if (!string.IsNullOrEmpty(dr["TareaPadre"].ToString()))
                    this.TareaPadre.Value = Convert.ToString(dr["TareaPadre"]);
                if (!string.IsNullOrEmpty(dr["TareaEntregable"].ToString()))
                    this.TareaEntregable.Value = Convert.ToString(dr["TareaEntregable"]);
                if (!string.IsNullOrEmpty(dr["ImprimirTareaInd"].ToString()))
                    this.ImprimirTareaInd.Value = Convert.ToBoolean(dr["ImprimirTareaInd"]);
                if (!string.IsNullOrEmpty(dr["CostoAdicionalInd"].ToString()))
                    this.CostoAdicionalInd.Value = Convert.ToBoolean(dr["CostoAdicionalInd"]);
                this.UsuarioID.Value = Convert.ToString(dr["UsuarioID"]);
                if (!string.IsNullOrEmpty(dr["PorDescuento"].ToString()))
                    this.PorDescuento.Value = Convert.ToDecimal(dr["PorDescuento"]);
                if (!string.IsNullOrEmpty(dr["ConsEntrega"].ToString()))
                    this.ConsEntrega.Value = Convert.ToInt32(dr["ConsEntrega"]);                                
                this.VlrAIUxAPUAdmin.Value = 0;
                this.VlrAIUxAPUImpr.Value = 0;
                this.VlrAIUxAPUUtil.Value = 0;
                this.CantidadREC.Value = 0;
                this.Select.Value = false;
                this.FijadoInd.Value = false;
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
        public DTO_pyProyectoTarea()
        {
            InitCols();
            this.CostoLocalCLI.Value = 0;
            this.CostoExtraCLI.Value = 0;
            this.CostoTotalUnitML.Value = 0;
            this.CostoTotalUnitME.Value = 0;
            this.CostoTotalML.Value = 0;
            this.CostoTotalME.Value = 0;
            this.CostoDiferenciaML.Value = 0;
            this.VlrAIUxAPUAdmin.Value = 0;
            this.VlrAIUxAPUImpr.Value = 0;
            this.VlrAIUxAPUUtil.Value = 0;
            this.Index = 0;
            this.DetalleInd.Value = false;
            this.ImprimirTareaInd.Value = true;
            this.CostoAdicionalInd.Value = false;
            this.TituloPrintInd.Value = false;
            this.CantidadREC.Value = 0;
            this.Select.Value = false;
            this.FijadoInd.Value = false;
            //Preproyecto
            this.VlrUnitPreproyecto.Value = 0;
            this.VlrTotPreproyecto.Value = 0;
            this.VlrUnitCLIPreproyecto.Value = 0;
            this.VlrTotCLIPreproyecto.Value =0;
            //Costos Proyecto
            this.SubTotalLocalEjec.Value =0;
            this.CostoLocalIVATOTEjec.Value = 0;
            this.CostoLocalTOTEjec.Value =0;
        }

        public void InitCols() 
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.TareaID = new UDT_CodigoGrl();
            this.TareaCliente= new UDT_CodigoGrl20();
            this.Descriptivo = new UDT_DescripUnFormat();
            this.UnidadInvID = new UDT_UnidadInvID();   
            this.Cantidad = new UDT_Cantidad();
            this.Observacion = new UDT_DescripUnFormat();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.CostoLocalCLI = new UDT_Valor();
            this.CostoExtraCLI = new UDT_Valor();
            this.CostoLocalUnitCLI = new UDT_Valor();
            this.CostoExtraUnitCLI = new UDT_Valor();  
            this.CostoTotalUnitML = new UDT_Valor();
            this.CostoTotalUnitME = new UDT_Valor();
            this.CostoTotalML = new UDT_Valor();
            this.CostoTotalME = new UDT_Valor();
            this.AjCambioLocal = new UDT_Valor();
            this.AjCambioExtra = new UDT_Valor();
            this.VPN_ML = new UDT_Valor();
            this.VPN_ME = new UDT_Valor();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.Observaciones = new UDT_DescripUnFormat();
            this.Nivel = new UDTSQL_smallint();
            this.TareaPadre = new UDTSQL_char(20);
            this.TareaEntregable = new UDT_CodigoGrl20();
            this.DetalleInd = new UDT_SiNo();
            this.ImprimirTareaInd = new UDT_SiNo();
            this.CostoAdicionalInd = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
            this.UsuarioID = new UDT_UsuarioID();
            this.PorDescuento = new UDT_PorcentajeID();
            this.ConsEntrega = new UDT_Consecutivo();
            //Adicionales+
            this.Version = new UDTSQL_tinyint();
            this.Detalle = new List<DTO_pyProyectoDeta>();
            this.DetalleAPUCliente = new List<DTO_pyProyectoDeta>();
            this.TrabajoID = new UDT_CodigoGrl();             
            this.CostoDiferenciaML = new UDT_Valor();
            this.CantidadTarea = new UDT_Cantidad();
            this.CantidadREC = new UDT_Cantidad();
            this.CantidadStock = new UDT_Cantidad();          
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.CapituloTareaID = new UDT_CodigoGrl();
            this.CapituloDesc = new UDT_Descriptivo();
            this.CapituloGrupoID = new UDT_CodigoGrl();
            this.FechaTermina = new UDTSQL_smalldatetime();
            this.DiasAtraso = new UDT_Cantidad();
            this.VlrAIUxAPUAdmin = new UDT_Valor();
            this.VlrAIUxAPUImpr = new UDT_Valor();
            this.VlrAIUxAPUUtil = new UDT_Valor();
            this.TituloPrintInd = new UDT_SiNo();
            this.CapituloTareaID = new UDT_CodigoGrl();
            this.CapituloDesc = new UDT_Descriptivo();
            this.Select = new UDT_SiNo();
            this.FijadoInd = new UDT_SiNo();
            this.MarcaInvID = new UDT_Descriptivo();
            this.RefProveedor = new UDT_CodigoGrl20();
            #region Consulta Ejecucion
            this.CantidadPend = new UDT_Cantidad();
            this.CantidadEjec = new UDT_Cantidad();
            this.CantidadPresup = new UDT_Cantidad();
            this.VlrPresupuestado = new UDT_Valor();
            this.VlrEjecutado = new UDT_Valor();
            this.VlrPendiente = new UDT_Valor();
            this.PorcentajeEjec = new UDT_PorcentajeID();
            //Preproyecto
            this.VlrUnitPreproyecto = new UDT_Valor();
            this.VlrTotPreproyecto = new UDT_Valor();
            this.VlrUnitCLIPreproyecto = new UDT_Valor();
            this.VlrTotCLIPreproyecto = new UDT_Valor();
            //Costos Proyecto
            this.SubTotalLocalEjec = new UDT_Valor();
            this.CostoLocalIVATOTEjec = new UDT_Valor();
            this.CostoLocalTOTEjec = new UDT_Valor();
            #endregion
        }

        #endregion

        #region Propiedades

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_CodigoGrl20 TareaCliente { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl TrabajoID { get; set; }

        [DataMember]
        public UDT_DescripUnFormat Descriptivo { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoLocalCLI { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalUnitCLI { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoExtraUnitCLI { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripUnFormat Observacion { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoExtraCLI { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoTotalUnitML { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoTotalUnitME { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoTotalML { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoTotalME { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor AjCambioLocal { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor AjCambioExtra { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VPN_ML { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VPN_ME { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaTermina { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripUnFormat Observaciones { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint Nivel { get; set; } // Numero de nivel

        [DataMember]
        [NotImportable]
        public UDTSQL_char TareaPadre { get; set; } // Descripcion nivel padre

        [DataMember]
        public UDT_CodigoGrl20 TareaEntregable { get; set; } //Tarea para Entregar

        [DataMember]
        [NotImportable]
        public UDT_SiNo DetalleInd { get; set; } // Si es ultimo nivel 

        [DataMember]
        [NotImportable]
        public UDT_SiNo ImprimirTareaInd { get; set; } // Si es ultimo nivel 

        [DataMember]
        [NotImportable]
        public UDT_SiNo CostoAdicionalInd { get; set; } // Si es ultimo nivel 

        [DataMember]
        [NotImportable]
        public UDT_UsuarioID UsuarioID { get; set; } // Usuario Actual

        [DataMember]
        [NotImportable]
        public UDT_PorcentajeID PorDescuento { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo ConsEntrega { get; set; }

        #endregion

        #region Adicionales

        [DataMember]
        [NotImportable]
        public UDTSQL_tinyint Version { get; set; } 

        [DataMember]
        [NotImportable]
        public UDT_SiNo TituloPrintInd { get; set; } // Si imprime titulos

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoDiferenciaML { get; set; }

        [DataMember]
        [NotImportable]
        public List<DTO_pyProyectoDeta> Detalle { get; set; }

        [DataMember]
        [NotImportable]
        public List<DTO_pyProyectoDeta> DetalleAPUCliente { get; set; }

        [DataMember]
        [NotImportable]
        public int? Index { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadTarea { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadREC { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad CantidadStock { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl CapituloTareaID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo CapituloDesc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl CapituloGrupoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad DiasAtraso { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAIUxAPUAdmin { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAIUxAPUImpr { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAIUxAPUUtil { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo Select { get; set; }//Seleccionar para incluir en PlanEntregas

        [DataMember]
        [NotImportable]
        public UDT_SiNo FijadoInd { get; set; } //Para saber si ya se incluyo en planEntregas

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo MarcaInvID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_CodigoGrl20 RefProveedor { get; set; }

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

        //Preproyecto: campos para sumatorias del reporte Control de Costos

        [DataMember]
        public UDT_Valor VlrUnitPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrTotPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrUnitCLIPreproyecto { get; set; }

        [DataMember]
        public UDT_Valor VlrTotCLIPreproyecto { get; set; }

        //Costos Proyecto : campos para sumatorias del reporte Control de Costos

        [DataMember]
        public UDT_Valor SubTotalLocalEjec { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalIVATOTEjec { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalTOTEjec { get; set; }

        #endregion

        #endregion

    }
}
