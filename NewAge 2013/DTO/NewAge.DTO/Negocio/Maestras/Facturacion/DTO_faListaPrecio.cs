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
    /// Models DTO_faAsesor
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faListaPrecio : DTO_MasterBasic
    {
        #region DTO_faListaPrecio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faListaPrecio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!string.IsNullOrEmpty(dr["FactorID"].ToString()))
                 this.FactorID.Value = Convert.ToDecimal(dr["FactorID"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faListaPrecio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.FactorID = new UDT_FactorID();
        }

        public DTO_faListaPrecio(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faListaPrecio(DTO_aplMaestraPropiedades masterProperties) : base(masterProperties)
        {
        }  

        #endregion 

        [DataMember]
        public UDT_FactorID FactorID { get; set; }

    }
}


