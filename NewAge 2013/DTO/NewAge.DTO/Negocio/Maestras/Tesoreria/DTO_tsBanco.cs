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
    /// Models DTO_tsBanco
    /// </summary>  
    [DataContract]
    [Serializable]
    public class DTO_tsBanco : DTO_MasterBasic
    {
        #region DTO_tsBanco
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_tsBanco(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Procedimiento.Value = dr["Procedimiento"].ToString();
                this.CodigoAlt1.Value = dr["CodigoAlt1"].ToString();
                this.CodigoAlt2.Value = dr["CodigoAlt2"].ToString();
                this.CodigoAlt3.Value = dr["CodigoAlt3"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_tsBanco() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Procedimiento = new UDTSQL_char(20);
            this.CodigoAlt1 = new UDTSQL_char(20);
            this.CodigoAlt2 = new UDTSQL_char(20);
            this.CodigoAlt3 = new UDTSQL_char(20);
        }

        public DTO_tsBanco(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_tsBanco(DTO_aplMaestraPropiedades masterProperties) 
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_char Procedimiento {get;set;}

        [DataMember]
        public UDTSQL_char CodigoAlt1 { get; set; }

        [DataMember]
        public UDTSQL_char CodigoAlt2 { get; set; }

        [DataMember]
        public UDTSQL_char CodigoAlt3 { get; set; }

    }
}
