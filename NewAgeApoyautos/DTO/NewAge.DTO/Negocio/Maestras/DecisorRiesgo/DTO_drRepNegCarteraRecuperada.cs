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
    /// Models DTO_drRepNegCarteraRecuperada
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drRepNegCarteraRecuperada : DTO_MasterBasic
    {
        #region drRepNegCarteraRecuperada
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drRepNegCarteraRecuperada(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Factor.Value = Convert.ToDecimal(dr["Factor"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drRepNegCarteraRecuperada()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.Factor = new UDT_PorcentajeID();

        }

        public DTO_drRepNegCarteraRecuperada(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drRepNegCarteraRecuperada(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion
        
        [DataMember]
        public UDT_PorcentajeID Factor { get; set; }

    }

}
