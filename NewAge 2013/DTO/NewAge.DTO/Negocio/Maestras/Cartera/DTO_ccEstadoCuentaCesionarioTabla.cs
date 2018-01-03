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
    /// Models DTO_ccEstadoCuentaCesionarioTabla
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccEstadoCuentaCesionarioTabla : DTO_MasterComplex
    {
        #region DTO_ccEstadoCuentaCesionarioTabla
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccEstadoCuentaCesionarioTabla(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ConceptoECCesionarioDesc.Value = dr["ConceptoECCesionarioDesc"].ToString();
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                }

                this.ConceptoECCesionario.Value = dr["ConceptoECCesionario"].ToString();
                this.DocumentoID.Value = dr["DocumentoID"].ToString();
                
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccEstadoCuentaCesionarioTabla() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConceptoECCesionario = new UDT_BasicID();
            this.ConceptoECCesionarioDesc = new UDT_Descriptivo();
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
        }

        public DTO_ccEstadoCuentaCesionarioTabla(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccEstadoCuentaCesionarioTabla(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ConceptoECCesionario { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoECCesionarioDesc { get; set; }

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

    }

}
