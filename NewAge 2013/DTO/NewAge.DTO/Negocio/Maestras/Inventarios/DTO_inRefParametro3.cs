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
    /// Models DTO_inRefParametro3
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inRefParametro3 : DTO_MasterBasic
    {
        #region DTO_inRefParametro3
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inRefParametro3(IDataReader dr, DTO_aplMaestraPropiedades mp)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Fecha"].ToString()))
                    this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["FechaVto"].ToString()))
                    this.FechaVto.Value = Convert.ToDateTime(dr["FechaVto"]);
            }
            catch (Exception e)
            {
                ;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inRefParametro3() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Fecha = new UDTSQL_smalldatetime();
            this.FechaVto = new UDTSQL_smalldatetime();
        }

        public DTO_inRefParametro3(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inRefParametro3(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaVto { get; set; }

    }

}
