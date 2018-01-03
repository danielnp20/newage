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
    /// Models DTO_plGrupoPresupuestalLinea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plGrupoPresupuestalLinea : DTO_MasterComplex
    {
        #region DTO_plGrupoPresupuestalLinea
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plGrupoPresupuestalLinea(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.GrupoPresDesc.Value = dr["GrupoPresDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                }

                this.GrupoPresupuestoID.Value = dr["GrupoPresupuestoID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plGrupoPresupuestalLinea() : base()
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
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.CentroCostoID = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
        }

        public DTO_plGrupoPresupuestalLinea(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_plGrupoPresupuestalLinea(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
      
        [DataMember]
        public UDT_BasicID GrupoPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoPresDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

    }
}
