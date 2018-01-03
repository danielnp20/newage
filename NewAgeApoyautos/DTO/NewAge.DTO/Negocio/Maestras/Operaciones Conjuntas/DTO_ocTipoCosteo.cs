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
    /// Models DTO_ocTipoCosteo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ocTipoCosto : DTO_MasterBasic
    {
        #region ocTipoCosteo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ocTipoCosto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.SocioDesc.Value = dr["SocioDesc"].ToString();
                }
                this.SocioID.Value = dr["SocioID"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ocTipoCosto()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.SocioID = new UDT_BasicID();
            this.SocioDesc = new UDT_Descriptivo();
        }

        public DTO_ocTipoCosto(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ocTipoCosto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID SocioID { get; set; }

        [DataMember]
        public UDT_Descriptivo SocioDesc { get; set; }

    }
}
