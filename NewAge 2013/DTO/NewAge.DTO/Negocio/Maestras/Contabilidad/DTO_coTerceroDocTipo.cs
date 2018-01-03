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
    /// Models DTO_coTerceroDocTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coTerceroDocTipo : DTO_MasterBasic
    {
        #region DTO_coTerceroDocTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coTerceroDocTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                this.PersonaNaturalInd.Value = Convert.ToBoolean(dr["PersonaNaturalInd"]);
                this.TipoDocNomina.Value = Convert.ToByte(dr["TipoDocNomina"]);
            }
            catch (Exception e)
            {               
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coTerceroDocTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PersonaNaturalInd = new UDT_SiNo();
            this.TipoDocNomina = new UDTSQL_tinyint();
        }

        public DTO_coTerceroDocTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coTerceroDocTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_SiNo PersonaNaturalInd { get; set; }
  
        [DataMember]
        public UDTSQL_tinyint TipoDocNomina { get; set; }
    }
}
