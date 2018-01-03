using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_glConsultaFiltro
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glConsultaSeleccion
    {
        #region Contructora

        /// <summary>
        /// Constructor
        /// </summary>
        public DTO_glConsultaSeleccion(IDataReader dr)
        {
            if (dr["glConsultaSeleccionID"] != DBNull.Value)
                this.glConsultaSeleccionID = Convert.ToInt32(dr["glConsultaSeleccionID"]);

            if (dr["glConsultaID"] != DBNull.Value)
                this.glConsultaID = Convert.ToInt32(dr["glConsultaID"]);

            if (dr["CampoFisico"] != DBNull.Value)
                this.CampoFisico = dr["CampoFisico"].ToString();

            if (dr["CampoDesc"] != DBNull.Value)
                this.CampoDesc = dr["CampoDesc"].ToString();

            if (dr["Idx"] != DBNull.Value)
                this.Idx = Convert.ToInt32(dr["Idx"]);

            if (dr["Tipo"] != DBNull.Value)
                this.Tipo = dr["Tipo"].ToString();

            if (dr["OrdenIdx"] != DBNull.Value)
                this.OrdenIdx = Convert.ToInt32(dr["OrdenIdx"]);

            if (dr["OrdenTipo"] != DBNull.Value)
                this.OrdenTipo = dr["OrdenTipo"].ToString();

            if (dr["GroupBy"] != DBNull.Value)
                this.GroupBy = Convert.ToBoolean(dr["GroupBy"]);
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glConsultaSeleccion()
        {
            this.glConsultaID = null;
            this.glConsultaSeleccionID = null;
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Gets or sets the glConsultaID
        /// </summary>
        [DataMember]
        public int? glConsultaSeleccionID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int? glConsultaID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string CampoFisico
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string CampoDesc
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int Idx
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string Tipo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public int OrdenIdx
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Nombre
        /// </summary>
        [DataMember]
        public string OrdenTipo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the GroupBy
        /// </summary>
        [DataMember]
        public bool? GroupBy
        {
            get;
            set;
        }
        #endregion
    }
}
