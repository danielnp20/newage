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
    /// Models DTO_ccIncorporacionNovedad
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccIncorporacionNovedad : DTO_MasterBasic
    {
        #region DTO_ccIncorporacionNovedad
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccIncorporacionNovedad(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    //
                }

                if (!string.IsNullOrEmpty(dr["TipoNovedad"].ToString()))
                    this.TipoNovedad.Value = Convert.ToByte(dr["TipoNovedad"]); 
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccIncorporacionNovedad()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoNovedad = new UDTSQL_tinyint();
        }

        public DTO_ccIncorporacionNovedad(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccIncorporacionNovedad(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoNovedad { get; set; }
    }

}
