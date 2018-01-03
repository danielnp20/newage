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
    /// Models DTO_drRepNegMora
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drRepNegMora : DTO_MasterBasic
    {
        #region drRepNegMora
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drRepNegMora(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Dato.Value = Convert.ToBoolean(dr["Dato"]);
                this.DudRecaudo.Value = Convert.ToBoolean(dr["DudRecaudo"]);
                this.Castigadas.Value = Convert.ToBoolean(dr["Castigadas"]);
                this.CtasCobrador.Value = Convert.ToBoolean(dr["CtasCobrador"]);
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
        public DTO_drRepNegMora()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Dato = new UDT_SiNo();
            this.DudRecaudo = new UDT_SiNo();
            this.Castigadas = new UDT_SiNo();
            this.CtasCobrador = new UDT_SiNo();
            this.Factor = new UDT_PorcentajeID();
        }

        public DTO_drRepNegMora(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drRepNegMora(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion


        [DataMember]
        public UDT_SiNo Dato { get; set; }
        [DataMember]
        public UDT_SiNo DudRecaudo { get; set; }
        [DataMember]
        public UDT_SiNo Castigadas { get; set; }
        [DataMember]
        public UDT_SiNo CtasCobrador { get; set; }

        [DataMember]
        public UDT_PorcentajeID Factor { get; set; }

    }

}
