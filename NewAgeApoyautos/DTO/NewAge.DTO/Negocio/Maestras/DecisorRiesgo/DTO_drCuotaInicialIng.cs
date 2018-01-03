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
    /// Models DTO_drCuotaInicialIng
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_drCuotaInicialIng : DTO_MasterBasic
    {
        #region drCuotaInicialIng
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_drCuotaInicialIng(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.PorcBase.Value = Convert.ToDecimal(dr["PorcBase"]);
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
        public DTO_drCuotaInicialIng()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.PorcBase = new UDT_PorcentajeID();
            this.Factor = new UDT_PorcentajeID();

        }

        public DTO_drCuotaInicialIng(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_drCuotaInicialIng(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_PorcentajeID PorcBase { get; set; }

        [DataMember]
        public UDT_PorcentajeID Factor { get; set; }


    }

}
