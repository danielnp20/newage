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
    /// Models DTO_plRecurso
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plRecurso : DTO_MasterBasic
    {
        #region DTO_plRecurso
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plRecurso(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if(!isReplica)
                    this.RecursoGrupoDesc.Value = Convert.ToString(dr["RecursoGrupoDesc"]);
                this.CAPEXInd.Value = Convert.ToBoolean(dr["CAPEXInd"]);
                this.RecursoGrupoID.Value = Convert.ToString(dr["RecursoGrupoID"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plRecurso() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CAPEXInd = new UDT_SiNo();
            this.RecursoGrupoID = new UDT_BasicID();
            this.RecursoGrupoDesc = new UDT_Descriptivo();
        }

        public DTO_plRecurso(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_plRecurso(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_SiNo CAPEXInd { get; set; }

        [DataMember]
        public UDT_BasicID RecursoGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo RecursoGrupoDesc { get; set; }

    }

}
