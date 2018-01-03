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
    /// Models DTO_coImpDeclaracionCalendario
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpDeclaracionCalendario : DTO_MasterComplex
    {
        #region DTO_coImpDeclaracionCalendario
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpDeclaracionCalendario(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ImpuestoDeclDesc.Value = dr["ImpuestoDeclDesc"].ToString();
                }

                this.ImpuestoDeclID.Value = dr["ImpuestoDeclID"].ToString();
                this.AñoFiscal.Value = Convert.ToInt32(dr["AñoFiscal"]);
                this.Periodo.Value = Convert.ToByte(dr["Periodo"]);
                this.Fecha.Value = Convert.ToDateTime(dr["Fecha"]);
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"]);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpDeclaracionCalendario()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ImpuestoDeclID = new UDT_BasicID();
            this.ImpuestoDeclDesc = new UDT_Descriptivo();
            this.AñoFiscal = new UDTSQL_int();
            this.Periodo = new UDTSQL_tinyint();
            this.Fecha = new UDTSQL_smalldatetime();
            this.NumeroDoc = new UDT_Consecutivo();
        }

        public DTO_coImpDeclaracionCalendario(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_coImpDeclaracionCalendario(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ImpuestoDeclID { get; set; }

        [DataMember]
        public UDT_Descriptivo ImpuestoDeclDesc { get; set; }

        [DataMember]
        public UDTSQL_int AñoFiscal { get; set; }

        [DataMember]
        public UDTSQL_tinyint Periodo { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Fecha { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }
    }
}
