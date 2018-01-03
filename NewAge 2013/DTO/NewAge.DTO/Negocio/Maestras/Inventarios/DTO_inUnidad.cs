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
    /// Models DTO_inUnidad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inUnidad : DTO_MasterBasic
    {
        #region DTO_inUnidad
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inUnidad(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.TipoMedida.Value = Convert.ToByte(dr["TipoMedida"]);
                this.BienesInd.Value = Convert.ToBoolean(dr["BienesInd"]);
                this.ServicioInd.Value = Convert.ToBoolean(dr["ServicioInd"]);
                this.AproximaEnteroInd.Value = Convert.ToBoolean(dr["AproximaEnteroInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inUnidad() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoMedida = new UDTSQL_tinyint();
            this.BienesInd = new UDT_SiNo();
            this.ServicioInd = new UDT_SiNo();
            this.AproximaEnteroInd = new UDT_SiNo();
        }

        public DTO_inUnidad(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inUnidad(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint TipoMedida { get; set; }

        [DataMember]
        public UDT_SiNo BienesInd { get; set; }

        [DataMember]
        public UDT_SiNo ServicioInd { get; set; }

        [DataMember]
        public UDT_SiNo AproximaEnteroInd { get; set; }

    }

}
