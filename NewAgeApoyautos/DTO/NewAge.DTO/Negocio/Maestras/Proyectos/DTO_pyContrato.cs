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
    /// Models DTO_pyContrato
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_pyContrato : DTO_MasterBasic
    {
        #region pyContrato
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_pyContrato(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ProyectoDesc.Value = dr["ProyectoDesc"].ToString();
                    this.SocioDesc.Value = dr["ProyectoDesc"].ToString();
                    this.ClienteDesc.Value = dr["ClienteID"].ToString();
                    this.ListaPrecioDesc.Value = dr["ListaPrecioID"].ToString();
                }
                this.TipoContrato.Value = Convert.ToByte(dr["TipoContrato"]);
                this.LineaPrexTrabajoIND.Value = Convert.ToBoolean(dr["LineaPrexTrabajoIND"]);
                if (!string.IsNullOrEmpty(dr["ProyectoID"].ToString()))
                    this.ProyectoID.Value = dr["ProyectoID"].ToString();
                if (!string.IsNullOrEmpty(dr["SocioOperador"].ToString()))
                    this.SocioOperador.Value = dr["SocioOperador"].ToString();
                if (!string.IsNullOrEmpty(dr["ClienteID"].ToString()))
                    this.ClienteID.Value = dr["ClienteID"].ToString();
                if (!string.IsNullOrEmpty(dr["ListaPrecioID"].ToString()))
                    this.ListaPrecioID.Value = dr["ListaPrecioID"].ToString();
                if (!string.IsNullOrEmpty(dr["RteGarantiaPor"].ToString()))
                    this.RteGarantiaPor.Value = Convert.ToDecimal(dr["RteGarantiaPor"]);
                if (!string.IsNullOrEmpty(dr["MonedaInforme"].ToString()))
                    this.MonedaInforme.Value = Convert.ToByte(dr["MonedaInforme"]);
                if (!string.IsNullOrEmpty(dr["DiasPago"].ToString()))
                    this.DiasPago.Value = Convert.ToByte(dr["DiasPago"]);
                if (!string.IsNullOrEmpty(dr["FormaPago"].ToString()))
                    this.FormaPago.Value = Convert.ToByte(dr["FormaPago"]);
                if (!string.IsNullOrEmpty(dr["VlrAnticipo"].ToString()))
                    this.VlrAnticipo.Value = Convert.ToDecimal(dr["VlrAnticipo"]);  
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>g
        public DTO_pyContrato()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoContrato = new UDTSQL_tinyint();
            this.LineaPrexTrabajoIND = new UDT_SiNo();
            this.ProyectoID = new UDT_BasicID();
            this.ProyectoDesc = new UDT_Descriptivo();
            this.SocioOperador = new UDT_BasicID();
            this.SocioDesc = new UDT_Descriptivo();
            this.ClienteID = new UDT_BasicID();
            this.ClienteDesc = new UDT_Descriptivo();
            this.ListaPrecioID = new UDT_BasicID();
            this.ListaPrecioDesc = new UDT_Descriptivo();
            this.RteGarantiaPor = new UDT_PorcentajeID();
            this.MonedaInforme = new UDTSQL_tinyint();
            this.DiasPago = new UDTSQL_tinyint();
            this.FormaPago = new UDTSQL_tinyint();
            this.VlrAnticipo = new UDT_Valor();
        }

        public DTO_pyContrato(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_pyContrato(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoContrato { get; set; }

        [DataMember]
        public UDT_SiNo LineaPrexTrabajoIND { get; set; }

        [DataMember]
        public UDT_BasicID SocioOperador { get; set; }

        [DataMember]
        public UDT_Descriptivo SocioDesc { get; set; }

        [DataMember]
        public UDT_BasicID ClienteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ClienteDesc { get; set; }

        [DataMember]
        public UDT_BasicID ListaPrecioID { get; set; }

        [DataMember]
        public UDT_Descriptivo ListaPrecioDesc { get; set; }

        [DataMember]
        public UDT_BasicID ProyectoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ProyectoDesc { get; set; }

        [DataMember]
        public UDT_PorcentajeID RteGarantiaPor { get; set; }

        [DataMember]
        public UDTSQL_tinyint MonedaInforme { get; set; }

        [DataMember]
        public UDTSQL_tinyint DiasPago { get; set; }

        [DataMember]
        public UDTSQL_tinyint FormaPago { get; set; }

        [DataMember]
        public UDT_Valor VlrAnticipo { get; set; }
    }

}
