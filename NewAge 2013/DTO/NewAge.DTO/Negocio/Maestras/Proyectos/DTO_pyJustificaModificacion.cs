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
    /// Models DTO_pyJustificaModificacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyJustificaModificacion : DTO_MasterBasic
    {
        #region DTO_pyJustificaModificacion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyJustificaModificacion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrEmpty(dr["TipoModificacion"].ToString()))
                    this.TipoModificacion.Value = Convert.ToByte(dr["TipoModificacion"]);
                if (!string.IsNullOrEmpty(dr["Responsable"].ToString()))
                    this.Responsable.Value = Convert.ToByte(dr["Responsable"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_pyJustificaModificacion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoModificacion = new UDTSQL_tinyint();
            this.Responsable = new UDTSQL_tinyint();
        }

        public DTO_pyJustificaModificacion(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyJustificaModificacion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion


        [DataMember]
        public UDTSQL_tinyint TipoModificacion { get; set; }

        [DataMember]
        public UDTSQL_tinyint Responsable { get; set; }



    }

}
