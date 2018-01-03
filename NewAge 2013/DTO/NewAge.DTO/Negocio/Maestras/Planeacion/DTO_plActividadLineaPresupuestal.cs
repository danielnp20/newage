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
    /// Models DTO_plActividadLineaPresupuestal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plActividadLineaPresupuestal : DTO_MasterComplex
    {
        #region DTO_plActividadLineaPresupuestal
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plActividadLineaPresupuestal(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                    this.RecursoDesc.Value = dr["RecursoDesc"].ToString();
                }

                this.ActividadID.Value = dr["ActividadID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.RecursoID.Value = dr["RecursoID"].ToString();
                this.ControlCosto.Value = Convert.ToByte(dr["ControlCosto"]);
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plActividadLineaPresupuestal() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ActividadID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.RecursoID = new UDT_BasicID();
            this.RecursoDesc = new UDT_Descriptivo();
            this.ControlCosto = new UDTSQL_tinyint();
        }

        public DTO_plActividadLineaPresupuestal(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_plActividadLineaPresupuestal(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
      
        [DataMember]
        public UDT_BasicID ActividadID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_BasicID RecursoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint ControlCosto { get; set; }
    }
}
