using System;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportDetalleEmpleadoConcepto
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportDetalleEmpleadoConcepto
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReportDetalleEmpleadoConcepto(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Base"].ToString()))
                    {this.Base.Value = Math.Round(Convert.ToDecimal(dr["Base"].ToString()), 1);}
                this.Valor.Value = Math.Round(Convert.ToDecimal(dr["Valor"].ToString()), 0);
                if (!string.IsNullOrWhiteSpace(dr["ConceptoNOID"].ToString()))
                    { this.ConceptoNOID = (dr["ConceptoNOID"]).ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["ConceptoNODesc"].ToString()))
                { this.ConceptoNODesc = (dr["ConceptoNODesc"]).ToString(); }
                if (!string.IsNullOrWhiteSpace(dr["Tipo"].ToString()))
                    this.Tipo.Value = Convert.ToByte(dr["Tipo"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_noReportDetalleEmpleadoConcepto(IDataReader dr, bool isNullble)
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
        public DTO_noReportDetalleEmpleadoConcepto()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.Valor = new UDT_Valor();
            this.Base = new UDT_Valor();
            this.Tipo = new UDTSQL_tinyint();
            this.ValorDeducciones = new UDT_Valor();
            this.ValorDevengos = new UDT_Valor();
            this.ValorTotal = new UDT_Valor();
        }
        #endregion
        #region Propiedades

        [DataMember]
        public string ConceptoNOID { get; set; }
        [DataMember]
        public string ConceptoNODesc { get; set;}
        [DataMember]
        public string Dias1 { get; set; }
        [DataMember]
        public UDT_Valor Base { get; set; }
        [DataMember]
        public UDT_Valor Valor { get; set; }
        [DataMember]
        public UDTSQL_tinyint Tipo { get; set; }
        /// <summary>
        /// Campos Extras
        /// </summary>
        [DataMember]
        public UDT_Valor ValorDevengos { get; set; }
        [DataMember]
        public UDT_Valor ValorDeducciones { get; set; }

        [DataMember]
        public UDT_Valor ValorTotal { get; set; }
        #endregion
    }
}
