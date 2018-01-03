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
    /// Models DTO_ccVendedorCartera
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccVendedorCartera : DTO_MasterBasic
    {
        #region DTO_ccVendedorCartera
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccVendedorCartera(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                this.TerceroID.Value = dr["TerceroID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_ccVendedorCartera()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TerceroDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();

        }

        public DTO_ccVendedorCartera(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccVendedorCartera(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }
    }

}
