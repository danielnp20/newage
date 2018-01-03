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
    /// Models DTO_ocOtrosConceptos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ocOtrosConceptos : DTO_MasterBasic
    {
        #region DTO_ocOtrosConceptos
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ocOtrosConceptos(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.ControlNITInd.Value = Convert.ToBoolean(dr["ControlNITInd"]);
                this.TipoDato.Value = Convert.ToByte(dr["TipoDato"]);        
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ocOtrosConceptos() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ControlNITInd = new UDT_SiNo();
            this.TipoDato = new UDTSQL_tinyint();
        }

        public DTO_ocOtrosConceptos(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ocOtrosConceptos(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_SiNo  ControlNITInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDato { get; set; }

    }

}
