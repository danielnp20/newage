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
    /// Models DTO_acClase
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acClase : DTO_MasterBasic
    {
        #region DTO_acClase
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acClase(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.VidaUtilLOC.Value = Convert.ToInt16(dr["VidaUtilLOC"]);
                this.VidaUtilIFRS.Value = Convert.ToInt16(dr["VidaUtilIFRS"]);
                this.VidaUtilUSG.Value = Convert.ToInt16(dr["VidaUtilUSG"]);
                this.TipoDepreLOC.Value = Convert.ToByte(dr["TipoDepreLOC"]);
                this.TipoDepreIFRS.Value = Convert.ToByte(dr["TipoDepreIFRS"]);
                this.TipoDepreUSG.Value = Convert.ToByte(dr["TipoDepreUSG"]);
                this.PorcSalvamentoLOC.Value = Convert.ToDecimal(dr["TipoDepreLOC"]);
                this.PorcSalvamentoIFRS.Value = Convert.ToDecimal(dr["PorcSalvamentoIFRS"]);
                this.PorcSalvamentoUSG.Value = Convert.ToDecimal(dr["PorcSalvamentoUSG"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acClase()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.VidaUtilLOC = new UDTSQL_int();
            this.VidaUtilIFRS = new UDTSQL_int();
            this.VidaUtilUSG = new UDTSQL_int();
            this.TipoDepreLOC = new UDTSQL_tinyint();
            this.TipoDepreIFRS = new UDTSQL_tinyint();
            this.TipoDepreUSG = new UDTSQL_tinyint();
            this.PorcSalvamentoLOC = new UDT_PorcentajeID();
            this.PorcSalvamentoIFRS = new UDT_PorcentajeID();
            this.PorcSalvamentoUSG = new UDT_PorcentajeID();
        }

        public DTO_acClase(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acClase(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_int VidaUtilLOC { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilIFRS { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilUSG { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreLOC { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreIFRS { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreUSG { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcSalvamentoLOC { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcSalvamentoIFRS { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorcSalvamentoUSG { get; set; }
    }
}
