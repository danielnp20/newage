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
    /// Models DTO_plServicio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plServicio : DTO_MasterBasic
    {
        #region DTO_plServicio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plServicio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if(!isReplica)
                    this.LineaPresupuestoDesc.Value = Convert.ToString(dr["LineaPresupuestoDesc"]);

                this.LineaPresupuestoID.Value = Convert.ToString(dr["LineaPresupuestoID"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plServicio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
        }

        public DTO_plServicio(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_plServicio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }



    }

}
