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
    /// Models DTO_coTasaCierre
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coTasaCierre : DTO_MasterComplex
    {
        #region DTO_coTasaCierre
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coTasaCierre(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.MonedaDesc.Value = dr["MonedaDesc"].ToString();
                }

                this.MonedaID.Value = dr["MonedaID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.TasaCambio.Value = Convert.ToDecimal(dr["TasaCambio"]);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coTasaCierre() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.MonedaID = new UDT_BasicID();
            this.MonedaDesc = new UDT_Descriptivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.TasaCambio = new UDT_TasaID();
        }

        public DTO_coTasaCierre(DTO_MasterComplex comp) : base(comp)
        {
            InitCols();
        }

        public DTO_coTasaCierre(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID MonedaID { get; set; }

        [DataMember]
        public UDT_Descriptivo MonedaDesc { get; set; }
        
        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }

        [DataMember]
        public UDT_TasaID TasaCambio { get; set; }

    }
}
