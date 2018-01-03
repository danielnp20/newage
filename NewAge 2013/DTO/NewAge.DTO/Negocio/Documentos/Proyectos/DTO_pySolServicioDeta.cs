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
    /// Models DTO_pySolServicioDeta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pySolServicioDeta
    {
        #region pySolServicioDeta

        public DTO_pySolServicioDeta(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ConsecTarea.Value = Convert.ToInt32(dr["ConsecTarea"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.TrabajoID.Value = Convert.ToString(dr["TrabajoID"]);
                this.TrabajoDesc.Value = Convert.ToString(dr["TrabajoDesc"]);
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
                if (!string.IsNullOrEmpty(dr["TiempoTotal"].ToString()))
                    this.TiempoTotal.Value = Convert.ToDecimal(dr["TiempoTotal"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrEmpty(dr["FechaInicioAUT"].ToString()))
                    this.FechaInicioAUT.Value = Convert.ToDateTime(dr["FechaInicioAUT"]);
                if (!string.IsNullOrEmpty(dr["FechaFijadaInd"].ToString()))
                    this.FechaFijadaInd.Value = Convert.ToBoolean(dr["FechaFijadaInd"]);
                //Adicionales
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                if (!string.IsNullOrEmpty(dr["TipoRecurso"].ToString()))
                    this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                this.CostoLocalInicial.Value = this.CostoLocal.Value;
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        public DTO_pySolServicioDeta(IDataReader dr, bool componenteInd)
        {
            InitCols();
            try
            {
                this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.RecursoDesc.Value = Convert.ToString(dr["RecursoDesc"]);
                //this.TrabajoID.Value = Convert.ToString(dr["TrabajoID"]);
                //this.TrabajoDesc.Value = Convert.ToString(dr["TrabajoDesc"]);
                //this.TareaID.Value = Convert.ToString(dr["TrabajoID"]);
                this.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);
                this.CostoLocal.Value = Convert.ToDecimal(dr["CostoLocal"]);
                this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
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
        public DTO_pySolServicioDeta()
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
            this.TiempoTotal = new UDT_Cantidad();
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.Observaciones = new UDT_DescripTExt();
            this.FechaFijadaInd = new UDT_SiNo();
            this.FechaInicioAUT = new UDTSQL_smalldatetime();

            //Adicionales
            this.RecursoDesc = new UDT_Descriptivo();
            this.TrabajoDesc = new UDT_Descriptivo();
            this.MonedaID = new UDT_MonedaID();
            this.ProveedorID = new UDT_ProveedorID();
            this.TareaID = new UDT_CodigoGrl();
            this.TipoRecurso = new UDTSQL_tinyint();
            this.SelectInd = new UDT_SiNo();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.SelectInd.Value = false;
            this.CostoLocalInicial = new UDT_Valor();
            this.VlrAIUAdmin = new UDT_Valor();
            this.VlrAIUImpr = new UDT_Valor();
            this.VlrAIUUtil = new UDT_Valor();
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
        public UDT_Cantidad TiempoTotal { get; set; }
	
        [DataMember]
        public UDTSQL_smalldatetime FechaInicio { get; set; }
	
        [DataMember]
        public UDTSQL_smalldatetime FechaFin { get; set; }	
	
        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaInicioAUT { get; set; }

        [DataMember]
        public UDT_SiNo FechaFijadaInd { get; set; }	

        #endregion

        #region Campos Extras

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        public UDT_Descriptivo TrabajoDesc { get; set; }

        [DataMember]
        public UDT_ProveedorID ProveedorID { get; set; }
        
        [DataMember]
        public UDT_MonedaID MonedaID { get; set; }

        [DataMember]
        public UDT_CodigoGrl TareaID { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoRecurso { get; set; }

        [DataMember]
        public UDT_SiNo SelectInd { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalInicial { get; set; }

        [DataMember]
        public UDT_Valor VlrAIUAdmin { get; set; }

        [DataMember]
        public UDT_Valor VlrAIUImpr { get; set; }

        [DataMember]
        public UDT_Valor VlrAIUUtil { get; set; }
        
        #endregion
    }
}
