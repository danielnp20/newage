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
    /// Models DTO_ccCompradorGrupo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccCompradorGrupo : DTO_MasterBasic
    {
        #region DTO_ccCompradorGrupo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccCompradorGrupo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!string.IsNullOrEmpty(dr["TasaCesion"].ToString()))
                    this.TasaCesion.Value = Convert.ToDecimal(dr["TasaCesion"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccCompradorGrupo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TasaCesion = new UDT_TasaID();
        }

        public DTO_ccCompradorGrupo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccCompradorGrupo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_TasaID TasaCesion { get; set; }
    }
}
