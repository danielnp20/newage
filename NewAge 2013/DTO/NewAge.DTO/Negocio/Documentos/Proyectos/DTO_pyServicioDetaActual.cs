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
    /// Models DTO_pyServicioDetaActual
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyServicioDetaActual
    {
        #region pyServicioDetaActual

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyServicioDetaActual(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ConsecPrograma.Value = Convert.ToInt32(dr["ConsecPrograma"]);
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                if (!string.IsNullOrWhiteSpace(dr["CantidadTR"].ToString()))
                    this.CantidadTR.Value = Convert.ToDecimal(dr["CantidadTR"]);
                this.SemanaPrograma.Value = Convert.ToInt32(dr["SemanaPrograma"]);
                if (!string.IsNullOrWhiteSpace(dr["RecursoID"].ToString()))
                    this.RecursoID.Value = Convert.ToString(dr["RecursoID"]);
                this.TipoRecurso.Value = Convert.ToByte(dr["TipoRecurso"]);
                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
                if (!string.IsNullOrWhiteSpace(dr["CodigoBSID"].ToString()))
                    this.CodigoBSID.Value = Convert.ToString(dr["CodigoBSID"]);
                if (!string.IsNullOrWhiteSpace(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = Convert.ToString(dr["inReferenciaID"]);
                if (!string.IsNullOrWhiteSpace(dr["Cantidad"].ToString()))
                    this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                if (!string.IsNullOrWhiteSpace(dr["UnidadInvID"].ToString()))
                    this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);

                if (!string.IsNullOrWhiteSpace(dr["DocCompra"].ToString()))
                    this.DocCompra.Value = Convert.ToInt32(dr["DocCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["PorVariacion"].ToString()))
                    this.PorVariacion.Value = Convert.ToDecimal(dr["PorVariacion"]);

                if (!string.IsNullOrWhiteSpace(dr["CostoLocalPRY"].ToString()))
                    this.CostoLocalPRY.Value = Convert.ToDecimal(dr["CostoLocalPRY"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoExtraPRY"].ToString()))
                    this.CostoExtraPRY.Value = Convert.ToDecimal(dr["CostoExtraPRY"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoLocalEMP"].ToString()))
                    this.CostoLocalEMP.Value = Convert.ToDecimal(dr["CostoLocalEMP"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoExtraEMP"].ToString()))
                    this.CostoExtraEMP.Value = Convert.ToDecimal(dr["CostoExtraEMP"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaIniciaPRO"].ToString()))
                    this.CostoLocal.Value = Convert.ToDecimal(dr["FechaIniciaPRO"]);
                if (!string.IsNullOrWhiteSpace(dr["CostoExtra"].ToString()))
                    this.CostoExtra.Value = Convert.ToDecimal(dr["CostoExtra"]);
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
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
        public DTO_pyServicioDetaActual()
        {
            InitCols();
        }

        public void InitCols()
        {
            this.ConsecPrograma = new UDT_Consecutivo();
            this.NumeroDoc = new UDT_Consecutivo();
            this.CantidadTR = new UDT_Cantidad();
            this.SemanaPrograma = new UDTSQL_int();
            this.RecursoID = new UDT_CodigoGrl();
            this.TipoRecurso = new UDTSQL_int();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID(); 
            this.CodigoBSID = new UDT_CodigoBSID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.Cantidad = new UDT_Cantidad();
            this.UnidadInvID = new UDT_UnidadInvID();
            this.DocCompra = new UDT_Consecutivo();
            this.PorVariacion = new UDT_PorcentajeID();
            this.CostoLocalPRY = new UDT_Valor();
            this.CostoExtraPRY = new UDT_Valor();
            this.CostoLocalEMP = new UDT_Valor();
            this.CostoExtraEMP = new UDT_Valor();
            this.CostoLocal = new UDT_Valor();
            this.CostoExtra = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }

        #endregion

        #region Porpiedades
        
        [DataMember]
        public UDT_Consecutivo ConsecPrograma { get; set; }
        
        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        public UDT_Cantidad CantidadTR { get; set; }
        
        [DataMember]
        public UDTSQL_int SemanaPrograma { get; set; }
        
        [DataMember]
        public UDT_CodigoGrl RecursoID { get; set; }
        
        [DataMember]
        public UDTSQL_int TipoRecurso { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }
        
        [DataMember]
        public UDT_CodigoBSID CodigoBSID { get; set; }
        
        [DataMember]
        public UDT_inReferenciaID inReferenciaID { get; set; }
        
        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }
        
        [DataMember]
        public UDT_UnidadInvID UnidadInvID { get; set; }

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


        #endregion 

    }
}
