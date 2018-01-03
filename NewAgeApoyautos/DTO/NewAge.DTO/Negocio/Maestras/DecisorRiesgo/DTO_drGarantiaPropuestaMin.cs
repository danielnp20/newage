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
    /// Models DTO_drGarantiaPropuestaMin
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drGarantiaPropuestaMin : DTO_MasterBasic
    {
        #region drGarantiaPropuestaMin
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drGarantiaPropuestaMin(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drGarantiaPropuestaMin()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.Valor = new UDT_Valor();
        }

        public DTO_drGarantiaPropuestaMin(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drGarantiaPropuestaMin(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion
        
        [DataMember]
        public UDT_Valor Valor { get; set; }


    }

}
