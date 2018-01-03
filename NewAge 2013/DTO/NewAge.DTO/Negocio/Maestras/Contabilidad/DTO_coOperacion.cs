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
    /// Models DTO_coOperacion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coOperacion : DTO_MasterBasic
    {
        #region DTO_coOperacion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coOperacion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    if (!string.IsNullOrEmpty(dr["CtaProvisionCostoEXTRADesc"].ToString()))
                        this.CtaProvisionCostoEXTRADesc.Value = dr["CtaProvisionCostoEXTRADesc"].ToString();
                    if (!string.IsNullOrEmpty(dr["CtaprovisionCostoLOCAL"].ToString()))
                        this.CtaProvisionCostoLOCALDesc.Value = dr["CtaprovisionCostoLOCAL"].ToString();
                }
                this.IvaCostoInd.Value = Convert.ToBoolean(dr["IvaCostoInd"]);
                this.TipoOperacion.Value = Convert.ToByte(dr["TipoOperacion"]);
                if (!string.IsNullOrEmpty(dr["CtaProvisionCostoEXTRA"].ToString()))
                    this.CtaProvisionCostoEXTRA.Value = dr["CtaProvisionCostoEXTRA"].ToString();
                if (!string.IsNullOrEmpty(dr["CtaprovisionCostoLOCAL"].ToString()))
                    this.CtaprovisionCostoLOCAL.Value = dr["CtaprovisionCostoLOCAL"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coOperacion()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.IvaCostoInd = new UDT_SiNo();
            this.TipoOperacion = new UDTSQL_tinyint();
            this.CtaProvisionCostoEXTRA = new UDT_BasicID();
            this.CtaProvisionCostoEXTRADesc = new UDT_Descriptivo();
            this.CtaprovisionCostoLOCAL = new UDT_BasicID();
            this.CtaProvisionCostoLOCALDesc = new UDT_Descriptivo();
        }

        public DTO_coOperacion(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coOperacion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_SiNo IvaCostoInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoOperacion { get; set; }

        [DataMember]
        public UDT_BasicID CtaprovisionCostoLOCAL { get; set; }

        [DataMember]
        public UDT_Descriptivo CtaProvisionCostoLOCALDesc { get; set; }

        [DataMember]
        public UDT_BasicID CtaProvisionCostoEXTRA { get; set; }

        [DataMember]
        public UDT_Descriptivo CtaProvisionCostoEXTRADesc { get; set; }

    }
}
