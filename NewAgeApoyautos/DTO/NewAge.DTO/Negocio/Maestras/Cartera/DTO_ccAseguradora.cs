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
    /// Models DTO_caAseguradora
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccAseguradora : DTO_MasterBasic
    {
        #region ccAseguradora
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccAseguradora(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();

                this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["DiasPago"].ToString()))
                    this.DiasPago.Value = Convert.ToInt32(dr["DiasPago"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_ccAseguradora()
            : base()
        {
            InitCols();
        }

        public DTO_ccAseguradora(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccAseguradora(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.DiasPago = new UDTSQL_int();
            this.TerceroDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
        }

        #endregion

        [DataMember]
        public UDTSQL_int DiasPago { get; set; }

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

    }

}
