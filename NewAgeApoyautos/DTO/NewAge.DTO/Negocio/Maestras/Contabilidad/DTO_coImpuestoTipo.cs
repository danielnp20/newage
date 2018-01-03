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
    /// Models DTO_coImpuestoTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpuestoTipo : DTO_MasterBasic
    {
        #region DTO_coImpuestoTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpuestoTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ConceptoCxPDesc.Value = dr["ConceptoCxPDesc"].ToString();
                }
                this.CausacionPago.Value = Convert.ToByte(dr["CausacionPago"]);
                this.ImpuestoAlcance.Value = Convert.ToByte(dr["ImpuestoAlcance"]);
                this.AcumuladoInd.Value = Convert.ToBoolean(dr["AcumuladoInd"]);
                this.ImpuestoAplicacion.Value = Convert.ToByte(dr["ImpuestoAplicacion"]);
                this.PeriodoPago.Value = Convert.ToByte(dr["PeriodoPago"]);
                this.ConceptoCxPID.Value = Convert.ToString(dr["ConceptoCxPID"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpuestoTipo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CausacionPago = new UDTSQL_tinyint();
            this.ImpuestoAlcance = new UDTSQL_tinyint();
            this.AcumuladoInd = new UDT_SiNo();
            this.ImpuestoAplicacion = new UDTSQL_tinyint();
            this.PeriodoPago = new UDTSQL_tinyint();
            this.ConceptoCxPID = new UDT_BasicID();
            this.ConceptoCxPDesc = new UDT_Descriptivo();
        }

        public DTO_coImpuestoTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coImpuestoTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_tinyint CausacionPago { get; set; }

        [DataMember]
        public UDTSQL_tinyint ImpuestoAlcance { get; set; }

        [DataMember]
        public UDT_SiNo AcumuladoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint ImpuestoAplicacion{ get; set; }

        [DataMember]
        public UDTSQL_tinyint PeriodoPago { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCxPID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCxPDesc { get; set; }
  
    }
}
