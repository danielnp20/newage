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
    /// Models DTO_plTasasPresupuesto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_plTasasPresupuesto : DTO_MasterComplex
    {
        #region DTO_plTasasPresupuesto
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_plTasasPresupuesto(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ContratoDesc.Value = dr["ContratoDesc"].ToString();
                    this.CampoDesc.Value = dr["CampoDesc"].ToString();
                }

                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.ContratoID.Value = dr["ContratoID"].ToString();
                this.Campo.Value = dr["Campo"].ToString();
                this.TRMCapex.Value = Convert.ToDecimal(dr["TRMCapex"]);
                this.TRMOpex.Value = Convert.ToDecimal(dr["TRMOpex"]);
                this.TRMOtros.Value = Convert.ToDecimal(dr["TRMOtros"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_plTasasPresupuesto() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoID = new UDT_PeriodoID();
            this.ContratoID = new UDT_BasicID();
            this.ContratoDesc = new UDT_Descriptivo();
            this.Campo = new UDT_BasicID();
            this.CampoDesc = new UDT_Descriptivo();
            this.TRMCapex = new UDT_TasaID();
            this.TRMOpex = new UDT_TasaID();
            this.TRMOtros = new UDT_TasaID();
        }

        public DTO_plTasasPresupuesto(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_plTasasPresupuesto(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_BasicID ContratoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ContratoDesc { get; set; }

        [DataMember]
        public UDT_BasicID Campo { get; set; }

        [DataMember]
        public UDT_Descriptivo CampoDesc { get; set; }

        [DataMember]
        public UDT_TasaID TRMCapex { get; set; }

        [DataMember]
        public UDT_TasaID TRMOpex { get; set; }

        [DataMember]
        public UDT_TasaID TRMOtros { get; set; }
    }

}
