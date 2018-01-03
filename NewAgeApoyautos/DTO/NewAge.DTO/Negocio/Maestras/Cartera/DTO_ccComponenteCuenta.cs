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
    /// Models DTO_ccComponenteCuenta
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccComponenteCuenta : DTO_MasterComplex
    {
        #region DTO_ccComponenteCuenta
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccComponenteCuenta(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ClaseCreditoDesc.Value = dr["ClaseCreditoDesc"].ToString();
                    this.CuentaDesc.Value = dr["CuentaDesc"].ToString();
                    this.ComponenteCarteraDesc.Value = dr["ComponenteCarteraDesc"].ToString();
                    this.CuentaIngresoDesc.Value = dr["CuentaIngresoDesc"].ToString();
                    this.CtaRecursosTercerosDesc.Value = dr["CtaRecursosTercerosDesc"].ToString();
                    this.CtaRecursosCesionDesc.Value = dr["CtaRecursosCesionDesc"].ToString();
                    this.CuentaDistribuyeDesc.Value = dr["CuentaDistribuyeDesc"].ToString();
                    this.TerceroDesc.Value = dr["TerceroDesc"].ToString();
                }

                this.ClaseCredito.Value = dr["ClaseCredito"].ToString();
                this.CuentaID.Value = dr["CuentaID"].ToString();
                this.CuentaIngreso.Value = dr["CuentaIngreso"].ToString();
                this.CtaRecursosTerceros.Value = dr["CtaRecursosTerceros"].ToString();
                this.CtaRecursosCesion.Value = dr["CtaRecursosCesion"].ToString();
                this.ComponenteCarteraID.Value = dr["ComponenteCarteraID"].ToString();
                if (!string.IsNullOrEmpty(dr["TipoEstado"].ToString()))
                    this.TipoEstado.Value = Convert.ToByte(dr["TipoEstado"].ToString());
                if (!string.IsNullOrEmpty(dr["CuentaControl"].ToString()))
                    this.CuentaControl.Value = Convert.ToByte(dr["CuentaControl"].ToString());
                this.TerceroID.Value = dr["TerceroID"].ToString();
                this.AsesorInd.Value = Convert.ToBoolean(dr["AsesorInd"]);
                this.CuentaDistribuye.Value = dr["CuentaDistribuye"].ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccComponenteCuenta() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.CuentaID                 = new UDT_BasicID();
            this.CuentaDesc               = new UDT_Descriptivo();
            this.CuentaIngreso            = new UDT_BasicID();
            this.CuentaIngresoDesc        = new UDT_Descriptivo();
            this.CtaRecursosTerceros      = new UDT_BasicID();
            this.CtaRecursosTercerosDesc  = new UDT_Descriptivo();
            this.CtaRecursosCesion        = new UDT_BasicID();
            this.CtaRecursosCesionDesc = new UDT_Descriptivo();
            this.ComponenteCarteraID      = new UDT_BasicID();
            this.ComponenteCarteraDesc    = new UDT_Descriptivo();
            this.ClaseCredito               = new UDT_BasicID();
            this.ClaseCreditoDesc           = new UDT_Descriptivo();
            this.TipoEstado               = new UDTSQL_tinyint();
            this.CuentaControl            = new UDTSQL_tinyint();
            this.TerceroID                = new UDT_BasicID();
            this.TerceroDesc              = new UDT_Descriptivo();
            this.AsesorInd                = new UDT_SiNo();
            this.CuentaDistribuye = new UDT_BasicID();
            this.CuentaDistribuyeDesc = new UDT_Descriptivo();
        }

        public DTO_ccComponenteCuenta(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccComponenteCuenta(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 

        [DataMember]
        public UDT_BasicID ClaseCredito { get; set; }

        [DataMember]
        public UDT_Descriptivo ClaseCreditoDesc { get; set; }

        [DataMember]
        public UDT_BasicID ComponenteCarteraID { get; set; }

        [DataMember]
        public UDT_Descriptivo ComponenteCarteraDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint TipoEstado { get; set; }

        [DataMember]
        public UDT_BasicID CuentaIngreso { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaIngresoDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CtaRecursosTerceros { get; set; }

        [DataMember]
        public UDT_Descriptivo CtaRecursosTercerosDesc { get; set; }

        [DataMember]
        public UDT_BasicID CtaRecursosCesion { get; set; }

        [DataMember]
        public UDT_Descriptivo CtaRecursosCesionDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaID { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDesc { get; set; }

        [DataMember]
        public UDTSQL_tinyint CuentaControl { get; set; }

        [DataMember]
        public UDT_BasicID TerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo TerceroDesc { get; set; }

        [DataMember]
        public UDT_SiNo AsesorInd { get; set; }

        [DataMember]
        public UDT_BasicID CuentaDistribuye { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaDistribuyeDesc { get; set; }

        
    }

}
