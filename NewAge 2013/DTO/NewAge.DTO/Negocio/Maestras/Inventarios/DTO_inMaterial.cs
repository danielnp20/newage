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
    /// Models DTO_inMaterial
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inMaterial : DTO_MasterBasic
    {
        #region DTO_inMaterial
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inMaterial(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Margen"].ToString()))
                    this.Margen.Value = Convert.ToDecimal(dr["Margen"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inMaterial()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Margen = new UDT_Cantidad();
        }

        public DTO_inMaterial(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inMaterial(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_Cantidad Margen { get; set; }

    }

}
