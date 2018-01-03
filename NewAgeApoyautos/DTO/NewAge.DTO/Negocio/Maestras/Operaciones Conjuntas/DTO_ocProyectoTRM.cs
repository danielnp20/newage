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
    /// Models DTO_ocProyectoTRM
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ocProyectoTRM : DTO_MasterComplex
    {
        #region DTO_ocProyectoTRM
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ocProyectoTRM(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.CampoDesc.Value = dr["CampoDesc"].ToString();
                    this.SocioDesc.Value = dr["SocioDesc"].ToString();
                }
                this.Campo.Value = dr["Campo"].ToString();
                this.SocioID.Value = dr["SocioID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.TRMCapex.Value = Convert.ToDecimal(dr["TRMCapex"]);
                this.TRMOpex.Value = Convert.ToDecimal(dr["TRMOpex"]);
                this.TRMInversion.Value = Convert.ToDecimal(dr["TRMInversion"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ocProyectoTRM()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Campo = new UDT_BasicID();
            this.CampoDesc = new UDT_Descriptivo();
            this.SocioID = new UDT_BasicID();
            this.SocioDesc = new UDT_Descriptivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.TRMCapex = new UDT_PorcentajeID();
            this.TRMOpex = new UDT_PorcentajeID();
            this.TRMInversion = new UDT_PorcentajeID();
        }

        public DTO_ocProyectoTRM(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ocProyectoTRM(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID Campo { get; set; }

        [DataMember]
        public UDT_Descriptivo CampoDesc { get; set; }

        [DataMember]
        public UDT_BasicID SocioID { get; set; }

        [DataMember]
        public UDT_Descriptivo SocioDesc { get; set; }

        [DataMember]
        public UDT_PeriodoID PeriodoID { get; set; }
        
        [DataMember]
        public UDT_PorcentajeID TRMCapex { get; set; }

        [DataMember]
        public UDT_PorcentajeID TRMOpex { get; set; }

        [DataMember]
        public UDT_PorcentajeID TRMInversion { get; set; }

    }
}
