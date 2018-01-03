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
    /// Models DTO_coRegimenFiscal
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coRegimenFiscal : DTO_MasterBasic 
    {
        #region DTO_coRegimenFiscal
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coRegimenFiscal(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.TipoTercero.Value = Convert.ToByte(dr["TipoTercero"]);
                this.FactEquivalenteInd.Value = Convert.ToBoolean(dr["FactEquivalenteInd"]);
                this.ValorTope.Value = Convert.ToDecimal(dr["ValorTope"]);
                this.ExcluyeCREEInd.Value = Convert.ToBoolean(dr["ExcluyeCREEInd"]);
                if(!string.IsNullOrEmpty(dr["PersonaJuridica"].ToString()))
                    this.PersonaJuridica.Value = Convert.ToBoolean(dr["PersonaJuridica"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coRegimenFiscal() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoTercero = new UDTSQL_tinyint();
            this.FactEquivalenteInd = new UDT_SiNo();
            this.ValorTope = new UDT_Valor();
            this.ExcluyeCREEInd = new UDT_SiNo();
            this.PersonaJuridica = new UDT_SiNo();
        }

        public DTO_coRegimenFiscal(DTO_MasterBasic basic)  : base(basic)
        {
            InitCols();
        }

        public DTO_coRegimenFiscal(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint TipoTercero { get; set; }

        [DataMember]
        public UDT_SiNo FactEquivalenteInd { get; set; }

        [DataMember]
        public UDT_Valor ValorTope { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeCREEInd { get; set; }

        [DataMember]
        public UDT_SiNo PersonaJuridica { get; set; }
    }
}
