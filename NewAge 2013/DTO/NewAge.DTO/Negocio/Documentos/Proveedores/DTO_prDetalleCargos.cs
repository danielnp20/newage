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
    /// Class recibidos de bienes y servicios:
    /// Models DTO_prDetalleCargos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_prDetalleCargos
    {
        #region DTO_prDetalleCargos

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_prDetalleCargos(IDataReader dr)
        {
            InitCols();
            try
            {
                this.ConsecutivoDetaID.Value = Convert.ToInt32(dr["ConsecutivoDetaID"]);
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
                this.PorcentajeID.Value = Convert.ToDecimal(dr["PorcentajeID"]);
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.ProyectoID.Value = dr["ProyectoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.vlrMdaLoc.Value = Convert.ToDecimal(dr["vlrMdaLoc"]);
                this.vlrMdaExt.Value = Convert.ToDecimal(dr["vlrMdaExt"]);                             
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_prDetalleCargos()
        {
            this.InitCols();
        }

        /// <summary
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConsecutivoDetaID = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.PorcentajeID = new UDT_PorcentajeID();
            this.CuentaID = new UDT_CuentaID();
            this.TerceroID = new UDT_TerceroID();            
            this.ProyectoID = new UDT_ProyectoID();
            this.CentroCostoID = new UDT_CentroCostoID();
            this.LineaPresupuestoID = new UDT_LineaPresupuestoID();
            this.ConceptoCargoID = new UDT_ConceptoCargoID();
            this.vlrMdaLoc = new UDT_Valor();
            this.vlrMdaExt = new UDT_Valor();          
        }
        #endregion

        #region Propiedades
        [DataMember]
        [NotImportable]
        public int Index { get; set; }

        [DataMember]
        [NotImportable]
        public int IndexDet { get; set; }

        [DataMember]
        public UDT_Consecutivo ConsecutivoDetaID { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcentajeID { get; set; }

        [DataMember]
        public UDT_CuentaID CuentaID { get; set; }

        [DataMember]
        public UDT_TerceroID TerceroID { get; set; }

        [DataMember]
        public UDT_ProyectoID ProyectoID { get; set; }

        [DataMember]
        public UDT_CentroCostoID CentroCostoID { get; set; }

        [DataMember]
        public UDT_LineaPresupuestoID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_ConceptoCargoID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaLoc { get; set; }

        [DataMember]
        public UDT_Valor vlrMdaExt { get; set; }

        #endregion
    }
}

