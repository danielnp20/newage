using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using NewAge.DTO.UDT;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noProvisionDeta
    {
        #region Contructor
        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noProvisionDeta(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"].ToString());
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.Periodo.Value = Convert.ToDateTime(dr["Periodo"].ToString());
                this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                this.ContratoNOID.Value = Convert.ToInt32(dr["ContratoNOID"].ToString());
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                this.ConceptoNODesc.Value = dr["Descriptivo"].ToString();
                this.DiasTrabajados.Value = Convert.ToInt32(dr["DiasTrabajados"].ToString());
                this.DiasProvision.Value = Convert.ToInt32(dr["DiasProvision"].ToString());
                this.Sueldo.Value = Convert.ToDecimal(dr["Sueldo"]);
                this.AuxilioTransporte.Value = Convert.ToDecimal(dr["AuxilioTransporte"]);
                this.BaseNeta.Value = Convert.ToDecimal(dr["BaseNeta"]);
                this.BaseVariable.Value = Convert.ToDecimal(dr["BaseVariable"]);
                this.VlrConsolidadoINI.Value = Convert.ToDecimal(dr["VlrConsolidadoINI"]);
                this.VlrProvisionINI.Value = Convert.ToDecimal(dr["VlrProvisionINI"]);

                if (!string.IsNullOrWhiteSpace(dr["VlrAnticipoINI"].ToString()))
                    this.VlrAnticipoINI.Value = Convert.ToDecimal(dr["VlrAnticipoINI"]);

                this.VlrPagosMES.Value = Convert.ToDecimal(dr["VlrPagosMES"]);
                this.VlrProvisionMES.Value = Convert.ToDecimal(dr["VlrProvisionMES"]);
                this.VlrConsolidadoMES.Value = Convert.ToDecimal(dr["VlrConsolidadoMES"]);

                if (!string.IsNullOrWhiteSpace(dr["VlrAnticipoMES"].ToString()))
                    this.VlrAnticipoMES.Value = Convert.ToDecimal(dr["VlrAnticipoMES"]);

                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"]);
               
            }
            catch (Exception e)
            {
                throw e;
            }          
        } 

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noProvisionDeta()
        {
            this.InitCols();
        }

        /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.NumeroDoc = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.Periodo = new UDTSQL_smalldatetime();
            this.EmpleadoID = new UDT_EmpleadoID();
            this.ContratoNOID = new UDT_ContratoNOID();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.ConceptoNODesc = new UDT_DescripTBase();
            this.DiasTrabajados = new UDTSQL_int();
            this.DiasProvision = new UDTSQL_int();
            this.Sueldo = new UDT_Valor();
            this.AuxilioTransporte = new UDT_Valor();
            this.BaseVariable = new UDT_Valor();
            this.BaseNeta = new UDT_Valor();
            this.VlrConsolidadoINI = new UDT_Valor();
            this.VlrProvisionINI = new UDT_Valor();
            this.VlrAnticipoINI = new UDT_Valor();
            this.VlrPagosMES = new UDT_Valor();
            this.VlrProvisionMES = new UDT_Valor();
            this.VlrConsolidadoMES = new UDT_Valor();
            this.VlrAnticipoMES = new UDT_Valor();
            this.Consecutivo = new UDT_Consecutivo();
        }
        #endregion


        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo NumeroDoc { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime Periodo { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_ContratoNOID ContratoNOID { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_DescripTBase ConceptoNODesc { get; set; }

        [DataMember]
        public UDTSQL_int DiasTrabajados { get; set; }

        [DataMember]
        public UDTSQL_int DiasProvision { get; set; }

        [DataMember]
        public UDT_Valor Sueldo { get; set; }

        [DataMember]
        public UDT_Valor AuxilioTransporte { get; set; }
        
        [DataMember]
        public UDT_Valor BaseVariable { get; set; }
        
        [DataMember]
        public UDT_Valor BaseNeta { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor VlrConsolidadoINI { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrProvisionINI { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAnticipoINI { get; set; }

        [DataMember]
        public UDT_Valor VlrPagosMES { get; set; }

        [DataMember]
        public UDT_Valor VlrProvisionMES { get; set; }
        
        [DataMember]
        [NotImportable]
        public UDT_Valor VlrConsolidadoMES { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Valor VlrAnticipoMES { get; set; }

        [DataMember]
        [NotImportable]
        public UDT_Consecutivo Consecutivo { get; set; }
               
    }
}
