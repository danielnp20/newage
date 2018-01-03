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
    /// Models DTO_coLineaNegocio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glTasaCambio : DTO_MasterComplex
    {
        #region DTO_glTasaCambio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glTasaCambio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.MonedaDesc.Value = dr["MonedaDesc"].ToString();
                }

                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                this.TasaCambio.Value = Convert.ToDecimal(dr["TasaCambio"]);
            }
            catch (Exception e)
            {
                throw e;
            }         
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glTasaCambio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            MonedaID = new UDT_BasicID();

            MonedaDesc = new UDT_Descriptivo();

            Fecha = new UDTSQL_smalldatetime();

            TasaCambio = new UDT_Valor();
        }

        public DTO_glTasaCambio(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glTasaCambio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID MonedaID { get; set; }

        [DataMember]
        public UDT_Descriptivo MonedaDesc { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Valor TasaCambio { get; set; } 

    }

}
