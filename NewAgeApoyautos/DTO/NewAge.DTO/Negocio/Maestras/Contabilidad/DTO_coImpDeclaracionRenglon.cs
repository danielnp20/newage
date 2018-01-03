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
    /// Models DTO_coImpDeclaracionRenglon
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpDeclaracionRenglon : DTO_MasterComplex
    {
        #region DTO_coImpDeclaracionRenglon
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpDeclaracionRenglon(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
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
                this.Renglon.Value = dr["Renglon"].ToString();
                this.Descriptivo.Value = dr["Descriptivo"].ToString();
                this.SignoSuma.Value = Convert.ToByte(dr["SignoSuma"]);
                this.Tarifa.Value = Convert.ToDecimal(dr["Tarifa"]); 
                this.CostosInd.Value = Convert.ToBoolean(dr["CostosInd"]);
                if (!string.IsNullOrWhiteSpace(dr["BienServicio"].ToString()))
                    this.BienServicio.Value = Convert.ToByte(dr["BienServicio"]);
                this.PagoInd.Value = Convert.ToBoolean(dr["PagoInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpDeclaracionRenglon()
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
            this.Renglon = new UDT_Renglon();
            this.Descriptivo = new UDT_DescripTBase();
            this.SignoSuma = new UDTSQL_tinyint();
            this.Tarifa = new UDT_PorcentajeID();
            this.CostosInd = new UDT_SiNo();
            this.BienServicio = new UDTSQL_tinyint();
            this.PagoInd = new UDT_SiNo();
        }

        public DTO_coImpDeclaracionRenglon(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_coImpDeclaracionRenglon(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ImpuestoDeclID { get; set; }

        [DataMember]
        public UDT_Descriptivo ImpuestoDeclDesc { get; set; }

        [DataMember]
        public UDT_Renglon Renglon { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_tinyint SignoSuma { get; set; }

        [DataMember]
        public UDT_PorcentajeID Tarifa { get; set; }

        [DataMember]
        public UDT_SiNo  CostosInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint BienServicio { get; set; }

        [DataMember]
        public UDT_SiNo PagoInd { get; set; }
    }
}
