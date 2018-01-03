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
    /// Models DTO_inMovimientoTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_inMovimientoTipo : DTO_MasterBasic
    {
        #region DTO_inMovimientoTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_inMovimientoTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.MvtoTipoInvRevDesc.Value = dr["MvtoTipoInvRevDesc"].ToString();
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                }

                this.TipoMovimiento.Value = Convert.ToByte(dr["TipoMovimiento"]);
                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.MvtoTipoReversion.Value = dr["MvtoTipoReversion"].ToString();
                if (!string.IsNullOrEmpty(dr["coDocumentoID"].ToString()))
                    this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                if (!string.IsNullOrEmpty(dr["TerceroInd"].ToString()))
                    this.TerceroInd.Value = Convert.ToBoolean(dr["TerceroInd"]);
                this.EstadoInv.Value = Convert.ToByte(dr["EstadoInv"]);
                if (!string.IsNullOrEmpty(dr["AjustaCostotInd"].ToString()))
                    this.AjustaCostotInd.Value = Convert.ToBoolean(dr["AjustaCostotInd"]);
                if (!string.IsNullOrEmpty(dr["LibroFuncionalInd"].ToString()))
                    this.LibroFuncionalInd.Value = Convert.ToBoolean(dr["LibroFuncionalInd"]);
                if (!string.IsNullOrEmpty(dr["LibroIFRSInd"].ToString()))
                    this.LibroIFRSInd.Value = Convert.ToBoolean(dr["LibroIFRSInd"]);
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_inMovimientoTipo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoMovimiento = new UDTSQL_tinyint();
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.MvtoTipoReversion = new UDT_BasicID();
            this.MvtoTipoInvRevDesc = new UDT_Descriptivo();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.TerceroInd = new UDT_SiNo();
            this.EstadoInv = new UDTSQL_tinyint();
            this.AjustaCostotInd = new UDT_SiNo();
            this.LibroFuncionalInd = new UDT_SiNo();
            this.LibroIFRSInd = new UDT_SiNo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
        }

        public DTO_inMovimientoTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_inMovimientoTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoMovimiento { get; set; }

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }

        [DataMember]
        public UDT_BasicID MvtoTipoReversion { get; set; }

        [DataMember]
        public UDT_Descriptivo MvtoTipoInvRevDesc { get; set; }

        [DataMember]
        public UDT_SiNo TerceroInd { get; set; }

        [DataMember]
        public UDTSQL_tinyint EstadoInv { get; set; }

        [DataMember]
        public UDT_SiNo AjustaCostotInd { get; set; }

        [DataMember]
        public UDT_SiNo LibroFuncionalInd { get; set; }

        [DataMember]
        public UDT_SiNo LibroIFRSInd { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

    }

}
