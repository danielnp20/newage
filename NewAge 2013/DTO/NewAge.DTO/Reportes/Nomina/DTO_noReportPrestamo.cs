using System;
using System.Runtime.Serialization;
using System.Data;
using NewAge.DTO.UDT;

namespace NewAge.DTO.Negocio
{
    /// <summary>
    /// Class Error:
    /// Models DTO_noReportPrestamo
    /// </summary>
    [DataContract]
    [Serializable]
    public class DTO_noReportPrestamo
    {
        #region Constructor

        /// <summary>
        /// Constuye el DTO a partir de un resultado de base de datos
        /// </summary>
        /// <param name="?"></param>
        public DTO_noReportPrestamo(IDataReader dr)
        {
            InitCols();
            try
            {
                if (!string.IsNullOrWhiteSpace(dr["Numero"].ToString()))
                    this.Numero = Convert.ToInt32(dr["Numero"]);
                if (!string.IsNullOrWhiteSpace(dr["EmpleadoID"].ToString()))
                    this.EmpleadoID.Value = dr["EmpleadoID"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["Descriptivo"].ToString()))
                    this.Descriptivo.Value = dr["Descriptivo"].ToString();
                if (!string.IsNullOrWhiteSpace(dr["VlrPrestamo"].ToString()))
                    this.VlrPrestamo.Value = Convert.ToDecimal(dr["VlrPrestamo"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrCuota"].ToString()))
                    this.VlrCuota.Value = Convert.ToDecimal(dr["VlrCuota"]);
                if (!string.IsNullOrWhiteSpace(dr["VlrAbono"].ToString()))
                    this.VlrAbono.Value = Convert.ToDecimal(dr["VlrAbono"]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public DTO_noReportPrestamo(IDataReader dr, bool isNullble)
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
        public DTO_noReportPrestamo()
        {
            InitCols();
        }
        /// Inicializa las columnas
        /// </summary>
        private void InitCols()
        {
            this.EmpleadoID = new UDT_EmpleadoID();
            this.Descriptivo = new UDT_DescripTExt();
            this.VlrPrestamo = new UDT_Valor();
            this.VlrCuota = new UDT_Valor();
            this.VlrAbono = new UDT_Valor();
        }
        #endregion
        #region Propiedades

        [DataMember]
        public int Numero { get; set; }

        [DataMember]
        public UDT_EmpleadoID EmpleadoID { get; set; }

        [DataMember]
        public UDT_DescripTExt Descriptivo { get; set; }

        [DataMember]
        public UDT_Valor VlrPrestamo { get; set; }

        [DataMember]
        public UDT_Valor VlrCuota { get; set; }

        [DataMember]
        public UDT_Valor VlrAbono { get; set; }
        #endregion
    }
}
