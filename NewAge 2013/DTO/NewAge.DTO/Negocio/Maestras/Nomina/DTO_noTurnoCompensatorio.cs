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
    /// Models DTO_noTurnoCompensatorio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noTurnoCompensatorio : DTO_MasterBasic
    {
        #region DTO_noTurnoCompensatorio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noTurnoCompensatorio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.HorarioDesc.Value = dr["HorarioDesc"].ToString();
                }
                
                this.FactorDT.Value = Convert.ToDecimal(dr["FactorDT"]);
                if(!string.IsNullOrEmpty(dr["FactorFT"].ToString()))
                    this.FactorFT.Value = Convert.ToDecimal(dr["FactorFT"]);
                this.HorarioID.Value = dr["HorarioID"].ToString();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noTurnoCompensatorio() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.FactorDT = new UDTSQL_numeric();
            this.FactorFT = new UDTSQL_numeric();
            this.HorarioID = new UDT_BasicID();
            this.HorarioDesc = new UDT_Descriptivo();
        }

        public DTO_noTurnoCompensatorio(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_noTurnoCompensatorio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDTSQL_numeric FactorDT { get; set; }

        [DataMember]
        public UDTSQL_numeric FactorFT { get; set; }

        [DataMember]
        public UDT_BasicID HorarioID { get; set; }

        [DataMember]
        public UDT_Descriptivo HorarioDesc { get; set; }

    }
}



