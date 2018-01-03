using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    [DataContract]
    [Serializable]
    public class DTO_noLiquidacionPreliminar 
    {
        #region Contructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noLiquidacionPreliminar(IDataReader dr)
        {
            this.InitCols();
            try
            {
                this.Consecutivo.Value = Convert.ToInt32(dr["Consecutivo"].ToString());
                this.EmpresaID.Value = dr["EmpresaID"].ToString();
                this.NumeroDoc.Value = Convert.ToInt32(dr["NumeroDoc"].ToString());
                this.ConceptoNOID.Value = dr["ConceptoNOID"].ToString();
                if (!string.IsNullOrEmpty(dr["Dias"].ToString()))  
                   this.Dias.Value = Convert.ToInt32(dr["Dias"].ToString());
                this.Base.Value = Math.Round(Convert.ToDecimal(dr["Base"].ToString()), 1);
                this.Valor.Value = Math.Round(Convert.ToDecimal(dr["Valor"].ToString()), 0);
                this.OrigenConcepto.Value = Convert.ToByte(dr["OrigenConcepto"].ToString());
                if (!string.IsNullOrEmpty(dr["FondoNOID"].ToString()))  
                    this.FondoNOID.Value = dr["FondoNOID"].ToString();
                this.ContratoNONovID.Value = dr["ContratoNONovID"].ToString();
                this.Numero.Value = Convert.ToInt32(dr["Numero"]);
                this.ConceptoNODesc.Value = dr["Descriptivo"].ToString();

                if (!string.IsNullOrEmpty(dr["DatoAdd1"].ToString()))
                    this.DatoAdd1.Value = dr["DatoAdd1"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd2"].ToString()))
                    this.DatoAdd2.Value = dr["DatoAdd2"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd3"].ToString()))
                    this.DatoAdd3.Value = dr["DatoAdd3"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd4"].ToString()))
                    this.DatoAdd4.Value = dr["DatoAdd4"].ToString();
                if (!string.IsNullOrEmpty(dr["DatoAdd5"].ToString()))
                    this.DatoAdd5.Value = dr["DatoAdd5"].ToString();

                if (!string.IsNullOrEmpty(dr["ValorAdd1"].ToString()))
                    this.ValorAdd1.Value = Convert.ToDecimal(dr["ValorAdd1"]);
                if (!string.IsNullOrEmpty(dr["ValorAdd2"].ToString()))
                    this.ValorAdd2.Value = Convert.ToDecimal(dr["ValorAdd2"]);
                if (!string.IsNullOrEmpty(dr["ValorAdd3"].ToString()))
                    this.ValorAdd3.Value = Convert.ToDecimal(dr["ValorAdd3"]);
                if (!string.IsNullOrEmpty(dr["ValorAdd4"].ToString()))
                    this.ValorAdd4.Value = Convert.ToDecimal(dr["ValorAdd4"]);
                if (!string.IsNullOrEmpty(dr["ValorAdd5"].ToString()))
                    this.ValorAdd5.Value = Convert.ToDecimal(dr["ValorAdd5"]);

                if (!string.IsNullOrEmpty(dr["Documento1"].ToString()))
                    this.Documento1.Value = Convert.ToInt32(dr["Documento1"]);
                if (!string.IsNullOrEmpty(dr["Documento2"].ToString()))
                    this.Documento2.Value = Convert.ToInt32(dr["Documento2"]);
                if (!string.IsNullOrEmpty(dr["Documento3"].ToString()))
                    this.Documento3.Value = Convert.ToInt32(dr["Documento3"]);
                if (!string.IsNullOrEmpty(dr["Documento4"].ToString()))
                    this.Documento4.Value = Convert.ToInt32(dr["Documento4"]);
                if (!string.IsNullOrEmpty(dr["Documento5"].ToString()))
                    this.Documento5.Value = Convert.ToInt32(dr["Documento5"]);
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noLiquidacionPreliminar()
        {
            this.InitCols();
        }

          /// <summary>
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Consecutivo = new UDT_Consecutivo();
            this.EmpresaID = new UDT_EmpresaID();
            this.NumeroDoc = new UDT_Consecutivo();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.Dias = new UDTSQL_int();
            this.Base = new UDT_Valor();
            this.Valor = new UDT_Valor();
            this.OrigenConcepto = new UDTSQL_tinyint();
            this.FondoNOID = new UDT_FondoNOID();
            this.ContratoNONovID = new UDT_ContratoNONovID();
            this.Numero = new UDT_Consecutivo();
            this.ConceptoNODesc = new UDT_DescripTBase();
            this.eg_noConceptoNOM = new UDT_EmpresaGrupoID();
            this.eg_noContratoNov = new UDT_EmpresaGrupoID();
            this.eg_noFondo = new UDT_EmpresaGrupoID();
            this.DatoAdd1 = new UDTSQL_char(20);
            this.DatoAdd2 = new UDTSQL_char(20);
            this.DatoAdd3 = new UDTSQL_char(20);
            this.DatoAdd4 = new UDTSQL_char(20);
            this.DatoAdd5 = new UDTSQL_char(20);
            this.ValorAdd1 = new UDT_Valor();
            this.ValorAdd2 = new UDT_Valor();
            this.ValorAdd3 = new UDT_Valor();
            this.ValorAdd4 = new UDT_Valor();
            this.ValorAdd5 = new UDT_Valor();
            this.Documento1 = new UDT_Consecutivo();
            this.Documento2 = new UDT_Consecutivo();
            this.Documento3 = new UDT_Consecutivo();
            this.Documento4 = new UDT_Consecutivo();
            this.Documento5 = new UDT_Consecutivo();
        }

        #endregion

        #region Propiedades

        [DataMember]
        public UDT_Consecutivo Consecutivo { get; set; }

        [DataMember]
        public UDT_EmpresaID EmpresaID { get; set; }

        [DataMember]
        public UDT_Consecutivo NumeroDoc { get; set; }

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_DescripTBase ConceptoNODesc { get; set; }

        [DataMember]
        public UDTSQL_int Dias { get; set; }

        [DataMember]
        public UDT_Valor Base { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDTSQL_tinyint OrigenConcepto { get; set; }

        [DataMember]
        public UDT_FondoNOID FondoNOID { get; set; }

        [DataMember]
        public UDT_ContratoNONovID ContratoNONovID { get; set; }

        [DataMember]
        public UDT_Consecutivo Numero { get; set; }

        [DataMember]
        public UDT_EmpresaGrupoID eg_noConceptoNOM { get; set; }

        [DataMember]
        public UDT_EmpresaGrupoID eg_noFondo { get; set; }
        
        [DataMember]
        public UDT_EmpresaGrupoID eg_noContratoNov { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd1 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd2 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd3 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd4 { get; set; }

        [DataMember]
        public UDTSQL_char DatoAdd5 { get; set; }

        [DataMember]
        public UDT_Valor ValorAdd1 { get; set; }

        [DataMember]
        public UDT_Valor ValorAdd2 { get; set; }

        [DataMember]
        public UDT_Valor ValorAdd3 { get; set; }

        [DataMember]
        public UDT_Valor ValorAdd4 { get; set; }

        [DataMember]
        public UDT_Valor ValorAdd5 { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento1 { get; set; }

        [DataMember]
        public UDT_Consecutivo Documento2 { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Documento3 { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Documento4 { get; set; }
        
        [DataMember]
        public UDT_Consecutivo Documento5 { get; set; }

        
        
        #endregion

    }
}
