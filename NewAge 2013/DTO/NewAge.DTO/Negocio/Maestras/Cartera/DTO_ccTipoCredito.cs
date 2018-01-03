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
    /// Models DTO_ccTipoCredito
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccTipoCredito : DTO_MasterBasic
    {
        #region DTO_ccTipoCredito
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccTipoCredito(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                this.TipoCredito.Value = Convert.ToByte(dr["TipoCredito"]);
                if (!string.IsNullOrEmpty(dr["CancelaGarInd"].ToString()))
                    this.CancelaGarInd.Value = Convert.ToBoolean(dr["CancelaGarInd"]);
                if (!string.IsNullOrEmpty(dr["AbonaCapitalInd"].ToString()))
                    this.AbonaCapitalInd.Value = Convert.ToBoolean(dr["AbonaCapitalInd"]);
                if (!string.IsNullOrEmpty(dr["CuotaExtraInd"].ToString()))
                    this.CuotaExtraInd.Value= Convert.ToBoolean(dr["CuotaExtraInd"]);
                if (!string.IsNullOrEmpty(dr["CancelaCreditoInd"].ToString()))
                    this.CancelaCreditoInd.Value = Convert.ToBoolean(dr["CancelaCreditoInd"]);
                this.OrigenSolicitud.Value = Convert.ToByte(dr["OrigenSolicitud"]);
                if (!string.IsNullOrEmpty(dr["NuevoClienteInd"].ToString()))
                    this.NuevoClienteInd.Value = Convert.ToBoolean(dr["NuevoClienteInd"]);

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccTipoCredito()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoCredito = new UDTSQL_tinyint();
            this.CancelaGarInd = new UDT_SiNo();
            this.AbonaCapitalInd = new UDT_SiNo();
            this.CuotaExtraInd= new UDT_SiNo();
            this.CancelaCreditoInd = new UDT_SiNo();
            this.OrigenSolicitud = new UDTSQL_tinyint();
            this.NuevoClienteInd = new UDT_SiNo();

        }

        public DTO_ccTipoCredito(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccTipoCredito(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoCredito { get; set; }

        [DataMember]
        public UDT_SiNo CancelaGarInd { get; set; }

        [DataMember]
        public UDT_SiNo AbonaCapitalInd { get; set; }

        [DataMember]
        public UDT_SiNo CuotaExtraInd { get; set; }
        [DataMember]
        public UDT_SiNo CancelaCreditoInd { get; set; }
        [DataMember]
        public UDTSQL_tinyint OrigenSolicitud { get; set; }
        [DataMember]
        public UDT_SiNo NuevoClienteInd { get; set; }

    }

}
