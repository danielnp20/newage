using System;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportDetalleNominaXConcepto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportDetalleNominaXConcepto
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReportDetalleNominaXConcepto(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoID"].ToString()))
                    { this.EmpleadoID = dr["EmpleadoID"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoDesc"].ToString()))
                    { this.EmpleadoDesc = dr["EmpleadoDesc"].ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["Base"].ToString()))
                    {this.Base.Value = Math.Round(Convert.ToDecimal(dr["Base"].ToString()), 1);}
                this.Valor.Value =  Math.Round(Convert.ToDecimal(dr["Valor"].ToString()), 0);

                if (!string.IsNullOrWhiteSpace(dr["ConceptoNOID"].ToString()))
                    { this.ConceptoNOID.Value = (dr["ConceptoNOID"]).ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["Descriptivo"].ToString()))
                    {this.ConceptoNODesc.Value = (dr["Descriptivo"]).ToString(); }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_noReportDetalleNominaXConcepto(IDataReader dr, bool isNullble)
        {
            InitCols();
            try
            {

            }
            catch (Exception e)
            { ; }
        }
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public DTO_noReportDetalleNominaXConcepto()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Valor = new UDT_Valor();
            this.Base = new UDT_Valor();
            this.ConceptoNOID = new UDT_ConceptoNOID();
            this.ConceptoNODesc = new UDT_DescripTExt();
            
        }
        #endregion
        #region Propiedades

        [DataMember]
        public UDT_ConceptoNOID ConceptoNOID { get; set; }

        [DataMember]
        public UDT_DescripTExt ConceptoNODesc { get; set; }

        [DataMember]
        public string EmpleadoID { get; set; }

        [DataMember]
        public string EmpleadoDesc { get; set; }

        [DataMember]
        public UDT_Valor Base { get; set; }

        [DataMember]
        public UDT_Valor Valor { get; set; }

        [DataMember]
        public UDT_Valor VlrDevengos { get; set; }

        [DataMember]
        public UDT_Valor VlrDeducciones { get; set; }

        [DataMember]
        public DateTime FechaIni { get; set; }

        [DataMember]
        public DateTime FechaFin { get; set; }

       
        #endregion
    }
}
