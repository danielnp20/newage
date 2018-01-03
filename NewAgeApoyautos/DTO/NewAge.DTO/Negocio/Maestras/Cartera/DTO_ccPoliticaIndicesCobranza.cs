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
    /// Models DTO_ccGestionCobranza
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccPoliticaIndicesCobranza : DTO_MasterComplex
    {
        #region DTO_ccPoliticaIndicesCobranza
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccPoliticaIndicesCobranza(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.TipoIndice.Value = Convert.ToByte(dr["TipoIndice"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
                this.Puntos.Value = Convert.ToDecimal(dr["Puntos"]);
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccPoliticaIndicesCobranza()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoIndice = new UDTSQL_tinyint();
            this.Valor = new UDT_Valor();
            this.Puntos = new UDT_Valor();
        }

        public DTO_ccPoliticaIndicesCobranza(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccPoliticaIndicesCobranza(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoIndice { get; set; }
        
        [DataMember]
        public UDT_Valor Valor { get; set; }
        
        [DataMember]
        public UDT_Valor Puntos { get; set; }
        
    }
}

