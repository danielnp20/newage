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
    /// Models DTO_glDiasFestivos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDiasFestivos : DTO_MasterComplex
    {
        #region DTO_glDiasFestivos
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDiasFestivos(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.DiasFestivoID.Value = Convert.ToDateTime(dr["DiasFestivoID"]);
                this.Fecha.Value = Convert.ToDateTime((dr["Fecha"]));
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDiasFestivos() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DiasFestivoID = new UDTSQL_smalldatetime();
            this.Fecha = new UDTSQL_smalldatetime();
        }

        public DTO_glDiasFestivos(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glDiasFestivos(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_smalldatetime DiasFestivoID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha{ get; set; }

    }
}
