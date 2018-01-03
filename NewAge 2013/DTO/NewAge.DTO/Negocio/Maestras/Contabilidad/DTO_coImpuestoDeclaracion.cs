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
    /// Models DTO_coImpuestoDeclaracion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coImpuestoDeclaracion : DTO_MasterBasic
    {
        #region DTO_coImpuestoDeclaracion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coImpuestoDeclaracion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.LugarGeoDesc.Value = dr["LugarGeoDesc"].ToString();
                    this.ImpuestoTipoDesc.Value = dr["ImpuestoTipoDesc"].ToString();
                }

                this.PeriodoDeclaracion.Value = Convert.ToByte(dr["PeriodoDeclaracion"]);
                this.DigitoAprox.Value = Convert.ToByte(dr["DigitoAprox"]);
                this.LugarGeograficoID.Value = (dr["LugarGeograficoID"]).ToString();
                this.ImpuestoTipoID.Value = (dr["ImpuestoTipoID"]).ToString();
                this.MunicipalInd.Value = Convert.ToBoolean(dr["MunicipalInd"]);
            }
            catch (Exception e)
            {                
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coImpuestoDeclaracion() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoDeclaracion = new UDT_PeriodoCert();
            this.DigitoAprox = new UDTSQL_tinyint();
            this.LugarGeograficoID = new UDT_BasicID();
            this.LugarGeoDesc = new UDT_Descriptivo();
            this.ImpuestoTipoID = new UDT_BasicID();
            this.ImpuestoTipoDesc = new UDT_Descriptivo();
            this.MunicipalInd = new UDT_SiNo();

        }

        public DTO_coImpuestoDeclaracion(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coImpuestoDeclaracion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_PeriodoCert PeriodoDeclaracion { get; set; }

        [DataMember]
        public UDTSQL_tinyint DigitoAprox { get; set; }

        [DataMember]
        public UDT_BasicID LugarGeograficoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LugarGeoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ImpuestoTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ImpuestoTipoDesc { get; set; }

        [DataMember]
        public UDT_SiNo MunicipalInd { get; set; }

        
  
    }
}
