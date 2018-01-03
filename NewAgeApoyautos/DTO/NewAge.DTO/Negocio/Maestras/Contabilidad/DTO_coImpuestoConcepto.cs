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
    /// Models DTO_coImpuestoConcepto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpuestoConcepto : DTO_MasterBasic
    {
        #region DTO_coImpuestoConcepto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpuestoConcepto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.GrupoFiscalDesc.Value = dr["GrupoFiscalDesc"].ToString();

                this.GrupoFiscalID.Value = dr["GrupoFiscalID"].ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpuestoConcepto()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.GrupoFiscalID = new UDT_BasicID();
            this.GrupoFiscalDesc = new UDT_Descriptivo();
        }

        public DTO_coImpuestoConcepto(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coImpuestoConcepto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID GrupoFiscalID { get; set; }

        [DataMember]
        public UDT_Descriptivo GrupoFiscalDesc { get; set; }
    }

}
