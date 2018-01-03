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
    /// Models DTO_drMoraUltAno
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drMoraUltAno : DTO_MasterBasic
    {
        #region drMoraUltAno
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drMoraUltAno(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.Mora30.Value = Convert.ToBoolean(dr["Mora30"]);
                this.Mora60.Value = Convert.ToBoolean(dr["Mora60"]);
                this.Mora90.Value = Convert.ToBoolean(dr["Mora90"]);
                this.Mora120.Value = Convert.ToBoolean(dr["Mora120"]);

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
        public DTO_drMoraUltAno()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Mora30 = new UDT_SiNo();
            this.Mora60 = new UDT_SiNo();
            this.Mora90 = new UDT_SiNo();
            this.Mora120 = new UDT_SiNo();
            this.Factor = new UDT_PorcentajeID();
        }

        public DTO_drMoraUltAno(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drMoraUltAno(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion


        [DataMember]
        public UDT_SiNo Mora30 { get; set; }
        [DataMember]
        public UDT_SiNo Mora60 { get; set; }
        [DataMember]
        public UDT_SiNo Mora90 { get; set; }
        [DataMember]
        public UDT_SiNo Mora120 { get; set; }

        [DataMember]
        public UDT_PorcentajeID Factor { get; set; }

    }

}
