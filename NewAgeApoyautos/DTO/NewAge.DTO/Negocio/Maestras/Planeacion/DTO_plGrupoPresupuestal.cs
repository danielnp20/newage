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
    /// Models plGrupoPresupuestal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plGrupoPresupuestal : DTO_MasterBasic
    {
        #region plGrupoPresupuestal
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plGrupoPresupuestal(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.TipoControl.Value = Convert.ToByte(dr["TipoControl"]);
                this.AplicaSoloPresupuestoInd.Value = Convert.ToBoolean(dr["AplicaSoloPresupuestoInd"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plGrupoPresupuestal() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoControl = new UDTSQL_tinyint();
            this.AplicaSoloPresupuestoInd = new UDT_SiNo();
        }

        public DTO_plGrupoPresupuestal(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_plGrupoPresupuestal(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDTSQL_tinyint TipoControl { get; set; }

        [DataMember]
        public UDT_SiNo AplicaSoloPresupuestoInd { get; set; }
        
    }

}
