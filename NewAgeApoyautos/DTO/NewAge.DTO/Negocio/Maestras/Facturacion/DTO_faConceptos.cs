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
    /// Models DTO_faConceptos
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_faConceptos : DTO_MasterBasic
    {
        #region DTO_faCliente
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_faConceptos(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.ConceptoCargoDesc.Value = dr["ConceptoCargoDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.MvtoTipoInvDesc.Value = dr["MvtoTipoInvDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                }

                this.TipoConcepto.Value = Convert.ToByte(dr["TipoConcepto"]);
                this.ConceptoCargoID.Value = dr["ConceptoCargoID"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.MvtoTipoInvID.Value = dr["MvtoTipoInvID"].ToString();
                this.IvaExcluidoInd.Value = Convert.ToBoolean(dr["IvaExcluidoInd"]);
                if(!string.IsNullOrEmpty(dr["LineaPresupuestoID"].ToString()))
                    this.LineaPresupuestoID.Value = dr["MvtoTipoInvID"].ToString();
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_faConceptos()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.TipoConcepto = new UDTSQL_tinyint();
            this.ConceptoCargoID = new UDT_BasicID();
            this.ConceptoCargoDesc = new UDT_Descriptivo();
            this.CuentaID = new UDT_BasicID();
            this.CuentaDesc = new UDT_Descriptivo();
            this.MvtoTipoInvID = new UDT_BasicID();
            this.MvtoTipoInvDesc = new UDT_Descriptivo();
            this.IvaExcluidoInd = new UDT_SiNo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
        }

        public DTO_faConceptos(DTO_MasterBasic basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_faConceptos(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDTSQL_tinyint TipoConcepto { get; set; }

        [DataMember]
        public UDT_BasicID ConceptoCargoID { get; set; }

        [DataMember]
        public UDT_Descriptivo ConceptoCargoDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_BasicID MvtoTipoInvID { get; set; }

        [DataMember]
        public UDT_Descriptivo MvtoTipoInvDesc { get; set; }

        [DataMember]
        public UDT_SiNo IvaExcluidoInd { get; set; }

    }
}

