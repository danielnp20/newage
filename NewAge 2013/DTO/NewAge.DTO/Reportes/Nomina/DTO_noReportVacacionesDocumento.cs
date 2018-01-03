using System;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportVacacionesDocumento
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_ReportVacacionesDocumento
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_ReportVacacionesDocumento(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["PeriodoDescansoInicial"].ToString()))
                    this.PeriodoDescansoInicial.Value = Convert.ToDateTime(dr["PeriodoDescansoInicial"]);
                if (!string.IsNullOrWhiteSpace(dr["PeriodoDescasonFinal"].ToString()))
                    this.PeriodoDescasonFinal.Value = Convert.ToDateTime(dr["PeriodoDescasonFinal"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_ReportVacacionesDocumento(IDataReader dr, bool isNullble)
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
        public DTO_ReportVacacionesDocumento()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.PeriodoDescansoInicial = new UDTSQL_smalldatetime();
            this.PeriodoDescasonFinal = new UDTSQL_smalldatetime();
        }
        #endregion
        #region Propiedades

        [DataMember]
        public UDTSQL_smalldatetime PeriodoDescansoInicial { get; set; }

        [DataMember]
        public UDTSQL_smalldatetime PeriodoDescasonFinal { get; set; }

        #endregion
    }
}
