using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportNominaResumidoXConcepto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportNominaResumidoXConcepto
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReportNominaResumidoXConcepto(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Descriptivo"].ToString()))
                    this.Concepto.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["ConceptoNOID"].ToString()))
                    this.Codigo.Value = dr["ConceptoNOID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Tipo"].ToString()))
                    this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor.Value = Convert.ToDecimal(dr["Valor"]);
            }
            catch (Exception e)
            { 
                throw e; 
            }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noReportNominaResumidoXConcepto()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Devengos = new UDT_Valor();
            this.Deducciones = new UDT_Valor();
            this.Concepto = new UDT_DescripTExt();
            this.Codigo = new UDT_ConceptoNOID();
            this.Tipo = new UDTSQL_tinyint();
            this.Valor = new UDT_Valor();
            this.VlrTotal = new UDT_Valor();
        }
        #endregion
        #region Propiedades

        //Campos extras

        [DataMember]
        [AllowNull]
        public UDT_Valor Devengos { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Deducciones { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_DescripTExt Concepto { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_ConceptoNOID Codigo { get; set; }

        [DataMember]
        [AllowNull]
        public DateTime FechaInicial { get; set; }

        [DataMember]
        [AllowNull]
        public DateTime FechaFinal { get; set; }

        [DataMember]
        [AllowNull]
        public UDTSQL_tinyint Tipo { get; set; }

        [DataMember]
        [AllowNull]
        public UDT_Valor VlrTotal { get; set; }

        #endregion
    }
}
