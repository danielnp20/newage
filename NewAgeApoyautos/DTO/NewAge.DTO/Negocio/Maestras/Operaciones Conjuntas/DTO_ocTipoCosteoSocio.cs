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
    /// Models DTO_ocTipoCosteoSocio
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ocTipoCosteoSocio : DTO_MasterComplex
    {
        #region ocTipoCosteoSocio
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ocTipoCosteoSocio(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.SocioDesc.Value = dr["SocioDesc"].ToString();
                    this.ActividadDesc.Value = dr["ActividadDesc"].ToString();
                    this.LineaPresupuestoDesc.Value = dr["LineaPresupuestoDesc"].ToString();
                    this.TipoCostoDesc.Value = dr["TipoCostoDesc"].ToString();
                }
                this.SocioID.Value = dr["SocioID"].ToString();
                this.ActividadID.Value = dr["ActividadID"].ToString();
                this.LineaPresupuestoID.Value = dr["LineaPresupuestoID"].ToString();
                this.TipoCostoID.Value = dr["TipoCostoID"].ToString();
                this.IndExcluye.Value = Convert.ToBoolean(dr["IndExcluye"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ocTipoCosteoSocio()
            : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.SocioID = new UDT_BasicID();
            this.SocioDesc = new UDT_Descriptivo();
            this.ActividadID = new UDT_BasicID();
            this.ActividadDesc = new UDT_Descriptivo();
            this.LineaPresupuestoID = new UDT_BasicID();
            this.LineaPresupuestoDesc = new UDT_Descriptivo();
            this.TipoCostoID = new UDT_BasicID();
            this.TipoCostoDesc = new UDT_Descriptivo();
            this.IndExcluye = new UDT_SiNo();
        }

        public DTO_ocTipoCosteoSocio(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ocTipoCosteoSocio(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }
        #endregion

        [DataMember]
        public UDT_BasicID SocioID { get; set; }

        [DataMember]
        public UDT_Descriptivo SocioDesc { get; set; }

        [DataMember]
        public UDT_BasicID ActividadID { get; set; }

        [DataMember]
        public UDT_Descriptivo ActividadDesc { get; set; }

        [DataMember]
        public UDT_BasicID LineaPresupuestoID { get; set; }

        [DataMember]
        public UDT_Descriptivo LineaPresupuestoDesc { get; set; }

        [DataMember]
        public UDT_BasicID TipoCostoID { get; set; }

        [DataMember]
        public UDT_Descriptivo TipoCostoDesc { get; set; }

        [DataMember]
        public UDT_SiNo IndExcluye { get; set; }

    }
}
