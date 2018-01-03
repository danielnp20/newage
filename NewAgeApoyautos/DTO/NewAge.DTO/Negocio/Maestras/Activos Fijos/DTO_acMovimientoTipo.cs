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
    /// Models DTO_acMovimientoTipo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_acMovimientoTipo : DTO_MasterBasic
    {
        #region DTO_acMovimientoTipo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_acMovimientoTipo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.coDocumentoDesc.Value = dr["coDocumentoDesc"].ToString();
                    this.MvtoTipoReversionDesc.Value = dr["MvtoTipoReversionDesc"].ToString();
                    this.EstadoActDesc.Value = dr["EstadoActDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                }

                this.coDocumentoID.Value = dr["coDocumentoID"].ToString();
                this.TipoMvto.Value = Convert.ToByte(dr["TipoMvto"]);
                this.IniciaDepreInd.Value = Convert.ToBoolean(dr["IniciaDepreInd"]);
                this.MvtoTipoReversion.Value = dr["MvtoTipoReversion"].ToString();
                this.DetieneDepreInd.Value = Convert.ToBoolean(dr["DetieneDepreInd"]);
                this.EstadoActID.Value = dr["EstadoActID"].ToString();
                if (!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.DetieneDepreIFRSInd.Value = Convert.ToBoolean(dr["DetieneDepreIFRSInd"]);
                this.ModificaDepreciacionInd.Value = Convert.ToBoolean(dr["ModificaDepreciacionInd"]);

            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_acMovimientoTipo()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.coDocumentoID = new UDT_BasicID();
            this.coDocumentoDesc = new UDT_Descriptivo();
            this.TipoMvto = new UDTSQL_tinyint();
            this.IniciaDepreInd = new UDT_SiNo();
            this.MvtoTipoReversion = new UDT_BasicID();
            this.MvtoTipoReversionDesc = new UDT_Descriptivo();
            this.DetieneDepreInd = new UDT_SiNo();
            this.EstadoActID = new UDT_BasicID();
            this.EstadoActDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.DetieneDepreIFRSInd = new UDT_SiNo();
            this.ModificaDepreciacionInd = new UDT_SiNo();
        }

        public DTO_acMovimientoTipo(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_acMovimientoTipo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID coDocumentoID { get; set; }

        [DataMember]
        public UDT_Descriptivo coDocumentoDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoMvto { get; set; }

        [DataMember]
        public UDT_SiNo IniciaDepreInd { get; set; }

        [DataMember]
        public UDT_BasicID MvtoTipoReversion { get; set; }

        [DataMember]
        public UDT_Descriptivo MvtoTipoReversionDesc { get; set; }

        [DataMember]
        public UDT_SiNo DetieneDepreInd { get; set; }

        [DataMember]
        public UDT_BasicID EstadoActID { get; set; }

        [DataMember]
        public UDT_Descriptivo EstadoActDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_SiNo DetieneDepreIFRSInd { get; set; }

        [DataMember]
        public UDT_SiNo ModificaDepreciacionInd { get; set; }
    }
}