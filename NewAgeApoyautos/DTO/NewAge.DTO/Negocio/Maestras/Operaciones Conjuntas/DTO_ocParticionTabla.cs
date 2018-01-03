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
    /// Models DTO_ocParticionTabla
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ocParticionTabla : DTO_MasterComplex
    {
        #region DTO_ocParticionTabla
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ocParticionTabla(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ContratoDesc.Value = dr["ContratoDesc"].ToString();
                    this.CampoDesc.Value = dr["CampoDesc"].ToString();
                    this.SocioDesc.Value = dr["SocioDesc"].ToString();
                }
                this.ContratoID.Value = dr["ContratoID"].ToString();
                this.Campo.Value = dr["Campo"].ToString();
                this.SocioID.Value = dr["SocioID"].ToString();
                this.PeriodoID.Value = Convert.ToDateTime(dr["PeriodoID"]);
                this.Valor.Value = Convert.ToDecimal(dr["Valor"]);

                if (!string.IsNullOrWhiteSpace(dr["PorCapex"].ToString()))
                    this.PorCapex.Value = Convert.ToDecimal(dr["PorCapex"]);
                if (!string.IsNullOrWhiteSpace(dr["PorOpex"].ToString()))
                    this.PorOpex.Value = Convert.ToDecimal(dr["PorOpex"]);
                if (!string.IsNullOrWhiteSpace(dr["PorInversion"].ToString()))
                    this.PorInversion.Value = Convert.ToDecimal(dr["PorInversion"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ocParticionTabla()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ContratoID = new UDT_BasicID();
            this.ContratoDesc = new UDT_Descriptivo();
            this.Campo = new UDT_BasicID();
            this.CampoDesc = new UDT_Descriptivo();
            this.SocioID = new UDT_BasicID();
            this.SocioDesc = new UDT_Descriptivo();
            this.PeriodoID = new UDT_PeriodoID();
            this.Valor = new UDT_Valor();
            this.PorCapex = new UDT_PorcentajeID();
            this.PorOpex = new UDT_PorcentajeID();
            this.PorInversion = new UDT_PorcentajeID();
        }

        public DTO_ocParticionTabla(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ocParticionTabla(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ContratoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ContratoDesc { get; set; }

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
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorCapex{ get; set; }

        [DataMember]
        public UDT_PorcentajeID PorOpex { get; set; }

        [DataMember]
        public UDT_PorcentajeID PorInversion { get; set; }
    }
}
