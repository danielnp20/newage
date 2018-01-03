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
    /// Models DTO_inEmpaque
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inEmpaque : DTO_MasterBasic
    {
        #region DTO_inEmpaque
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inEmpaque(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.UnidadInvDesc.Value = dr["UnidadInvDesc"].ToString();
                }

                this.EmpaqueTipo.Value = Convert.ToByte(dr["EmpaqueTipo"]);
                this.UnidadInvID.Value = dr["UnidadInvID"].ToString();
                this.Cantidad.Value = Convert.ToDecimal(dr["Cantidad"]);
                this.UnidadVariableInd.Value = Convert.ToBoolean(dr["UnidadVariableInd"]);
                this.Margen.Value = Convert.ToDecimal(dr["Margen"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inEmpaque()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpaqueTipo = new UDTSQL_tinyint();
            this.UnidadInvID = new UDT_BasicID();
            this.UnidadInvDesc = new UDT_Descriptivo();
            this.Cantidad = new UDT_Cantidad();
            this.UnidadVariableInd = new UDT_SiNo();
            this.Margen = new UDT_PorcentajeID();
        }

        public DTO_inEmpaque(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inEmpaque(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint EmpaqueTipo { get; set; }

        [DataMember]
        public UDT_BasicID UnidadInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo UnidadInvDesc { get; set; }

        [DataMember]
        public UDT_Cantidad Cantidad { get; set; }

        [DataMember]
        public UDT_SiNo UnidadVariableInd { get; set; }

        [DataMember]
        public UDT_PorcentajeID Margen { get; set; }

    }

}
