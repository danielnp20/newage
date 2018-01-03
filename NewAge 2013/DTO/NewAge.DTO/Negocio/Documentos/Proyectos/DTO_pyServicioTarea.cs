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
    /// Models DTO_pyServicioTarea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyServicioTarea
    {
        #region DTO_pyServicioTarea
        public DTO_pyServicioTarea(IDataReader dr)
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
                if (!string.IsNullOrEmpty(dr["CostoTotalUnitML"].ToString()))
                    this.CostoTotalUnitML.Value = Convert.ToDecimal(dr["CostoTotalUnitML"]);
                if (!string.IsNullOrEmpty(dr["CostoTotalUnitME"].ToString()))
                    this.CostoTotalUnitME.Value = Convert.ToDecimal(dr["CostoTotalUnitME"]);
                if (!string.IsNullOrEmpty(dr["CostoTotalML"].ToString()))
                    this.CostoTotalML.Value = Convert.ToDecimal(dr["CostoTotalML"]);
                if (!string.IsNullOrEmpty(dr["CostoTotalME"].ToString()))
                    this.CostoTotalME.Value = Convert.ToDecimal(dr["CostoTotalME"]);
                if (!string.IsNullOrEmpty(dr["FechaInicio"].ToString()))
                    this.FechaInicio.Value = Convert.ToDateTime(dr["FechaInicio"]);
                if (!string.IsNullOrEmpty(dr["FechaFin"].ToString()))
                    this.FechaFin.Value = Convert.ToDateTime(dr["FechaFin"]);
                this.Observaciones.Value = Convert.ToString(dr["Observaciones"]);
                if (!string.IsNullOrEmpty(dr["Nivel"].ToString()))
                    this.Nivel.Value = Convert.ToByte(dr["Nivel"]);
                if (!string.IsNullOrEmpty(dr["DetalleInd"].ToString()))
                    this.DetalleInd.Value = Convert.ToBoolean(dr["DetalleInd"]);
                if (!string.IsNullOrEmpty(dr["TareaPadre"].ToString()))
                    this.TareaPadre.Value = Convert.ToString(dr["TareaPadre"]);
                if (!string.IsNullOrEmpty(dr["ImprimirTareaInd"].ToString()))
                    this.ImprimirTareaInd.Value = Convert.ToBoolean(dr["ImprimirTareaInd"]);
                if (!string.IsNullOrEmpty(dr["CostoAdicionalInd"].ToString()))
                    this.CostoAdicionalInd.Value = Convert.ToBoolean(dr["CostoAdicionalInd"]);
                this.VlrAIUxAPUAdmin.Value = 0;
                this.VlrAIUxAPUImpr.Value = 0;
                this.VlrAIUxAPUUtil.Value = 0;
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
        public DTO_pyServicioTarea()
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
        }

        public void InitCols() 
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.TareaID = new UDT_CodigoGrl();
            this.TareaCliente= new UDT_CodigoGrl20();
            this.Descriptivo = new UDT_DescripTExt();
            this.UnidadInvID = new UDT_UnidadInvID();   
            this.Cantidad = new UDT_Cantidad();
            this.Observacion = new UDT_DescripTExt();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.CostoLocalCLI = new UDT_Valor();
            this.CostoExtraCLI = new UDT_Valor();
            this.CostoTotalUnitML = new UDT_Valor();
            this.CostoTotalUnitME = new UDT_Valor();
            this.CostoTotalML = new UDT_Valor();
            this.CostoTotalME = new UDT_Valor();        
            this.FechaInicio = new UDTSQL_smalldatetime();
            this.FechaFin = new UDTSQL_smalldatetime();
            this.Observaciones = new UDT_DescripTExt();
            this.Nivel = new UDTSQL_smallint();
            this.TareaPadre = new UDTSQL_char(20);
            this.DetalleInd = new UDT_SiNo();
            this.ImprimirTareaInd = new UDT_SiNo();
            this.CostoAdicionalInd = new UDT_SiNo();
            this.Consecutivo = new UDT_Consecutivo();
            //Adicionales
            this.TrabajoID = new UDT_CodigoGrl();
            this.TrabajoDesc = new UDT_Descriptivo();
            this.CostoLocalUnitCLI = new UDT_Valor();        
            this.CostoDiferenciaML = new UDT_Valor();
            this.SelectInd = new UDT_SiNo();
            this.Detalle = new List<DTO_pySolServicioDeta>();
            this.CantidadTarea = new UDT_Cantidad();
            this.VlrAIUxAPUAdmin = new UDT_Valor();
            this.VlrAIUxAPUImpr = new UDT_Valor();
            this.VlrAIUxAPUUtil = new UDT_Valor();
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
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_Cantidad CantidadTarea { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoLocalCLI { get; set; }

        [DataMember]
        public UDT_Valor CostoLocalUnitCLI { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt Observacion { get; set; }

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
        public UDT_Valor CostoTotalML { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoTotalME { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoTotalUnitML { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoTotalUnitME { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaInicio { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smalldatetime FechaFin { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        [NotImportable]
        public UDTSQL_smallint Nivel { get; set; } // Numero de nivel

        [DataMember]
        [NotImportable]
        public UDTSQL_char TareaPadre { get; set; } // Descripcion nivel padre

        [DataMember]
        [NotImportable]
        public UDT_SiNo DetalleInd { get; set; } // Si es ultimo nivel 

        [DataMember]
        [NotImportable]
        public UDT_SiNo ImprimirTareaInd { get; set; } // Si es ultimo nivel 

        [DataMember]
        [NotImportable]
        public UDT_SiNo CostoAdicionalInd { get; set; } // Si es ultimo nivel 

        #endregion

        #region Adicionales
        //[DataMember]
        //public UDTSQL_char Borrar { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Descriptivo TrabajoDesc { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor CostoDiferenciaML { get; set; }

        [DataMember]
        [NotImportable]
        public List<DTO_pySolServicioDeta> Detalle { get; set; }

        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_SiNo SelectInd { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAIUxAPUAdmin { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAIUxAPUImpr { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAIUxAPUUtil { get; set; }

        #endregion

    }
}
