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
    /// Models DTO_glDocumento
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_glDocumento : DTO_MasterBasic
    {
        #region DTO_glDocumento

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_glDocumento(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ModuloDesc.Value = dr["ModuloDesc"].ToString();
                    this.DocumentoAnulaDesc.Value = dr["DocumentoAnulaDesc"].ToString();
                    this.TareaIncumplimientoDesc.Value = dr["TareaIncumplimientoDesc"].ToString();
                }

                this.ModuloID.Value = dr["ModuloID"].ToString();
                this.DocumentoTipo.Value = Convert.ToByte(dr["DocumentoTipo"]);
                if (!string.IsNullOrWhiteSpace(dr["DocAnula"].ToString()))
                    this.DocAnula.Value = dr["DocAnula"].ToString();
                this.PrefijoTipo.Value = Convert.ToByte(dr["PrefijoTipo"]);
                this.Observaciones.Value = dr["Observaciones"].ToString();
                this.DocExternoInd.Value = Convert.ToBoolean(dr["DocExternoInd"]);
                this.TablaDocumento.Value = dr["TablaDocumento"] != null ? dr["TablaDocumento"].ToString() : string.Empty;
                this.AjustaComprobanteInd.Value = Convert.ToBoolean(dr["AjustaComprobanteInd"]); 
                this.ValidaValorInd.Value = Convert.ToBoolean(dr["ValidaValorInd"]);
                this.NivelInd.Value = Convert.ToBoolean(dr["NivelInd"]);
                this.NivelAprobacionInd.Value = Convert.ToBoolean(dr["NivelAprobacionInd"]);
                this.AprobacionValorInd.Value = Convert.ToBoolean(dr["AprobacionValorInd"]);
                this.AprobacionEspecialInd.Value = Convert.ToBoolean(dr["AprobacionEspecialInd"]);
                this.ComprobanteDocumInd.Value = Convert.ToBoolean(dr["ComprobanteDocumInd"]);
                this.TareaIncumplimiento.Value = Convert.ToString(dr["TareaIncumplimiento"]);
                this.DocUnicoInd.Value = Convert.ToBoolean(dr["DocUnicoInd"]);
                this.DocCxPGeneraInd.Value = Convert.ToBoolean(dr["DocCxPGeneraInd"]);
                if (!string.IsNullOrWhiteSpace(dr["NOLibroFuncionalInd"].ToString()))
                    this.NOLibroFuncionalInd.Value = Convert.ToBoolean(dr["NOLibroFuncionalInd"]);
                if (!string.IsNullOrWhiteSpace(dr["ConsultaxCtoCostoInd"].ToString()))
                    this.ConsultaxCtoCostoInd.Value = Convert.ToBoolean(dr["ConsultaxCtoCostoInd"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_glDocumento()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ModuloID = new UDT_BasicID();
            this.ModuloDesc = new UDT_Descriptivo();
            this.DocumentoTipo = new UDTSQL_tinyint();
            this.DocAnula = new UDT_BasicID();
            this.DocumentoAnulaDesc = new UDT_Descriptivo();
            this.PrefijoTipo = new UDTSQL_tinyint();
            this.Asignacion = new UDT_SiNo();
            this.Preaprobacion = new UDT_SiNo();
            this.Observaciones = new UDT_DescripTExt();
            this.TablaDocumento = new UDT_Descriptivo();
            this.AjustaComprobanteInd = new UDT_SiNo();
            this.DocExternoInd = new UDT_SiNo();
            this.ValidaValorInd = new UDT_SiNo();
            this.NivelInd = new UDT_SiNo();
            this.NivelAprobacionInd = new UDT_SiNo();
            this.AprobacionValorInd = new UDT_SiNo();
            this.AprobacionEspecialInd = new UDT_SiNo();
            this.ComprobanteDocumInd = new UDT_SiNo();
            this.TareaIncumplimiento = new UDT_BasicID();
            this.TareaIncumplimientoDesc = new UDT_Descriptivo();
            this.DocUnicoInd = new UDT_SiNo();
            this.DocCxPGeneraInd = new UDT_SiNo();
            this.NOLibroFuncionalInd = new UDT_SiNo();
            this.ConsultaxCtoCostoInd = new UDT_SiNo();
        }

        public DTO_glDocumento(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_glDocumento(DTO_aplMaestraPropiedades masterProperties)
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
        public UDTSQL_tinyint DocumentoTipo { get; set; }

        [DataMember]
        public UDT_BasicID DocAnula { get; set; }

        [DataMember]
        public UDT_Descriptivo DocumentoAnulaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint PrefijoTipo { get; set; }

        [DataMember]
        public UDT_SiNo Asignacion { get; set; }

        [DataMember]
        public UDT_SiNo Preaprobacion { get; set; }

        [DataMember]
        public UDT_DescripTExt Observaciones { get; set; }

        [DataMember]
        public UDT_Descriptivo TablaDocumento { get; set; }

        [DataMember]
        public UDT_SiNo AjustaComprobanteInd { get; set; }

        [DataMember]
        public UDT_SiNo DocExternoInd { get; set; }

        [DataMember]
        public UDT_SiNo DocUnicoInd{ get; set; }

        [DataMember]
        public UDT_SiNo DocCxPGeneraInd{ get; set; }

        [DataMember]
        public UDT_SiNo ValidaValorInd { get; set; }

        [DataMember]
        public UDT_SiNo NivelInd { get; set; }

        [DataMember]
        public UDT_SiNo NivelAprobacionInd { get; set; }

        [DataMember]
        public UDT_SiNo AprobacionValorInd { get; set; }

        [DataMember]
        public UDT_SiNo AprobacionEspecialInd { get; set; }

        [DataMember]
        public UDT_SiNo ComprobanteDocumInd { get; set; }

        [DataMember]
        public UDT_BasicID TareaIncumplimiento { get; set; }

        [DataMember]
        public UDT_Descriptivo TareaIncumplimientoDesc { get; set; }

        [DataMember]
        public UDT_SiNo NOLibroFuncionalInd { get; set; }

        [DataMember]
        public UDT_SiNo ConsultaxCtoCostoInd { get; set; }

        #endregion
    }
}
