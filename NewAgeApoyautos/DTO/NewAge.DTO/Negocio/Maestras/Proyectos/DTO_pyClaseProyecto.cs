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
    /// Models DTO_pyClaseServicio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyClaseProyecto : DTO_MasterBasic
    {
        #region DTO_pyClaseProyecto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyClaseProyecto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                    this.FacturaTipoDesc.Value = dr["FacturaTipoDesc"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = dr["Observacion"].ToString();

                this.ActividadID.Value = dr["ActividadID"].ToString();
                this.FacturaTipoID.Value = dr["FacturaTipoID"].ToString();

                if (!string.IsNullOrEmpty(dr["RecursosXTrabajoInd"].ToString()))
                    this.RecursosXTrabajoInd.Value = Convert.ToBoolean(dr["RecursosXTrabajoInd"]);
                if (!string.IsNullOrEmpty(dr["TiempoTareaAutInd"].ToString()))
                    this.TiempoTareaAutInd.Value = Convert.ToBoolean(dr["TiempoTareaAutInd"]);
                if (!string.IsNullOrEmpty(dr["TipoPresupuesto"].ToString()))
                    this.TipoPresupuesto.Value = Convert.ToByte(dr["TipoPresupuesto"]);
                if (!string.IsNullOrEmpty(dr["TareaAsociadaInd"].ToString()))
                    this.TareaAsociadaInd.Value = Convert.ToBoolean(dr["TareaAsociadaInd"]);
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyClaseProyecto() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Observacion = new UDT_DescripTExt();
            this.ActividadID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.FacturaTipoID = new UDT_BasicID();
            this.FacturaTipoDesc = new UDT_Descriptivo();
            this.RecursosXTrabajoInd = new UDT_SiNo();
            this.TiempoTareaAutInd = new UDT_SiNo();
            this.TipoPresupuesto = new UDTSQL_tinyint();
            this.TareaAsociadaInd = new UDT_SiNo();
        }

        public DTO_pyClaseProyecto(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyClaseProyecto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_BasicID ActividadID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDT_BasicID FacturaTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo FacturaTipoDesc { get; set; }

        [DataMember]
        public UDT_SiNo RecursosXTrabajoInd { get; set; }

        [DataMember]
        public UDT_SiNo TiempoTareaAutInd  { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoPresupuesto { get; set; }

        [DataMember]
        public UDT_SiNo TareaAsociadaInd { get; set; }
    }

}
