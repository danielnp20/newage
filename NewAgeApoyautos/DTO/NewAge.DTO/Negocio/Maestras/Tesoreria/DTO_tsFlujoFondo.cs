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
    /// Models DTO_tsFlujoFondo
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_tsFlujoFondo : DTO_MasterBasic
    {
        #region DTO_tsFlujoFondo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsFlujoFondo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica) 
            : base(dr,mp)
        {
                InitCols();
                try
                {
                    this.TipoFlujo.Value = Convert.ToByte(dr["TipoFlujo"]);
                }
                catch (Exception e)
                {                    
                    throw e;
                }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsFlujoFondo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoFlujo = new UDTSQL_tinyint();
        }

        public DTO_tsFlujoFondo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsFlujoFondo(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint TipoFlujo {get;set;}
        
    }
}
