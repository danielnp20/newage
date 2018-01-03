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
    /// Models DTO_glProcedimiento
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glProcedimiento : DTO_MasterBasic
    {
        #region DTO_glProcedimiento
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glProcedimiento(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                    this.ModuloDesc.Value = dr["ModuloDesc"].ToString();

                this.ModuloID.Value = dr["ModuloID"].ToString();
                this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glProcedimiento()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ModuloID = new UDT_BasicID();
            this.ModuloDesc = new UDT_Descriptivo();
            this.Tipo = new UDTSQL_tinyint();
        }

        public DTO_glProcedimiento(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glProcedimiento(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ModuloID { get; set; }

        [DataMember]
        public UDT_Descriptivo ModuloDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }
    }
}
