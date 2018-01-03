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
    /// Models DTO_drProbabilidadResultado
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drProbabilidadResultado : DTO_MasterBasic
    {
        #region drProbabilidadResultado
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drProbabilidadResultado(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Acierta.Value          = Convert.ToBoolean(dr["Acierta"]);
                this.Quanto.Value           = Convert.ToBoolean(dr["Quanto"]);
                this.Endeudamiento.Value = Convert.ToBoolean(dr["Endeudamiento"]);
                this.MorasVIG.Value         = Convert.ToBoolean(dr["MorasVIG"]);
                this.ReporteNEG.Value       = Convert.ToBoolean(dr["ReporteNEG"]);
                this.ModeloReciente.Value   = Convert.ToBoolean(dr["ModeloReciente"]);
                this.FactorPROB.Value = Convert.ToDecimal(dr["FactorPROB"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_drProbabilidadResultado()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.Acierta = new UDT_SiNo();
            this.Quanto = new UDT_SiNo();
            this.Endeudamiento = new UDT_SiNo();
            this.MorasVIG = new UDT_SiNo();
            this.ReporteNEG = new UDT_SiNo();
            this.ModeloReciente = new UDT_SiNo();
            this.FactorPROB = new UDT_PorcentajeID();            
        }

        public DTO_drProbabilidadResultado(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drProbabilidadResultado(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_SiNo Acierta { get; set; }
        [DataMember]
        public UDT_SiNo Quanto { get; set; }
        [DataMember]
        public UDT_SiNo Endeudamiento { get; set; }
        [DataMember]
        public UDT_SiNo MorasVIG { get; set; }
        [DataMember]
        public UDT_SiNo ModeloReciente { get; set; }
        [DataMember]
        public UDT_SiNo ReporteNEG { get; set; }
        [DataMember]
        public UDT_PorcentajeID FactorPROB { get; set; }
    }

}
