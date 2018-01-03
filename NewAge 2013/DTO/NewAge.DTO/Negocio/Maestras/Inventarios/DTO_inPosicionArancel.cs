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
    /// Models DTO_inPosicionArancel
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inPosicionArancel : DTO_MasterBasic
    {
        #region DTO_inPosicionArancel
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inPosicionArancel(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Porcentaje.Value = Convert.ToDecimal(dr["Porcentaje"]);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inPosicionArancel() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.Porcentaje = new UDTSQL_numeric();             
        }

        public DTO_inPosicionArancel(DTO_MasterBasic comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_inPosicionArancel(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_numeric Porcentaje { get; set; }

    }
}
