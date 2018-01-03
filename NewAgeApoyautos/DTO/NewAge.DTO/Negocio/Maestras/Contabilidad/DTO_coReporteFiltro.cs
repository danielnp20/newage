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
    /// Models DTO_coReporteLinea
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coReporteFiltro : DTO_MasterComplex
    {
        #region DTO_coReporteLinea
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coReporteFiltro(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if(!isReplica)
                {
                    this.ReporteDesc.Value = dr["ReporteDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.CentroCostoDesc.Value = dr["CentroCostoDesc"].ToString();
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.ComprobanteDesc.Value = dr["ComprobanteDesc"].ToString();
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                    this.FuenteUsoDesc.Value = dr["FuenteUsoDesc"].ToString();
                }

                this.ReporteID.Value = dr["ReporteID"].ToString();
                this.RepLineaID.Value = dr["RepLineaID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Cuenta"].ToString()))
                    this.Cuenta.Value = dr["Cuenta"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ConceptoCargo"].ToString()))
                this.ConceptoCargo.Value = dr["ConceptoCargo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["LineaPre"].ToString()))
                    this.LineaPre.Value = dr["LineaPre"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["CentroCosto"].ToString()))
                    this.CentroCosto.Value = dr["CentroCosto"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Proyecto"].ToString()))
                    this.Proyecto.Value = dr["Proyecto"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["TerceroID"].ToString()))
                    this.TerceroID.Value = dr["TerceroID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["FuenteUsoID"].ToString()))
                    this.FuenteUsoID.Value = dr["FuenteUsoID"].ToString();
                this.ExcluyeInd.Value = Convert.ToBoolean(dr["ExcluyeInd"]);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coReporteFiltro()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            //Inicializa las columnas
            this.ReporteID = new UDT_BasicID();
            this.ReporteDesc = new UDT_Descriptivo();
            this.RepLineaID = new UDT_BasicID();
            this.Cuenta = new  UDT_DescripTBase();
            this.ConceptoCargo = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.LineaPre = new UDT_BasicID();
            this.CentroCosto = new UDT_BasicID();
            this.CentroCostoDesc = new UDT_Descriptivo();
            this.Proyecto = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.ComprobanteID = new UDT_BasicID();
            this.ComprobanteDesc = new UDT_Descriptivo();
            this.TerceroID = new UDT_BasicID();
            this.TerceroDesc = new UDT_Descriptivo();
            this.FuenteUsoID = new UDT_BasicID();
            this.FuenteUsoDesc = new UDT_Descriptivo();
            this.ExcluyeInd = new UDT_SiNo();
        }

        public DTO_coReporteFiltro(DTO_MasterComplex comp)
            : base(comp)
        {
            InitCols();
        }

        public DTO_coReporteFiltro(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID ReporteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ReporteDesc { get; set; }

        [DataMember]
        public UDT_BasicID RepLineaID { get; set; }
        
        [DataMember]
        public UDT_DescripTBase Cuenta { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargo { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPre { get; set; }

        [DataMember]
        public UDT_BasicID CentroCosto { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCostoDesc { get; set; }

        [DataMember]
        public UDT_BasicID Proyecto { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteDesc { get; set; }

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID FuenteUsoID { get; set; }

        [DataMember]
        public UDT_Descriptivo FuenteUsoDesc { get; set; }

        [DataMember]
        public UDT_SiNo ExcluyeInd { get; set; }
    }
}
