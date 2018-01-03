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
    /// Models DTO_coDocumento
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coDocumento : DTO_MasterBasic 
    {
        #region DTO_coDocumento
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coDocumento(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.PrefijoDesc.Value = dr["PrefijoDesc"].ToString();
                    this.DocumentoDesc.Value = dr["DocumentoDesc"].ToString();
                    this.CuentaLocDesc.Value = dr["CuentaLocDesc"].ToString();
                    this.CuentaExtDesc.Value = dr["CuentaExtDesc"].ToString();
                    this.CuentaIFRSLOCDesc.Value = dr["CuentaIFRSLOCDesc"].ToString();
                    this.CuentaIFRSEXTDesc.Value = dr["CuentaIFRSEXTDesc"].ToString();
                    this.ComprobanteDesc.Value = dr["ComprobanteDesc"].ToString();
                    this.seGrupoDesc.Value = dr["seGrupoDesc"].ToString();
                    this.NotaRevelacionDesc.Value = dr["NotaRevelacionDesc"].ToString();
                }
                if (!string.IsNullOrEmpty(dr["PrefijoID"].ToString())) 
                    this.PrefijoID.Value = dr["PrefijoID"].ToString();
                this.DocumentoID.Value = dr["DocumentoID"].ToString(); 
                this.MonedaOrigen.Value = Convert.ToByte(dr["MonedaOrigen"]);
                if (!string.IsNullOrEmpty(dr["CuentaLOC"].ToString()))
                    this.CuentaLOC.Value = dr["CuentaLOC"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaEXT"].ToString()))
                    this.CuentaEXT.Value = dr["CuentaEXT"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaIFRSLOC"].ToString()))
                    this.CuentaIFRSLOC.Value = dr["CuentaLOC"].ToString();
                if (!string.IsNullOrEmpty(dr["CuentaIFRSEXT"].ToString()))
                    this.CuentaIFRSEXT.Value = dr["CuentaIFRSEXT"].ToString();
                this.TipoComprobante.Value = Convert.ToByte(dr["TipoComprobante"]);
                if (!string.IsNullOrEmpty(dr["ComprobanteID"].ToString()))
                    this.ComprobanteID.Value = dr["ComprobanteID"].ToString();
                this.seGrupoID.Value = dr["seGrupoID"].ToString();
                if (!string.IsNullOrEmpty(dr["NotaRevelacionID"].ToString()))
                    this.NotaRevelacionID.Value = dr["NotaRevelacionID"].ToString();
                if (!string.IsNullOrEmpty(dr["DistribuyeCostoInd"].ToString()))
                    this.DistribuyeCostoInd.Value = Convert.ToBoolean(dr["DistribuyeCostoInd"]);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coDocumento() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PrefijoID = new UDT_BasicID();
            this.PrefijoDesc = new UDT_Descriptivo();
            this.DocumentoID = new UDT_BasicID();
            this.DocumentoDesc = new UDT_Descriptivo();
            this.MonedaOrigen = new UDTSQL_tinyint();
            this.CuentaLOC = new UDT_BasicID();
            this.CuentaLocDesc = new UDT_Descriptivo();
            this.CuentaEXT = new UDT_BasicID();
            this.CuentaExtDesc = new UDT_Descriptivo();
            this.CuentaIFRSLOC = new UDT_BasicID();
            this.CuentaIFRSLOCDesc = new UDT_Descriptivo();
            this.CuentaIFRSEXT = new UDT_BasicID();
            this.CuentaIFRSEXTDesc = new UDT_Descriptivo();
            this.TipoComprobante = new UDTSQL_tinyint();
            this.ComprobanteID = new UDT_BasicID();
            this.ComprobanteDesc = new UDT_Descriptivo();
            this.seGrupoID = new UDT_BasicID();
            this.seGrupoDesc = new UDT_Descriptivo();
            this.NotaRevelacionID = new UDT_BasicID();
            this.NotaRevelacionDesc = new UDT_Descriptivo();
            this.DistribuyeCostoInd = new UDT_SiNo();
        }

        public DTO_coDocumento(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coDocumento(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        #region Propiedades

        [DataMember]
        public UDT_BasicID PrefijoID { get; set; }

        [DataMember]
        public UDT_Descriptivo PrefijoDesc { get; set; }

        [DataMember]
        public UDT_BasicID DocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint MonedaOrigen { get; set; }

        [DataMember]
        public UDT_BasicID CuentaLOC { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaLocDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaEXT { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaExtDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaIFRSLOC { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaIFRSLOCDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaIFRSEXT { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaIFRSEXTDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoComprobante { get; set; }

        [DataMember]
        public UDT_BasicID ComprobanteID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComprobanteDesc { get; set; }

        [DataMember]
        public UDT_BasicID seGrupoID { get; set; }

        [DataMember]
        public UDT_Descriptivo seGrupoDesc { get; set; }

        [DataMember]
        public UDT_BasicID NotaRevelacionID{ get; set; }

        [DataMember]
        public UDT_Descriptivo NotaRevelacionDesc{ get; set; } 

        [DataMember]
        public UDT_SiNo DistribuyeCostoInd { get; set; } 

        #endregion

    }

}
