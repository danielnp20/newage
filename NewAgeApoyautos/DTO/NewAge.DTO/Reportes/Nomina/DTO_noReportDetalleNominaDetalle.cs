using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.Reportes;
using NewAge.DTO.UDT;
using NewAge.Librerias.Project;
using NewAge.DTO.Attributes;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_acActivoControl
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportDetalleNominaDetalle 
    {
        #region Constructor

        public DTO_noReportDetalleNominaDetalle(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Base"].ToString()))
                    {this.Base = Convert.ToDecimal(dr["Base"]);}
                if (!string.IsNullOrWhiteSpace(dr["Valor"].ToString()))
                    this.Valor = Convert.ToDecimal(dr["Valor"]);
                if (!string.IsNullOrWhiteSpace(dr["Dias1"].ToString()))
                    this.DiasDescanso = (dr["Dias1"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["ConceptoNOID"].ToString()))
                    this.ConceptoID = (dr["ConceptoNOID"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["ConceptoDesc"].ToString()))
                    this.ConceptoDesc = (dr["ConceptoDesc"].ToString());
                if (!string.IsNullOrWhiteSpace(dr["NumeroDoc"].ToString()))
                    this.NumeroDoc = Convert.ToInt32(dr["NumeroDoc"]);
            }
            catch (Exception e)
            { ; }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noReportDetalleNominaDetalle()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.FechaIngreso = new DateTime();
            this.FechaRetiro = new DateTime();
            this.Base = new decimal();
            this.NumeroDoc = new int();
            this.Valor = new decimal();
        }
        #endregion
        #region Propiedades

        [DataMember]
        public int NumeroDoc { get; set; }

        [DataMember]
        public string EmpleadoID { get; set; }

        [DataMember]
        public string EmpleadoDesc { get; set; }

        [DataMember]
        public DateTime FechaIngreso { get; set; }

        [DataMember]
        public DateTime FechaRetiro { get; set; }

        [DataMember]
        public string DiasDescanso { get; set; }

        [DataMember]
        public string ConceptoID { get; set; }

        [DataMember]
        public string ConceptoDesc { get; set; }

        [DataMember]
        public decimal Base { get; set; }

        [DataMember]
        public decimal Devengos { get; set; }

        [DataMember]
        public decimal Valor { get; set; }

        [DataMember]
        public decimal Deducciones { get; set; }

        #endregion
    }
}
