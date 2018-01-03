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
    /// Models DTO_pyTrabajoFlujo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTrabajoFlujo : DTO_MasterBasic
    {
        #region pyTrabajoFlujo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTrabajoFlujo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
  
                //if (!string.IsNullOrEmpty(dr["UnidadInvID"].ToString()))
                    //this.UnidadInvID.Value = Convert.ToString(dr["UnidadInvID"]);
   
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyTrabajoFlujo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //this.UnidadInvID = new UDT_BasicID();
        }

        public DTO_pyTrabajoFlujo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTrabajoFlujo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        //[DataMember]
        //public UDT_BasicID UnidadInvID { get; set; }


    }

}
