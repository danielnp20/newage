using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.Librerias.Project;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_plLineaPresupuesto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plLineaPresupuesto : DTO_MasterBasic
    {
        #region DTO_plLineaPresupuesto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plLineaPresupuesto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.RecursoDesc.Value = dr["RecursoDesc"].ToString();
                    this.LineaGrupoDesc.Value = dr["LineaGrupoDesc"].ToString();
                }

                this.RecursoID.Value = dr["RecursoID"].ToString();
                this.LineaGrupoID.Value = dr["LineaGrupoID"].ToString();
                this.IngresosInd.Value = Convert.ToBoolean(dr["IngresosInd"]);
                this.TablaControlInd.Value = Convert.ToBoolean(dr["TablaControlInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ControlCosto"].ToString()))
                    this.ControlCosto.Value = Convert.ToByte(dr["ControlCosto"]);
                if (!string.IsNullOrWhiteSpace(dr["ControlEjecucionPxQ"].ToString()))
                    this.ControlEjecucionPxQ.Value = Convert.ToByte(dr["ControlEjecucionPxQ"]);
                this.ControlCantidadPXQInd.Value = Convert.ToBoolean(dr["ControlCantidadPXQInd"]);
            }
            catch (Exception e)
            {
               throw e ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plLineaPresupuesto() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.RecursoID = new UDT_BasicID();
            this.RecursoDesc = new UDT_Descriptivo();
            this.IngresosInd = new UDT_SiNo();
            this.TablaControlInd = new UDT_SiNo();
            this.ControlCosto = new UDTSQL_tinyint();
            this.ControlEjecucionPxQ = new UDTSQL_tinyint();
            this.ControlCantidadPXQInd = new UDT_SiNo();
            this.LineaGrupoID = new UDT_BasicID();
            this.LineaGrupoDesc = new UDT_Descriptivo();
        }

        public DTO_plLineaPresupuesto(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_plLineaPresupuesto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        public UDT_SiNo IngresosInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint ControlCosto { get; set; }

        [DataMember]
        public UDTSQL_tinyint ControlEjecucionPxQ { get; set; }

        [DataMember]
        public UDT_SiNo ControlCantidadPXQInd { get; set; }
        
        [DataMember]
        public UDT_BasicID LineaGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaGrupoDesc { get; set; }

        //Parametro extra

        [DataMember]
        public UDT_SiNo TablaControlInd { get; set; }

    }
}
