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
    /// Models DTO_coIvaRetencion
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_coIvaRetencion : DTO_MasterComplex 
    {
        #region DTO_coIvaRetencion
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_coIvaRetencion(IDataReader dr, DTO_aplMaestraPropiedades mp, bool isReplica)
            : base(dr, mp)
        {
            InitCols();

            try
            {
                if (!isReplica)
                {
                    this.RegFisEmpresaDesc.Value = dr["RegFisEmpresaDesc"].ToString();
                    this.RegFisTerceroDesc.Value = dr["RegFisTerceroDesc"].ToString();
                    this.CuentaIVADesc.Value = dr["CuentaIVADesc"].ToString();
                    this.CuentaReteIVADesc.Value = dr["CuentaReteIVADesc"].ToString();
                }

                this.RegimenFiscalEmpresaID.Value = dr["RegimenFiscalEmpresaID"].ToString();
                this.RegimenFiscalTerceroID.Value = dr["RegimenFiscalTerceroID"].ToString();
                this.CuentaIVA.Value = dr["CuentaIVA"].ToString();
                this.CuentaReteIVA.Value = dr["CuentaReteIVA"].ToString();
            }
            catch(Exception e)
            {
               throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_coIvaRetencion() : base()
        {
            InitCols();
        }

        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.RegimenFiscalEmpresaID = new UDT_BasicID();
            this.RegFisEmpresaDesc = new UDT_Descriptivo();
            this.RegimenFiscalTerceroID = new UDT_BasicID();
            this.RegFisTerceroDesc = new UDT_Descriptivo();
            this.CuentaIVA = new UDT_BasicID();
            this.CuentaIVADesc = new UDT_Descriptivo();
            this.CuentaReteIVA = new UDT_BasicID();
            this.CuentaReteIVADesc = new UDT_Descriptivo();
           
        }

        public DTO_coIvaRetencion(DTO_MasterComplex basic)
            : base(basic)
        {
            InitCols();
        }

        public DTO_coIvaRetencion(DTO_aplMaestraPropiedades masterProperties)
            : base(masterProperties)
        {
        }  
        #endregion 
 
        [DataMember]
        public UDT_BasicID RegimenFiscalEmpresaID { get; set; }

        [DataMember]
        public UDT_Descriptivo RegFisEmpresaDesc { get; set; }

        [DataMember]
        public UDT_BasicID RegimenFiscalTerceroID { get; set; }

        [DataMember]
        public UDT_Descriptivo RegFisTerceroDesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaIVA { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaIVADesc { get; set; }

        [DataMember]
        public UDT_BasicID CuentaReteIVA  { get; set; }

        [DataMember]
        public UDT_Descriptivo CuentaReteIVADesc  { get; set; }

    }

}
