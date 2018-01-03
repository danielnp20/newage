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
    /// Models DTO_glCorreosEspeciales
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glCorreosEspeciales : DTO_MasterBasic
    {
        #region DTO_glCorreosEspeciales
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glCorreosEspeciales(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Asunto.Value = dr["Asunto"].ToString();
                this.PlantillaEMail.Value = dr["PlantillaEMail"].ToString();
                this.CuentaOrigen.Value = Convert.ToByte(dr["CuentaOrigen"]);
                this.TipoCorreo.Value = Convert.ToByte(dr["TipoCorreo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glCorreosEspeciales()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Asunto = new UDTSQL_char(50);
            this.PlantillaEMail = new UDT_DescripUnFormat();
            this.CuentaOrigen = new UDTSQL_tinyint();
            this.TipoCorreo = new UDTSQL_tinyint();
        }

        public DTO_glCorreosEspeciales(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glCorreosEspeciales(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }

        #endregion

        [DataMember]
        public UDTSQL_char Asunto { get; set; }

        [DataMember]
        public UDT_DescripUnFormat PlantillaEMail { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuentaOrigen { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoCorreo { get; set; }

    }
}
