using System;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_acQueryActivoControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acQueryActivoControl
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acQueryActivoControl(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["PlaquetaID"].ToString()))
                    this.PlaquetaID.Value = (dr["PlaquetaID"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["SerialID"].ToString()))
                    this.SerialID.Value = (dr["SerialID"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Observacion"].ToString()))
                    this.Observacion.Value = (dr["Observacion"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["FechaCompra"].ToString()))
                    this.FechaCompra.Value = Convert.ToDateTime(dr["FechaCompra"]);
                if (!string.IsNullOrWhiteSpace(dr["Proveedor"].ToString()))
                    this.Proveedor.Value = (dr["Proveedor"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Factura"].ToString()))
                    this.Factura.Value = (dr["Factura"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["TipoDepreLOC"].ToString()))
                    this.TipoDepreLOC.Value = Convert.ToByte(dr["TipoDepreLOC"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoDepreIFRS"].ToString()))
                    this.TipoDepreIFRS.Value = Convert.ToByte(dr["TipoDepreIFRS"]);
                if (!string.IsNullOrWhiteSpace(dr["TipoDepreUSG"].ToString()))
                    this.TipoDepreUSG.Value = Convert.ToByte(dr["TipoDepreUSG"]);
                if (!string.IsNullOrWhiteSpace(dr["VidaUtilLOC"].ToString()))
                    this.VidaUtilLOC.Value = Convert.ToByte(dr["VidaUtilLOC"]);
                if (!string.IsNullOrWhiteSpace(dr["VidaUtilIFRS"].ToString()))
                    this.VidaUtilIFRS.Value = Convert.ToByte(dr["VidaUtilIFRS"]);
                if (!string.IsNullOrWhiteSpace(dr["VidaUtilUSG"].ToString()))
                    this.VidaUtilUSG.Value = Convert.ToByte(dr["VidaUtilUSG"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorSalvamentoLOC"].ToString()))
                    this.ValorSalvamentoLOC.Value = Convert.ToDecimal(dr["ValorSalvamentoLOC"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorSalvamentoIFRS"].ToString()))
                    this.ValorSalvamentoIFRS.Value = Convert.ToDecimal(dr["ValorSalvamentoIFRS"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorSalvamentoUSG"].ToString()))
                    this.ValorSalvamentoUSG.Value = Convert.ToDecimal(dr["ValorSalvamentoUSG"]);
                if (!string.IsNullOrWhiteSpace(dr["ValorRetiroIFRS"].ToString()))
                    this.ValorRetiroIFRS.Value = Convert.ToDecimal(dr["ValorRetiroIFRS"]);
                if (!string.IsNullOrWhiteSpace(dr["ActivoGrupo"].ToString()))
                    this.ActivoGrupo.Value = (dr["ActivoGrupo"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["ActivoClase"].ToString()))
                    this.ActivoClase.Value = (dr["ActivoClase"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["ActivoTipo"].ToString()))
                    this.ActivoTipo.Value = (dr["ActivoTipo"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Modelo"].ToString()))
                    this.Modelo.Value = (dr["Modelo"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["Proyecto"].ToString()))
                    this.Proyecto.Value = (dr["Proyecto"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["CentroCosto"].ToString()))
                    this.CentroCosto.Value = (dr["CentroCosto"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["LocFisica"].ToString()))
                    this.LocFisica.Value = (dr["LocFisica"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["ActivoPadreID"].ToString()))
                    this.ActivoPadreID.Value = Convert.ToInt32(dr["ActivoPadreID"]);
                if (!string.IsNullOrWhiteSpace(dr["Responsable"].ToString()))
                    this.Responsable.Value = (dr["Responsable"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["inReferenciaID"].ToString()))
                    this.inReferenciaID.Value = (dr["inReferenciaID"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["ActivoID"].ToString()))
                    this.ActivoID.Value = Convert.ToInt32(dr["ActivoID"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acQueryActivoControl()
        {
            InitCols();
        }
       
        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {

            this.PlaquetaID = new UDT_PlaquetaID();
            this.SerialID = new UDTSQL_char(25);
            this.Observacion = new UDT_DescripTExt();
            this.FechaCompra = new UDTSQL_smalldatetime();
            this.Proveedor = new UDT_Descriptivo();
            this.Factura = new UDTSQL_char(20);
            this.TipoDepreLOC = new UDTSQL_tinyint();
            this.TipoDepreIFRS = new UDTSQL_tinyint();
            this.TipoDepreUSG = new UDTSQL_tinyint();
            this.VidaUtilLOC = new UDTSQL_int();
            this.VidaUtilIFRS = new UDTSQL_int();
            this.VidaUtilUSG = new UDTSQL_int();
            this.ValorSalvamentoLOC = new UDT_Valor();
            this.ValorSalvamentoIFRS = new UDT_Valor();
            this.ValorSalvamentoUSG = new UDT_Valor();
            this.ValorRetiroIFRS = new UDT_Valor();
            this.ActivoGrupo = new UDT_Descriptivo();
            this.ActivoClase = new UDT_Descriptivo();
            this.ActivoTipo = new UDT_Descriptivo();
            this.Modelo = new UDTSQL_char(20);
            this.Proyecto = new UDT_Descriptivo();
            this.CentroCosto = new UDT_Descriptivo();
            this.LocFisica = new UDT_Descriptivo();
            this.ActivoPadreID = new UDT_Consecutivo();
            this.Responsable = new UDT_TerceroTipoID();
            this.inReferenciaID = new UDT_inReferenciaID();
            this.ActivoID = new UDT_ActivoID();
        }
        #endregion

        #region Propiedades


        [DataMember]
        public UDT_PlaquetaID PlaquetaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char SerialID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Observacion { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime FechaCompra { get; set; }

        [DataMember]
        public UDT_Descriptivo Proveedor { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char Factura { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDepreLOC { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoDepreIFRS { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint TipoDepreUSG { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilLOC { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilIFRS { get; set; }

        [DataMember]
        public UDTSQL_int VidaUtilUSG { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoLOC { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoIFRS { get; set; }

        [DataMember]
        public UDT_Valor ValorSalvamentoUSG { get; set; }

        [DataMember]
        public UDT_Valor ValorRetiroIFRS { get; set; }

        [DataMember]
        public UDT_Descriptivo ActivoGrupo { get; set; }

        [DataMember]
        public UDT_Descriptivo ActivoClase { get; set; }

        [DataMember]
        public UDT_Descriptivo ActivoTipo { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_char Modelo { get; set; }

        [DataMember]
        public UDT_Descriptivo Proyecto { get; set; }

        [DataMember]
        public UDT_Descriptivo CentroCosto { get; set; }

        [DataMember]
        public UDT_Descriptivo LocFisica { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Consecutivo ActivoPadreID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_TerceroTipoID Responsable { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_inReferenciaID inReferenciaID { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ActivoID ActivoID { get; set; }

        #endregion
    }
}
