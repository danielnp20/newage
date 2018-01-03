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
    /// Models DTO_cpCargoEspecial
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_cpCargoEspecial : DTO_MasterBasic
    {
        #region DTO_cpCargoEspecial

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_cpCargoEspecial(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
                InitCols();
                try
                {
                    if (!isReplica)
                    {
                        this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                        this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                        this.ConceptoCargoIVA1Desc.Value = dr["ConceptoCargoIVA1Desc"].ToString();
                    }

                    this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                    this.ConceptoCargo1.Value = dr["ConceptoCargo1"].ToString();
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                    this.CargoTipo.Value = Convert.ToByte(dr["CargoTipo"]);
                    this.CajaMenorInd.Value = Convert.ToBoolean(dr["CajaMenorInd"]);
                    this.LegalizaGastoInd.Value = Convert.ToBoolean(dr["LegalizaGastoInd"]);
                    this.TarjetasInd.Value = Convert.ToBoolean(dr["TarjetasInd"]);
                    this.TarjetasPagoInd.Value = Convert.ToBoolean(dr["TarjetasPagoInd"]);
                }
                catch (Exception e)
                {                    
                    throw e;
                }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_cpCargoEspecial() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.ConceptoCargo1 = new UDT_BasicID();
            this.ConceptoCargoIVA1Desc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.CargoTipo = new UDTSQL_tinyint();
            this.CajaMenorInd = new UDT_SiNo();
            this.LegalizaGastoInd = new UDT_SiNo();
            this.TarjetasInd = new UDT_SiNo();
            this.TarjetasPagoInd = new UDT_SiNo();
        }

        public DTO_cpCargoEspecial(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_cpCargoEspecial(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargo1 { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoIVA1Desc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint CargoTipo { get; set; }

        [DataMember]
        public UDT_SiNo CajaMenorInd { get; set; }

        [DataMember]
        public UDT_SiNo LegalizaGastoInd { get; set; }

        [DataMember]
        public UDT_SiNo TarjetasInd { get; set; }

        [DataMember]
        public UDT_SiNo TarjetasPagoInd { get; set; }

    }
}
