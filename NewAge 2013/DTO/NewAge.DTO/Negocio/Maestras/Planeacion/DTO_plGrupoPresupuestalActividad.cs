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
    /// Models DTO_plGrupoPresupuestalActividad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plGrupoPresupuestalActividad : DTO_MasterComplex
    {
        #region DTO_plGrupoPresupuestalActividad
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plGrupoPresupuestalActividad(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.GrupoPresDesc.Value = dr["GrupoPresDesc"].ToString();
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                }

                this.GrupoPresupuestoID.Value = dr["GrupoPresupuestoID"].ToString();
                this.ActividadID.Value = dr["ActividadID"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plGrupoPresupuestalActividad() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.GrupoPresupuestoID = new UDT_BasicID();
            this.GrupoPresDesc = new UDT_Descriptivo();
            this.ActividadID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
        }

        public DTO_plGrupoPresupuestalActividad(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_plGrupoPresupuestalActividad(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
      
        [DataMember]
        public UDT_BasicID GrupoPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoPresDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

    }
}
