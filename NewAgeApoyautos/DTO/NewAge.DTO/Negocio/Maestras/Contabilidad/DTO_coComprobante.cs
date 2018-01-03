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
    /// Models DTO_coComprobante
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coComprobante : DTO_MasterBasic 
    {
        #region DTO_coComprobante
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coComprobante(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ModuloDesc.Value = dr["ModuloDesc"].ToString();
                    this.ComprobanteAnulDesc.Value = dr["ComprobanteAnulDesc"].ToString();
                    this.BalanceTipoDesc.Value = dr["BalanceTipoDesc"].ToString();
                    this.ComprobanteIFRSDesc.Value = dr["ComprobanteIFRSDesc"].ToString();
                }

                this.ModuloID.Value = dr["ModuloID"].ToString();
                this.ComprobanteAnulID.Value = dr["ComprobanteAnulID"].ToString();
                this.BalanceTipoID.Value = dr["BalanceTipoID"].ToString();
                this.TipoConsec.Value = Convert.ToByte(dr["TipoConsec"]);
                this.LocalInd.Value = Convert.ToBoolean(dr["LocalInd"]);
                this.biMonedaInd.Value = Convert.ToBoolean(dr["biMonedaInd"]);
                this.MesNoCerradoInd.Value = Convert.ToBoolean(dr["MesNoCerradoInd"]);
                this.InfExogenaInd.Value = Convert.ToBoolean(dr["InfExogenaInd"]);
                this.AprobacionObligInd.Value = Convert.ToBoolean(dr["AprobacionObligInd"]);
                this.ExclEjecucionPresupInd.Value = Convert.ToBoolean(dr["ExclEjecucionPresupInd"]);
                this.ExclLibroIFRSInd.Value = Convert.ToBoolean(dr["ExclLibroIFRSInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ComprobanteIFRS"].ToString()))
                    this.ComprobanteIFRS.Value = dr["ComprobanteIFRS"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coComprobante() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ModuloID = new UDT_BasicID();
            this.ModuloDesc = new UDT_Descriptivo();
            this.ComprobanteAnulID = new UDT_BasicID();
            this.ComprobanteAnulDesc = new UDT_Descriptivo();
            this.TipoConsec = new UDTSQL_tinyint();
            this.BalanceTipoID = new UDT_BasicID();
            this.BalanceTipoDesc = new UDT_Descriptivo();
            this.LocalInd = new UDT_SiNo();
            this.biMonedaInd = new UDT_SiNo();
            this.MesNoCerradoInd = new UDT_SiNo();
            this.InfExogenaInd = new UDT_SiNo();
            this.ExclEjecucionPresupInd = new UDT_SiNo();
            this.ExclLibroIFRSInd = new UDT_SiNo();
            this.AprobacionObligInd = new UDT_SiNo();
            this.ComprobanteIFRS = new UDT_BasicID();
            this.ComprobanteIFRSDesc = new UDT_Descriptivo();
        }

        public DTO_coComprobante(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coComprobante(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        #region Propiedades
        [DataMember]
        public UDT_BasicID ModuloID { get; set; }

        [DataMember]
        public UDT_Descriptivo ModuloDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComprobanteAnulID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteAnulDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoConsec { get; set; }

        [DataMember]
        public UDT_SiNo LocalInd { get; set; }

        [DataMember]
        public UDT_BasicID BalanceTipoID { get; set; }

        [DataMember]
        public UDT_Descriptivo BalanceTipoDesc { get; set; }

        [DataMember]
        public UDT_SiNo biMonedaInd { get; set; }

        [DataMember]
        public UDT_SiNo MesNoCerradoInd { get; set; }

        [DataMember]
        public UDT_SiNo InfExogenaInd { get; set; }

        [DataMember]
        public UDT_SiNo ExclEjecucionPresupInd { get; set; }

        [DataMember]
        public UDT_SiNo ExclLibroIFRSInd { get; set; }

        [DataMember]
        public UDT_SiNo AprobacionObligInd { get; set; }

        [DataMember]
        public UDT_BasicID ComprobanteIFRS { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteIFRSDesc { get; set; }


        #endregion
    }

}
