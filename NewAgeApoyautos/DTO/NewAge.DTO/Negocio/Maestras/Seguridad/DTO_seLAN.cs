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
    /// Models DTO_seLAN
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_seLAN : DTO_MasterBasic
    {
        #region DTO_seLAN
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_seLAN(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                this.CadenaConn.Value = (dr["CadenaConn"]).ToString();
                this.CadenaConnLogger.Value = (dr["CadenaConnLogger"]).ToString();
                this.RemotoInd.Value = Convert.ToBoolean((dr["RemotoInd"]));
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_seLAN()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CadenaConn = new UDTSQL_varchar(250);
            this.CadenaConnLogger = new UDTSQL_varchar(250);
            this.RemotoInd = new UDT_SiNo();
        }

        public DTO_seLAN(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_seLAN(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_varchar CadenaConn { get; set; }

        [DataMember]
        public UDTSQL_varchar CadenaConnLogger { get; set; }

        [DataMember]
        public UDT_SiNo RemotoInd { get; set; }


        public override string ToString()
        {
            return this.ID.ToString();
        }

    }

}