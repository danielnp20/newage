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
    /// Models DTO_ccEstadoCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccEstadoCartera : DTO_MasterBasic
    {
        #region DTO_ccEstadoCartera
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoCartera(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                this.CambiaCuentaInd.Value = Convert.ToBoolean(dr["CambiaCuentaInd"]);
                this.TipoEstado.Value      = Convert.ToByte(dr["TipoEstado"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccEstadoCartera() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            TipoEstado      = new UDTSQL_tinyint();
            CambiaCuentaInd = new UDT_SiNo();    
        }

        public DTO_ccEstadoCartera(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccEstadoCartera(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }

        [DataMember]
        public UDT_SiNo CambiaCuentaInd { get; set; }
    }
}
