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
    /// Models DTO_drPorcentajeFinMax
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drPorcentajeFinMax : DTO_MasterBasic
    {
        #region drPorcentajeFinMax
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drPorcentajeFinMax(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                //this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drPorcentajeFinMax()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            //this.Valor = new UDT_Valor();
        }

        public DTO_drPorcentajeFinMax(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drPorcentajeFinMax(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion
        
        //[DataMember]
        //public UDT_Valor Valor { get; set; }


    }

}
