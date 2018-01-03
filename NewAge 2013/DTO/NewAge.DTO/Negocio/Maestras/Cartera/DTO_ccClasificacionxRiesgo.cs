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
    /// Models DTO_ccClasificacionxRiesgo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ccClasificacionxRiesgo : DTO_MasterComplex
    {
        #region ccClasificacionxRiesgo
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ccClasificacionxRiesgo(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();
            try
            {
                if (!isReplica)
                {
                    this.ClaseCreditoDesc.Value = Convert.ToString(dr["ClaseCreditoDesc"].ToString());
                    this.CuentaRecDesc.Value = Convert.ToString(dr["CuentaRecDesc"].ToString());
                    this.CuentaProvDesc.Value = Convert.ToString(dr["CuentaProvDesc"].ToString());
                    this.CuentaCauDesc.Value = Convert.ToString(dr["CuentaCauDesc"].ToString());
                    this.CuentaProvINTDesc.Value = Convert.ToString(dr["CuentaProvINTDesc"].ToString());
                }

                this.ClaseCredito.Value = Convert.ToString(dr["ClaseCredito"].ToString());
                this.DiasVencidos.Value = Convert.ToInt32(dr["DiasVencidos"].ToString());
                this.Descriptivo.Value = Convert.ToString(dr["Descriptivo"].ToString());
                this.DiasRango.Value = Convert.ToInt32(dr["DiasRango"].ToString());
                this.ClaseRiesgo.Value = Convert.ToString(dr["ClaseRiesgo"].ToString());
                this.CtaReclasificaCAP.Value = Convert.ToString(dr["CtaReclasificaCAP"].ToString());
                this.CtaProvisionCAP.Value = Convert.ToString(dr["CtaProvisionCAP"].ToString());
                if (!string.IsNullOrEmpty(dr["PorcMinProvisionCAP"].ToString()))
                    this.PorcMinProvisionCAP.Value = Convert.ToDecimal(dr["PorcMinProvisionCAP"].ToString());
                if (!string.IsNullOrEmpty(dr["PorcMaxProvisionCAP"].ToString()))
                    this.PorcMaxProvisionCAP.Value = Convert.ToDecimal(dr["PorcMaxProvisionCAP"].ToString());
                if (!string.IsNullOrEmpty(dr["CtaCausaINT"].ToString()))
                    this.CtaCausaINT.Value = Convert.ToString(dr["CtaCausaINT"].ToString());
                if (!string.IsNullOrEmpty(dr["CtaProvisionINT"].ToString()))
                    this.CtaProvisionINT.Value = Convert.ToString(dr["CtaProvisionINT"].ToString());
                if (!string.IsNullOrEmpty(dr["FactorCesion"].ToString()))
                    this.FactorCesion.Value = Convert.ToDecimal(dr["FactorCesion"].ToString());
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_ccClasificacionxRiesgo() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.ClaseCredito = new UDT_BasicID();
            this.ClaseCreditoDesc = new UDT_Descriptivo();
            this.DiasVencidos=new UDTSQL_int();
            this.Descriptivo = new UDT_DescripTBase();
            this.DiasRango = new UDTSQL_int();
            this.ClaseRiesgo=new UDT_CodigoGrl2();
            this.CtaReclasificaCAP = new UDT_BasicID();
            this.CuentaRecDesc = new UDT_Descriptivo();
            this.CtaProvisionCAP = new UDT_BasicID();
            this.CuentaProvDesc = new UDT_Descriptivo();
            this.PorcMinProvisionCAP=new UDT_PorcentajeCarteraID();
            this.PorcMaxProvisionCAP=new UDT_PorcentajeCarteraID();
            this.CtaCausaINT = new UDT_BasicID();
            this.CuentaCauDesc = new UDT_Descriptivo();
            this.CtaProvisionINT = new UDT_BasicID();
            this.CuentaProvINTDesc = new UDT_Descriptivo();
            this.FactorCesion = new UDT_PorcentajeID();

        }

        public DTO_ccClasificacionxRiesgo(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_ccClasificacionxRiesgo(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
        
        [DataMember]
        public UDT_BasicID ClaseCredito { get; set; }
        
        [DataMember]
        public UDT_Descriptivo ClaseCreditoDesc { get; set; }
        
        [DataMember]
        public UDTSQL_int DiasVencidos { get; set; }

        [DataMember]
        public UDT_DescripTBase Descriptivo { get; set; }

        [DataMember]
        public UDTSQL_int DiasRango { get; set; }
        
        [DataMember]
        public UDT_CodigoGrl2 ClaseRiesgo { get; set; }
        
        [DataMember]
        public UDT_BasicID CtaReclasificaCAP { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CuentaRecDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CtaProvisionCAP { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CuentaProvDesc { get; set; }
        
        [DataMember]
        public UDT_PorcentajeCarteraID PorcMinProvisionCAP { get; set; }
        
        [DataMember]
        public UDT_PorcentajeCarteraID PorcMaxProvisionCAP { get; set; }
        
        [DataMember]
        public UDT_BasicID CtaCausaINT { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CuentaCauDesc { get; set; }
        
        [DataMember]
        public UDT_BasicID CtaProvisionINT { get; set; }
        
        [DataMember]
        public UDT_Descriptivo CuentaProvINTDesc { get; set; }
        
        [DataMember]
        public UDT_PorcentajeID FactorCesion { get; set; }

    }

}
