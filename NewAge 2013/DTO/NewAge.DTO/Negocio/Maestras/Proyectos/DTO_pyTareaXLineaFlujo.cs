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
    /// Models DTO_pyTareaXLineaFlujo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyTareaXLineaFlujo : DTO_MasterComplex
    {
        #region pyTareaXLineaFlujo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyTareaXLineaFlujo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ClaseServicioDesc.Value = dr["ClaseServicioDesc"].ToString();
                    this.LineaFlujoDesc.Value = dr["LineaFlujoDesc"].ToString();
                    this.ActividadEtapaDesc.Value = dr["ActividadEtapaDesc"].ToString();
                    this.TareaDesc.Value = dr["TareaDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                }
                this.ClaseServicioID.Value = dr["ClaseServicioID"].ToString();
                this.LineaFlujoID.Value = dr["LineaFlujoID"].ToString();
                this.ActividadEtapaID.Value = dr["ActividadEtapaID"].ToString();
                this.TareaID.Value = dr["TareaID"].ToString();
                if (!string.IsNullOrEmpty(dr["Observacion"].ToString()))
                    this.Observacion.Value = Convert.ToString(dr["Observacion"]);
                this.CentroCostoID.Value = dr["CentroCostoID"].ToString();
                this.SemanaPrograma.Value = Convert.ToInt32(dr["SemanaPrograma"]);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_pyTareaXLineaFlujo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClaseServicioID =new UDT_BasicID();
            this.ClaseServicioDesc = new UDT_Descriptivo();
            this.LineaFlujoID =new UDT_BasicID();
            this.LineaFlujoDesc = new UDT_Descriptivo();
            this.ActividadEtapaID =new UDT_BasicID();
            this.ActividadEtapaDesc = new UDT_Descriptivo();
            this.TareaID = new UDT_BasicID();
            this.TareaDesc = new UDT_Descriptivo();
            this.Observacion=new UDT_DescripTExt();
            this.CentroCostoID =new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.SemanaPrograma = new UDTSQL_int();
        }

        public DTO_pyTareaXLineaFlujo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyTareaXLineaFlujo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID ClaseServicioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseServicioDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaFlujoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaFlujoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadEtapaID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadEtapaDesc { get; set; }

        [DataMember]
        public UDT_BasicID TareaID { get; set; }

        [DataMember]
        public UDT_Descriptivo TareaDesc { get; set; }

        [DataMember]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDT_BasicID CentroCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDTSQL_int SemanaPrograma { get; set; }
    }

}
