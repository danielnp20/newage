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
    /// Models DTO_glDocumentoTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDocumentoTipo : DTO_MasterBasic
    {
        #region DTO_glDocumentoTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocumentoTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                }

                this.DocumentoID.Value = dr["DocumentoID"].ToString();
                this.RelacionDoc.Value = Convert.ToByte(dr["RelacionDoc"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDocumentoTipo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
            this.RelacionDoc = new UDTSQL_tinyint();
        }

        public DTO_glDocumentoTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glDocumentoTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        #endregion

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint RelacionDoc { get; set; }
    }
}
